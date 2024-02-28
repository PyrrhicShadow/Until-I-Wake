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
        WakeUp, Work, Dinner, TV, Betdime, Leave, Arrive, Interview
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
        [SerializeField] WakeUp wakeUpGame;
        [SerializeField] AudioPlayable alarmClock;
        [SerializeField] CinemachineVirtualCamera wakeUpCamera;
        [SerializeField] Canvas wakeUpCanvas;
        [Header("Get Ready")]
        [SerializeField] Container dresser;
        [SerializeField] Container bathroomDoor;
        [SerializeField] Canvas bathroomCanvas;
        [SerializeField] Container fridge;
        [SerializeField] Container microwave;
        [SerializeField] Container placeSetting;
        [SerializeField] Interactable.Interactable food;
        [Header("Computer Work")]
        [SerializeField] ComputerInteractable computer;
        [Header("Unwind")]
        [SerializeField] Interactable.Interactable unwindTV;
        [Header("Bedtime")]
        [SerializeField] AudioPlayable speaker;
        [SerializeField] Interactable.Interactable bed;


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
            alarmClock.DisableTrigger();
            wakeUpCanvas.enabled = false;

            // get ready 
            dresser.DisableTrigger();
            bathroomDoor.DisableTrigger();
            fridge.DisableTrigger();
            microwave.DisableTrigger();
            placeSetting.DisableTrigger();
            food.DisableTrigger();

            // computer work 
            computer.DisableTrigger();

            // unwind 
            unwindTV.DisableTrigger();

            // bedtime 
            speaker.DisableTrigger();
            bed.DisableTrigger();
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

        public void Asleep()
        {
            task = Task.WakeUp; 
            UpdateClocks("08:00");
            wakeUpCamera.Priority += 20;
            wakeUpCanvas.enabled = true;
            gameManager.CharacterMovement(false);
            alarmClock.InteractAction();
        }

        internal void WakeUp()
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
            alarmClock.DisableTrigger();
        }

        /****** Get Ready ******/

        public void GetDayClothes()
        {
            dresser.EnableTrigger();
            // dresser.Store(0); 
            objectiveText.text = "Get ready for the day";
            subObjectiveText.text = "Get dressed \nUse the bathroom";
        }

        public void Bathroom()
        {
            bathroomDoor.EnableTrigger();
            bathroomCanvas.enabled = true;
        }

        public void BeginBreakfast()
        {
            fridge.EnableTrigger();
            // fridge.Store(0);
            subObjectiveText.text = "Make breakfast \nEat breakfast";
        }

        public void MicrowaveBreakfast()
        {
            microwave.EnableTrigger();
            // microwave.Store(0);
        }

        public void TakeMicrowaveFood()
        {
            placeSetting.EnableTrigger();
        }

        public void EatFood()
        {
            // food.EnableTrigger();
            CleanUpFood(); 
        }

        public void CleanUpFood()
        {
            // placeSetting.EnableTrigger();
            // sink.EnableTrigger(); 
            subObjectiveText.text = "Clean up";
            WorkTime(); 
        }

        /***** Computer Work ******/

        public void WorkTime()
        {
            task = Task.Work; 
            computer.EnableTrigger();
            objectiveText.text = "Work";
            subObjectiveText.text = string.Empty;
        }

        /****** Unwind ******/

        public void BeginDinner()
        {
            task = Task.Dinner; 
            fridge.EnableTrigger();
            // fridge.Store(1); 
            objectiveText.text = "Unwind";
            subObjectiveText.text = "Make Dinner \nEat dinner";
        }

        public void GetPan()
        {
            // cabinet.EnableTrigger(); 
            CookDinner(); 
        }

        public void CookDinner()
        {
            // stove.EnableTrigger(); 
            UnwindTV(); 
        }

        // eat food in pan on table and clean up same as before 

        public void UnwindTV()
        {
            task = Task.TV; 
            // unwindTV.EnableTrigger();
            subObjectiveText.text = "Play games on the TV"; 
            GetNightClothes(); 
        }

        /****** Bedtime *******/

        public void GetNightClothes()
        {
            task = Task.Betdime; 
            dresser.EnableTrigger();
            // dresser.Store(1); 
            objectiveText.text = "Get ready for bed";
            subObjectiveText.text = "Grab bed clothes \nTake a shower";
        }

        public void SleepTunes()
        {
            speaker.EnableTrigger();
            subObjectiveText.text = "Put on some tunes \nGo to sleep";
        }

        public void Bedtime()
        {
            bed.EnableTrigger();
        }

    }
}