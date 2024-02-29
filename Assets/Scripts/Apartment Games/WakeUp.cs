using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    public class WakeUpGame : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] Canvas mainCanvas;
        [SerializeField] Animator mainAnimator;

        // Start is called before the first frame update
        void Start()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            }

            if (mainCanvas == null)
            {
                mainCanvas = gameObject.GetComponent<Canvas>();
            }

            if (mainAnimator == null)
            {
                mainAnimator = gameObject.GetComponent<Animator>();
            }
        }

        [ContextMenu("End Minigame")]
        public void EndMinigame()
        {
            Debug.Log("Minigame complete!");
            StartCoroutine(fadeBackground());
        }

        IEnumerator fadeBackground()
        {
            mainAnimator.Play("FadeBackground");
            yield return new WaitForSeconds(1f);
            gameManager.WakeUpCheck();
        }

        IEnumerator BypassMinigame()
        {
            yield return new WaitForSeconds(2f);
            EndMinigame();
        }

        public void StartNormalMinigame()
        {
            Debug.Log("Normal day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

        public void StartInterviewMinigame() {
            Debug.Log("Interview day wake up mini game start"); 
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

        private void GuidedWeek()
        {
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }
        private void GuidedInterview()
        {
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }
        private void StandardWeek()
        {
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }
        private void StandardInterview()
        {
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }
        private void FinalNightmare()
        {
            StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
        }

    }
}