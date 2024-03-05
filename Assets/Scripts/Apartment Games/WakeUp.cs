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
            mainAnimator.enabled = false; 
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

        public void StartNormalMinigame()
        {
            mainAnimator.enabled = true; 
            Debug.Log("Normal day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

        public void StartInterviewMinigame() {
            mainAnimator.enabled = true; 
            Debug.Log("Interview day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

    }
}