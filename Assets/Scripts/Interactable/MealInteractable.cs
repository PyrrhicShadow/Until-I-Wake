using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class MealInteractable : Interactable
    {
        [SerializeField] ParticleSystem eatingParticles;
        [SerializeField] AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                StartCoroutine(EatingParticles());
                if (isTask) {
                    gameManager.Agenda.TaskComplete(); 
                }
            }
            base.InteractAction();
        }

        IEnumerator EatingParticles()
        {
            eatingParticles.Play(false);
            yield return new WaitForSeconds(interactDelay);
            eatingParticles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            gameManager.GetUnSeated(); 
            yield return new WaitForEndOfFrame(); 
            yield return new WaitForEndOfFrame(); 
            Destroy(this.gameObject); 
        }
    }
}