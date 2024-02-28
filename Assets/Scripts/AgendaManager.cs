using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PyrrhicSilva.Interactable;
using Cinemachine;
using TMPro;

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
        MorningClothes, MorningBathroom, 
        BeginBreakfast, MakeBreakfast, TakeBreakfast, EatBreakfast, CleanBreakfast,
        Work, 
        BeginDinner, TakePan, MakeDinner, TakeDinner, EatDinner, CleanDinner,
        UnwindTV, 
        NightClothes, NightBathroom, 
        SleepTunes, Sleep, 
        PackDorm, LeaveDorm, ArriveHQ, 
        TakeDinnerHQ, UnwindHQ, 
        SleepHQ, 
        WakeUpHQ,
        TakeLunch, EatLunch, 
        PreInterview, Interview, 
        UnwindHQSad, 
        TakeBreakfastHQ, EatBreakfastHQ, 
        LeaveHQ, ArriveDorm, 
        EndTask,

    }

    public class AgendaManager : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [Header("Temporal Positioning")]
        [SerializeField] Cycle _cycle = Cycle.Guided;
        [SerializeField] Day _day = Day.Mon;
        [SerializeField] Task _task = Task.WakeUp;
        public Cycle cycle { get { return _cycle; } internal set { _cycle = value; } }
        public Day day { get { return _day; } internal set { _day = value; } }
        public Task task { get { return _task; } internal set { _task = value; } }
        [Header("Agenda Setup")]
        [SerializeField] Canvas agendaCanvas;
        [SerializeField] TMP_Text objectiveText;
        [SerializeField] TMP_Text subObjectiveText;
        [SerializeField] TMP_Text[] clocks;
        [Header("Wake Up")]
        [SerializeField] WakeUpGame wakeUpGame;
        [SerializeField] AudioPlayable alarmClock;
        [SerializeField] CinemachineVirtualCamera wakeUpCamera;
        [SerializeField] Canvas wakeUpCanvas;
        [Header("Get Ready")]
        [SerializeField] Container dresser;
        [SerializeField] Container bathroomDoor;
        [SerializeField] Container fridge;
        [SerializeField] Container microwave;
        [SerializeField] Container placeSetting;
        [SerializeField] MealInteractable food;
        [SerializeField] ChairInteractable mealChair; 
        [Header("Computer Work")]
        [SerializeField] ComputerInteractable computer; 
        [SerializeField] ChairInteractable deskChair; 
        [Header("Unwind")]
        [SerializeField] ChairInteractable unwindTV;
        [SerializeField] ChairInteractable couch; 
        [Header("Bedtime")]
        [SerializeField] AudioPlayable speaker;
        [SerializeField] Interactable.Interactable bed;
        [Header("Travel")] 
        [SerializeField] DoorInteractable frontDoor; 


        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }
        }

        private void Start()
        {
            // wake up
            wakeUpCanvas.enabled = false;

            // get ready 
            dresser.DisableTrigger();
            bathroomDoor.DisableTrigger();
            fridge.DisableTrigger();
            microwave.DisableTrigger();
            placeSetting.DisableTrigger();
            // food.enabled = false;
            // mealChair.enabled = false(); 

            // computer work 
            computer.enabled = false;

            // unwind 
            // unwindTV.DisableTrigger();
            // couch.enabled = false; 

            // bedtime 
            speaker.DisableTrigger();
            // bed.DisableTrigger();

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
            if (day < Day.Sun) {
                day++; 
            }
            else {
                if (cycle < Cycle.Nightmare) {
                    day = 0; 
                    cycle++; 
                }
                else {
                    Debug.Log("Game complete!"); 
                }
            }
        }

        /******* Wake Up *******/
        public void Asleep() {
            task = Task.Asleep; 
            wakeUpGame.StartMinigame(); 
            WakeUp(); 
        }

        public void WakeUp()
        {
            task = Task.WakeUp; 
            UpdateClocks("08:00");
            wakeUpCamera.Priority += 20;
            wakeUpCanvas.enabled = true;
            gameManager.CharacterMovement(false);
            alarmClock.InteractAction();
        }

        internal void AlarmOff()
        {
            alarmClock.enabled = false;
            StartCoroutine(wakeUp());
        }

        IEnumerator wakeUp()
        {
            gameManager.CharacterMovement(false);
            wakeUpCamera.gameObject.GetComponent<Animator>().Play("WakeUp");
            yield return new WaitForSeconds(4.5f);
            wakeUpCamera.Priority -= 20;
            yield return new WaitForSeconds(2f);
            gameManager.CharacterMovement(true);
            MorningClothes(); 
        }

        /****** Get Ready ******/

        public void MorningClothes()
        {
            task = Task.MorningClothes; 
            dresser.EnableTrigger();
            // dresser.Store(0); 
            objectiveText.text = "Get ready for the day";
            subObjectiveText.text = "Get dressed \nUse the bathroom";
        }

        public void MorningBathroom()
        {
            task = Task.MorningBathroom; 
            bathroomDoor.EnableTrigger();
        }

        public void BeginBreakfast()
        {
            task = Task.BeginBreakfast; 
            UpdateClocks("08:27");
            fridge.EnableTrigger();
            // fridge.Store(0);
            subObjectiveText.text = "Make breakfast \nEat breakfast";
        }

        public void MakeBreakfast()
        {
            task = Task.MakeBreakfast;
            UpdateClocks("08:39"); 
            microwave.EnableTrigger();
            // microwave.Store(0);
        }

        public void TakeBreakfast()
        {
            task = Task.TakeBreakfast; 
            placeSetting.EnableTrigger();
        }

        public void EatBreakfast()
        {
            task  = Task.EatBreakfast; 
            UpdateClocks("08:46");
            // food.enabled = true;
            CleanUpBreakfast(); 
        }

        public void CleanUpBreakfast()
        {
            task = Task.CleanBreakfast; 
            // placeSetting.EnableTrigger();
            // sink.EnableTrigger(); 
            subObjectiveText.text = "Clean up";
            WorkTime(); 
        }

        /***** Computer Work ******/

        [ContextMenu("Work")]
        public void WorkTime()
        {
            task = Task.Work; 
            UpdateClocks("09:00");
            computer.enabled = true;
            objectiveText.text = "Work";
            subObjectiveText.text = string.Empty;
        }

        public void ReturnFromWork() {
            UpdateClocks("17:00");
            gameManager.TeleportCharacter(computer.ExitTransform); 
            gameManager.GetUnSeated(); 
        }

        /****** Unwind ******/

        public void BeginDinner()
        {
            task = Task.BeginDinner; 
            fridge.EnableTrigger();
            // fridge.Store(1); 
            objectiveText.text = "Unwind";
            subObjectiveText.text = "Make Dinner \nEat dinner";
        }

        public void TakePan()
        {
            task = Task.TakePan; 
            // cabinet.EnableTrigger(); 
            MakeDinner(); 
        }

        public void MakeDinner()
        {
            task = Task.MakeDinner; 
            // stove.EnableTrigger(); 
            TakeDinner(); 
        }

        public void TakeDinner() {
            task = Task.TakeDinner; 
            // stove.EnableTrigger(); 
            EatDinner(); 
        }

        public void EatDinner() {
            task = Task.EatDinner; 
            UpdateClocks("17:37");
            // food.enabled = true; 
            CleanUpDinner(); 
        }

        public void CleanUpDinner() {
            task = Task.CleanDinner; 
            UnwindTV(); 
        }

        [ContextMenu("Unwind TV")]
        public void UnwindTV()
        {
            task = Task.UnwindTV; 
            UpdateClocks("17:43");
            // unwindTV.enabled = true;
            subObjectiveText.text = "Play games on the TV"; 
            GetNightClothes(); 
        }

        public void ReturnFromTV() {
            UpdateClocks("23:30");
            gameManager.TeleportCharacter(unwindTV.ExitTransform); 
            gameManager.GetUnSeated(); 
        }

        /****** Bedtime *******/

        public void GetNightClothes()
        {
            task = Task.NightClothes; 
            dresser.EnableTrigger();
            // dresser.Store(1); 
            objectiveText.text = "Get ready for bed";
            subObjectiveText.text = "Grab bed clothes \nTake a shower";
        }

        public void NightBathroom()
        {
            task = Task.NightBathroom; 
            bathroomDoor.EnableTrigger();
        }

        public void SleepTunes()
        {
            task = Task.SleepTunes; 
            UpdateClocks("23:04");
            speaker.EnableTrigger();
            subObjectiveText.text = "Put on some tunes \nGo to sleep";
        }

        public void Bedtime()
        {
            task = Task.Sleep; 
            UpdateClocks("23:14");
            // bed.EnableTrigger();
            EndDay(); 
        }

        public void EndDay() {
            task = Task.EndTask; 
            IncrementDay(); 
            Asleep(); 
        }

        public void ReturnFromTravel() {
            gameManager.TeleportCharacter(frontDoor.ExitTransform); 
        }

    }
}