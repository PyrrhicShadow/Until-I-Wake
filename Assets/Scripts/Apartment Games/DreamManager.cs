using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    public class DreamManager : MonoBehaviour
    {
        public static DreamManager dreamManager;
        [SerializeField] SplashController startSplash;
        [SerializeField] SplashController endSplash;
        [SerializeField] WakeUpGame wakeUpGame; 
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject[] dreams;
        [SerializeField] GameObject[] nightmares;
        [SerializeField] internal bool isNightmare = false; 
        [SerializeField] Dream currentDream;

        void Awake()
        {
            if (dreamManager == null)
            {
                DontDestroyOnLoad(this.gameObject);
                dreamManager = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        [ContextMenu("Start Dream")]
        public void StartDream()
        {
            startSplash.StartGame();
        }

        [ContextMenu("End Dream")]
        public void EndDream()
        {
            endSplash.StartGame();
        }

        public void PlayDream()
        {
            if (wakeUpGame == null) {
                wakeUpGame = GameObject.FindWithTag("WakeUp").GetComponent<WakeUpGame>(); 
            }

            if (isNightmare) {
                int randDream = Random.Range(0, nightmares.Length);
            currentDream = Instantiate(nightmares[randDream]).GetComponent<Dream>();
            }
            else {
            int randDream = Random.Range(0, dreams.Length);
            currentDream = Instantiate(dreams[randDream]).GetComponent<Dream>();
            }

            StartCoroutine(playDream());
        }

        IEnumerator playDream()
        {
            currentDream.Play();
            yield return new WaitForSeconds(currentDream.animationTime);
            yield return new WaitForEndOfFrame(); 
            wakeUpGame.StartMinigame(); 
        }
    }
}