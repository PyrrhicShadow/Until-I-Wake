using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class MealInteractable : Interactable
    {
        [SerializeField] ParticleSystem particles;
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
            }
            base.InteractAction();
        }

        IEnumerator EatingParticles()
        {
            particles.Play();
            yield return new WaitForSeconds(interactDelay);
            particles.Stop();
        }
    }
}