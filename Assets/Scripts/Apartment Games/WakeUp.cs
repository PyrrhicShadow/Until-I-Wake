using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    public class WakeUpGame : MonoBehaviour
    {
        [SerializeField] DreamManager dreamManager;
        [SerializeField] Canvas mainCanvas;
        [SerializeField] Animator mainAnimator;
        Cycle cycle; 
        Day day; 

        // Start is called before the first frame update
        void Start()
        {
            if (dreamManager == null)
            {
                dreamManager = GameObject.FindWithTag("Dream").GetComponent<DreamManager>();
            }

            if (mainCanvas == null)
            {
                mainCanvas = gameObject.GetComponent<Canvas>();
            }

            if (mainAnimator == null)
            {
                mainAnimator = gameObject.GetComponent<Animator>();
            }
            cycle = (Cycle)PlayerPrefs.GetInt("cycle", 0); 
            day = (Day)PlayerPrefs.GetInt("day", 0); 

            mainAnimator.enabled = false; 
            dreamManager.PlayDream(); 
        }

        [ContextMenu("End Minigame")]
        public void EndMinigame()
        {
            Debug.Log("Minigame complete!");
            dreamManager.EndDream(); 
        }

        IEnumerator BypassMinigame()
        {
            yield return new WaitForSeconds(2f);
            EndMinigame();
        }

        public void StartMinigame() {
            if (day > Day.Fri) {
                StartInterviewMinigame(); 
            }
            else {
                StartNormalMinigame(); 
            }
        }

        void StartNormalMinigame()
        {
            mainAnimator.enabled = true; 
            Debug.Log("Normal day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

        void StartInterviewMinigame() {
            mainAnimator.enabled = true; 
            Debug.Log("Interview day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

    }
}