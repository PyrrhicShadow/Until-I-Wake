using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PyrrhicSilva.Interactable;
using Cinemachine;
using TMPro;
using System;

namespace PyrrhicSilva
{
    public enum Cycle
    {
        Guided, Standard, Nightmare
    }

    public enum Day
    {
        Mon, Tue, Wed, Thur, Fri, Sat, Sun
    }

    public enum Task
    {
        Asleep, WakeUp,
        Morning,
        CookBreakfast, EatBreakfast,
        Work,
        CookDinner, TakePan, EatDinner,
        Unwind,
        Night,
        Bedtime,
        Leave, Arrive,
        Interview,
    }

    public enum Step
    {
        Begin, Perform, Finish
    }

    [Serializable]
    class Objective
    {
        [SerializeField] Task _task;
        [SerializeField] Step _step;
        public Task task { get { return _task; } protected set { _task = value; } }
        public Step step { get { return _step; } protected set { _step = value; } }

        public Objective()
        {
            task = Task.Asleep;
            step = Step.Begin;
        }

        public void NewObjective(Task task, Step step)
        {
            this.task = task;
            this.step = step;
        }

        public void NewObjective(Task task)
        {
            this.task = task;
            this.step = Step.Begin;
        }

        public override string ToString()
        {
            return "" + task + ": " + step;
        }
    }

    public class AgendaManager : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [Header("Temporal Positioning")]
        [SerializeField] internal Cycle cycle = Cycle.Guided;
        [SerializeField] internal Day day = Day.Mon;
        [SerializeField] internal Objective objective;
        [Header("Agenda Setup")]
        [SerializeField] Canvas agendaCanvas;
        [SerializeField] TMP_Text objectiveText;
        [SerializeField] TMP_Text subObjectiveText;
        [SerializeField] TMP_Text[] clocks;
        [Header("Wake Up")]
        [SerializeField] WakeUpGame wakeUpGame;
        [SerializeField] AudioPlayable alarmClock;
        [SerializeField] CinemachineVirtualCamera wakeUpCamera;
        [Header("Get Ready")]
        [SerializeField] Container dresser;
        [SerializeField] DoorInteractable bathroomDoor;
        [SerializeField] Container fridge;
        [SerializeField] CookingInteractable microwave;
        [SerializeField] Container placeSetting;
        [SerializeField] MealInteractable plateFood;
        [SerializeField] ChairInteractable diningTable;
        [SerializeField] Container sink;  
        [Header("Computer Work")]
        [SerializeField] ComputerInteractable computer;
        [Header("Unwind")]
        [SerializeField] Container cabinet; 
        [SerializeField] CookingInteractable stove;
        [SerializeField] MealInteractable panFood; 
        [SerializeField] ComputerInteractable couch;
        [Header("Bedtime")]
        [SerializeField] AudioPlayable speaker;
        [SerializeField] Interactable.Interactable bed;
        [SerializeField] CinemachineVirtualCamera sleepCamera; 
        [SerializeField] DreamManager dreamManager;
        [Header("Travel")]
        [SerializeField] DoorInteractable frontDoor;


        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }
            objective = new Objective();
        }

        private void Start()
        {
            if (dreamManager == null) {
                dreamManager = GameObject.FindWithTag("Dream").GetComponent<DreamManager>(); 
            }

            // wake up 
            alarmClock.DisableTrigger(); 

            // get ready 
            dresser.DisableTrigger();
            bathroomDoor.DisableTrigger();
            fridge.DisableTrigger();
            microwave.enabled = false; 
            microwave.DisableTrigger();
            placeSetting.DisableTrigger();
            plateFood.DisableTrigger();
            sink.DisableTrigger(); 

            // computer work 
            computer.DisableTrigger();

            // unwind 
            stove.DisableTrigger(); 
            cabinet.DisableTrigger();
            panFood.DisableTrigger(); 
            couch.DisableTrigger();

            // bedtime 
            speaker.DisableTrigger();
            bed.DisableTrigger();
            dreamManager.enabled = false; 

            // travel
            // frontDoor.DisableTrigger(); 
        }

        void UpdateClocks(string time)
        {
            foreach (TMP_Text textbox in clocks)
            {
                textbox.text = time;
            }
        }

        public void IncrementDay()
        {
            if (day < Day.Sun)
            {
                day++;
            }
            else
            {
                if (cycle < Cycle.Nightmare)
                {
                    day = 0;
                    cycle++;
                }
                else
                {
                    Debug.Log("Game complete!");
                }
            }
        }

        public void UpdateAgendaText(string obj, string sub) {
            objectiveText.text = obj; 
            subObjectiveText.text = sub; 
        }

        public void UpdateAgendaText(string sub) {
            subObjectiveText.text = sub; 
        }

        [ContextMenu("Refresh Objective")]
        public void TaskComplete()
        {
            Debug.Log(objective.ToString());
            gameManager.SaveGame();

            switch (objective.task)
            {
                case Task.Asleep:
                    BeginDay();
                    break;
                case Task.WakeUp:
                    WakeUp();
                    break;
                // get ready
                case Task.Morning:
                    Morning();
                    break;
                case Task.CookBreakfast:
                    BeginMeal();
                    break;
                case Task.EatBreakfast:
                    EatFood();
                    break;
                // work
                case Task.Work:
                    Work();
                    break;
                // unwind 
                case Task.CookDinner:
                    BeginMeal();
                    break;
                case Task.TakePan:
                    BeginMeal();
                    break;
                case Task.EatDinner:
                    EatFood();
                    break;
                case Task.Unwind:
                    Unwind();
                    break;
                // Bedtime 
                case Task.Night:
                    Night();
                    break;
                case Task.Bedtime:
                    Bedtime();
                    break;
                case Task.Leave:
                    Leave();
                    break;
                case Task.Arrive:
                    Arrive();
                    break;
                default:
                    BeginDay();
                    break;
            }
        }

        private void BeginDay()
        {
            // setup 
            gameManager.CharacterMovement(false);

            // start day with camera laying on the bed
            wakeUpCamera.Priority += 20;

            // update objectives
            objective.NewObjective(Task.WakeUp);
            TaskComplete();
        }

        /******* Wake Up *******/

        private void WakeUp()
        {
            alarmClock.EnableTrigger(); 
            switch (objective.step)
            {
                case Step.Begin:
                    Asleep();
                    break;
                case Step.Perform:
                    OpenEyes();
                    break;
                case Step.Finish:
                    AlarmOff();
                    break;
            }
        }

        private void Asleep()
        {
            objective.NewObjective(Task.WakeUp, Step.Perform);
            gameManager.CharacterMovement(false);

            switch (day)
            {
                case > Day.Fri:
                    wakeUpGame.StartInterviewMinigame();
                    break;
                default:
                    wakeUpGame.StartNormalMinigame();
                    break;
            }
        }

        // if (agenda.day < Day.Thur)
        //     {
        //         int rand = Random.Range(0, 7);
        //         if (((int)agenda.day + rand) % 2 == 0)
        //         {
        //             StandardInterview();
        //         }
        //         else
        //         {
        //             StandardWeek();
        //         }
        //     }
        //     else
        //     {
        //         FinalNightmare();
        //     }

        void OpenEyes()
        {
            // advance objective 
            objective.NewObjective(Task.WakeUp, Step.Finish);
            UpdateAgendaText("Wake up", "Turn off your alarm"); 

            // advance clocks 
            UpdateClocks("08:00");

            gameManager.CharacterMovement(true);

            // pass task completion to alarm clock
            alarmClock.InteractAction();
        }

        private void AlarmOff()
        {
            alarmClock.enabled = false;
            alarmClock.DisableTrigger(); 
            objective.NewObjective(Task.Morning);
            StartCoroutine(getOutOfBed());
        }

        IEnumerator getOutOfBed()
        {
            gameManager.CharacterMovement(false);
            wakeUpCamera.gameObject.GetComponent<Animator>().Play("WakeUp");
            yield return new WaitForSeconds(4.5f);
            wakeUpCamera.Priority -= 20;
            yield return new WaitForSeconds(2f);
            gameManager.CharacterMovement(true);
            TaskComplete();
        }

        /****** Morning ******/

        void Morning()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    MorningClothes();
                    break;
                case Step.Perform:
                    MorningBathroom();
                    break;
            }
        }
        void MorningClothes()
        {
            // advance to next objective
            objective.NewObjective(Task.Morning, Step.Perform);
            UpdateAgendaText("Get ready for the day", "Get dressed \nUse the bathroom"); 

            dresser.enabled = true; 
            dresser.EnableTrigger();
            // dresser.Store(0); 
        }

        void MorningBathroom()
        {
            // advance to next objective 
            objective.NewObjective(Task.CookBreakfast);

            bathroomDoor.enabled = true; 
            bathroomDoor.EnableTrigger();
        }

        /****** Meal ******/
        void BeginMeal()
        {
            if (objective.task == Task.TakePan)
            {
                CookFood();
            }
            else
            {
                switch (objective.step)
                {
                    case Step.Begin:
                        TakeFromFridge();
                        break;
                    case Step.Perform:
                        PrepareFood();
                        break;
                    case Step.Finish:
                        TakeMeal();
                        break;
                }
            }
        }

        void TakeFromFridge()
        { 
            fridge.enabled = true; 
            fridge.EnableTrigger();
            // fridge.Store(0);

            switch (objective.task)
            {
                case Task.CookBreakfast:
                    BeginBreakfast();
                    break;
                case Task.CookDinner:
                    BeginDinner();
                    break;
            }
        }

        void BeginBreakfast()
        {
            // advance clocks
            UpdateClocks("08:27");
            switch (day)
            {
                case > Day.Fri:
                    // advance to next task
                    objective.NewObjective(Task.EatBreakfast);

                    if (day == Day.Sat)
                    {
                        UpdateClocks("12:27");
                    }
                    // update Objective Text
                    UpdateAgendaText("Grab breakfast \nEat breakfast");

                    break;

                default:
                    // advance to next task
                    objective.NewObjective(Task.CookBreakfast, Step.Perform);

                    // update Objective Text
                    UpdateAgendaText("Make breakfast \nEat breakfast");

                    break;
            }
        }

        void BeginDinner()
        {
            // update objective text
            UpdateAgendaText("Unwind", "Make Dinner \nEat dinner");

            switch (day)
            {
                // fridge.Store(1);
                case Day.Sun:
                    objective.NewObjective(Task.CookDinner, Step.Perform);
                    // PrepareFood();
                    break;
                case > Day.Thur:
                    // TakeFood();
                    objective.NewObjective(Task.EatDinner);
                    // update objective text
                    UpdateAgendaText("Grab Dinner \nEat dinner");
                    break;
                default:
                    // PrepareFood();
                    objective.NewObjective(Task.TakePan);
                    break;
            }
        }

        void PrepareFood()
        {
            switch (objective.task)
            {
                case Task.CookBreakfast:
                    MicrowaveFood();
                    break;
                case Task.CookDinner:
                    CookFood();
                    break;
            }
        }

        void MicrowaveFood()
        {
            microwave.enabled = true;
            microwave.EnableTrigger();
            switch (objective.task)
            {
                case Task.CookBreakfast:
                    // advance clocks 
                    UpdateClocks("08:38");

                    // advance objective 
                    objective.NewObjective(Task.CookBreakfast, Step.Finish);
                    break;
                case Task.CookDinner:
                    // advance clocks 

                    // advance objective
                    objective.NewObjective(Task.CookDinner, Step.Finish);
                    break;
            }
        }

        void CookFood()
        {
            if (objective.task == Task.TakePan)
            {
                TakePan();
            }
            else
            {
                switch (objective.step)
                {
                    case Step.Perform:
                        CookDinner();
                        break;
                    case Step.Finish:
                        TakeMeal();
                        break;
                }
            }

        }

        void TakePan()
        {
            // advance clocks

            // advance objective 
            objective.NewObjective(Task.CookDinner, Step.Perform);

            cabinet.enabled = true; 
            cabinet.EnableTrigger(); 
            gameManager.TemporaryTask();
        }

        void CookDinner()
        {
            // advance objective 
            objective.NewObjective(Task.CookDinner, Step.Finish);

            stove.enabled = true; 
            stove.EnableTrigger(); 
            gameManager.TemporaryTask();
        }

        void TakeMeal()
        {
            placeSetting.enabled = true; 
            placeSetting.EnableTrigger();
            switch (objective.task)
            {
                case Task.CookBreakfast:
                    UpdateClocks("08:39");
                    objective.NewObjective(Task.EatBreakfast);
                    break;
                case Task.CookDinner:
                    objective.NewObjective(Task.EatDinner);
                    break;
            }
        }

        void EatFood()
        {
            microwave.enabled = false;
            switch (objective.step)
            {
                case Step.Begin:
                    EatMeal();
                    break;
                case Step.Perform:
                    TakePlate();
                    break;
                case Step.Finish:
                    CleanUp();
                    break;
            }
        }

        void EatMeal()
        {
            // gameManager.TemporaryTask();
            diningTable.enabled = true; 
            diningTable.EnableTrigger(); 
            
            microwave.enabled = false; 
            switch (objective.task)
            {
                case Task.EatBreakfast:
                    EatBreakfast();
                    break;
                case Task.EatDinner:
                    EatDinner();
                    break;
            }
        }

        void EatBreakfast()
        {
            objective.NewObjective(Task.EatBreakfast, Step.Perform);
            diningTable.InteractAction(); 
            plateFood.EnableTrigger(); 
            UpdateClocks("08:46");
        }

        void EatDinner()
        {
            objective.NewObjective(Task.EatDinner, Step.Perform);
            if (day < Day.Fri) {
                panFood.EnableTrigger(); 
            }
            else { plateFood.EnableTrigger(); }
            UpdateClocks("17:37");
        }


        void TakePlate()
        {
            subObjectiveText.text = "Clean up";

            placeSetting.EnableTrigger(); 

            switch (objective.task)
            {
                case Task.EatBreakfast:
                    BreakfastPlate();
                    break;
                case Task.EatDinner:
                    DinnerPlate();
                    break;
            }
        }

        void BreakfastPlate()
        {
            objective.NewObjective(Task.EatBreakfast, Step.Finish);

            // gameManager.TemporaryTask();
        }

        void DinnerPlate()
        {
            objective.NewObjective(Task.EatDinner, Step.Finish);

            // gameManager.TemporaryTask();
        }

        void CleanUp()
        {
            sink.EnableTrigger(); 
            gameManager.GetUnSeated(); 
            switch (objective.task)
            {
                case Task.EatBreakfast:
                    BreakfastCleanUp();
                    break;
                case Task.EatDinner:
                    DinnerCleanUp();
                    break;
            }
        }

        void BreakfastCleanUp()
        {
            objective.NewObjective(Task.Work);

            // gameManager.TemporaryTask();
        }


        void DinnerCleanUp()
        {
            objective.NewObjective(Task.Unwind);

            // gameManager.TemporaryTask();
        }

        /***** Computer Work ******/

        [ContextMenu("Work")]
        void Work()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    WorkTime();
                    break;
                default:
                    ReturnFromWork();
                    break;
            }
        }
        void WorkTime()
        {
            objective.NewObjective(Task.Work, Step.Perform);
            UpdateAgendaText("Work", string.Empty);
            gameManager.SaveGame();
            UpdateClocks("09:00");
            computer.enabled = true;
            computer.EnableTrigger(); 
        }

        void ReturnFromWork()
        {
            UpdateClocks("17:00");
            gameManager.TeleportCharacter(computer.ExitTransform);
            gameManager.GetUnSeated();
            gameManager.CharacterMovement(true);
            objective.NewObjective(Task.CookDinner);
            TaskComplete();
        }

        /****** Unwind ******/

        void Unwind()
        {
            objective.NewObjective(Task.Unwind, Step.Perform);

            switch (day)
            {
                case Day.Sun:
                    TV();
                    break;
                case > Day.Fri:
                    UnwindInterview();
                    break;
                default:
                    TV();
                    break;
            }
        }
        void TV()
        {
            switch (objective.step)
            {
                case Step.Perform:
                    ReturnFromTV();
                    break;
                default:
                    UnwindTV();
                    break;
            }
        }

        [ContextMenu("Unwind TV")]
        void UnwindTV()
        {
            UpdateClocks("17:43");
            couch.enabled = true;
            couch.EnableTrigger(); 
            UpdateAgendaText("Play games on the TV");
            objective.NewObjective(Task.Unwind, Step.Perform);
            gameManager.SaveGame();
            gameManager.TemporaryTask();
        }

        void UnwindInterview()
        {
            UpdateAgendaText("Interact with other people(?) \nHide in your room");
            objective.NewObjective(Task.Night); 
            gameManager.TemporaryTask();
        }

        void ReturnFromTV()
        {
            UpdateClocks("23:30");
            gameManager.TeleportCharacter(couch.ExitTransform);
            gameManager.GetUnSeated();
            gameManager.CharacterMovement(true);
            objective.NewObjective(Task.Night); 
            TaskComplete();
        }

        /****** Bedtime *******/

        void Night()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    GetNightClothes();
                    break;
                case Step.Perform:
                    NightBathroom();
                    break;
            }
        }
        void GetNightClothes()
        {
            objective.NewObjective(Task.Night, Step.Perform);
            dresser.EnableTrigger();
            // dresser.Store(1); 
            UpdateAgendaText("Get ready for bed", "Grab bed clothes \nTake a shower");
        }

        void NightBathroom()
        {
            objective.NewObjective(Task.Bedtime);
            bathroomDoor.EnableTrigger();
        }

        void Bedtime()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    PrepareForBed();
                    break;
                case Step.Perform:
                    GetInBed();
                    break;
                case Step.Finish:
                    Sleep();
                    break;
            }
        }

        void PrepareForBed()
        {
            switch (day)
            {
                case Day.Fri:
                    GetInBed();
                    break;
                case Day.Sat:
                    GetInBed();
                    break;
                default:
                    SleepTunes();
                    break;
            }
        }

        void SleepTunes()
        {
            objective.NewObjective(Task.Bedtime, Step.Perform);
            UpdateClocks("23:04");
            speaker.EnableTrigger();
            UpdateAgendaText("Put on some tunes \nGo to sleep");
        }

        void GetInBed()
        {
            if (objective.step == Step.Begin)
            {
                UpdateAgendaText("No tunes tonight \nGo to sleep");
            }
            objective.NewObjective(Task.Bedtime, Step.Finish);
            UpdateClocks("23:14");
            bed.EnableTrigger();
            // gameManager.TemporaryTask();
        }

        void Sleep()
        {
            objective.NewObjective(Task.Asleep);
            UpdateAgendaText("Sleep", string.Empty); 
            IncrementDay();
            gameManager.SaveGame();

            // Animate sleeping 
            gameManager.CharacterMovement(false); 
            sleepCamera.Priority += 20; 
            StartCoroutine(layInBed()); 
        }

        IEnumerator layInBed() {

            yield return new WaitForSeconds(3);

            // Load dream sequence
            dreamManager.enabled = true;
            dreamManager.StartDream();
        }

        void Leave()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    break;
                case Step.Perform:
                    break;
            }
        }

        void Arrive()
        {
            switch (objective.step)
            {
                case Step.Begin:
                    ReturnFromTravel();
                    break;
                case Step.Perform:
                    // unpack your things
                    break;
            }
        }

        void ReturnFromTravel()
        {
            gameManager.TeleportCharacter(frontDoor.ExitTransform);
            objective.NewObjective(Task.Arrive, Step.Perform);
            TaskComplete();
        }

        
    }
}