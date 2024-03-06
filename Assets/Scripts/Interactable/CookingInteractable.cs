using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class CookingInteractable : Container
    {
        [SerializeField] GameObject foodItem;

        // Start is called before the first frame update
        void Start()
        {
            if (destroyOnDrop) isTaken = true;
            foodItem.SetActive(false);

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                if (!isTaken) isTask = false;
                else isTask = true;
                if (destroyOnDrop) { foodItem.SetActive(true); }
            }
            base.InteractAction();
        }

        public override void EnableTrigger()
        {
            if (isTaken)
            {
                interactable = true;
                if (hasGlow)
                {
                    particles.Play(false);
                }
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                interactDelay = 3f;
                StartCoroutine(cookFood());
            }
            else base.EnableTrigger();

        }

        IEnumerator cookFood()
        {
            yield return new WaitForSeconds(interactDelay);
            base.EnableTrigger();
        }
    }
}