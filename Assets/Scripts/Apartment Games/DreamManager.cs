using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    class Dream : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] float _animationTime;
        public float animationTime { get { return _animationTime; } private set { _animationTime = value; } }

        public void Play()
        {
            animator.Play("dream");
        }
    }
    public class DreamManager : MonoBehaviour
    {
        public static DreamManager dreamManager;
        [SerializeField] SplashController startSplash;
        [SerializeField] SplashController endSplash;
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject[] dreams;
        [SerializeField] GameObject[] nightmares;
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
            int randDream = Random.Range(0, dreams.Length);
            currentDream = Instantiate(dreams[randDream]).GetComponent<Dream>();
            StartCoroutine(playDream());
        }

        public void PlayNightmare()
        {

            int randDream = Random.Range(0, nightmares.Length);
            currentDream = Instantiate(nightmares[randDream]).GetComponent<Dream>();
            StartCoroutine(playDream());
        }

        IEnumerator playDream()
        {
            currentDream.Play();
            yield return new WaitForSeconds(currentDream.animationTime);
        }
    }
}