using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class CookingInteractable : Container
    {
        // Start is called before the first frame update
        void Start()
        {
            if (destroyOnDrop)
            {
                isTaken = true;
            }
        }

        public override void EnableTrigger()
        {
            if (!isTaken)
            {
                audioSource.Play();
                interactDelay = 3f;
                StartCoroutine(cookFood()); 
            }
            else {
                base.EnableTrigger(); 
            }
        }

        IEnumerator cookFood() {
            yield return new WaitForSeconds(interactDelay); 
            base.EnableTrigger(); 
        }
    }
}