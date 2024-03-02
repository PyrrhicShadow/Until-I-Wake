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
            if (!isTaken) isTask = false;
            else isTask = true;
            base.InteractAction();
        }

        public override void EnableTrigger()
        {
            if (isTaken)
            {
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
            foodItem.SetActive(true); 
            yield return new WaitForSeconds(interactDelay);
            base.EnableTrigger();
        }
    }
}