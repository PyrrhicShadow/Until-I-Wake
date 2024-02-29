using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class Container : Interactable
    {
        [SerializeField] public GameObject contents;
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip pickUp;
        [SerializeField] protected AudioClip putDown;
        [SerializeField] protected bool isTaken = false;
        [SerializeField] protected bool destroyOnDrop = false;

        // Start is called before the first frame update
        void Start()
        {
            if (destroyOnDrop)
            {
                isTaken = true;
            }
        }

        public override void InteractAction()
        {
            if (interactable)
            {
                if (isTaken)
                {
                    if (gameManager.IsHolding)
                    {
                        if (destroyOnDrop)
                        {
                            gameManager.EmptyHands();

                        }
                        else
                        {
                            gameManager.Drop(this);
                            if (audioSource != null)
                            {
                                audioSource.PlayOneShot(putDown);
                            }
                        }
                        isTaken = false;

                        if (isTask)
                        {
                            gameManager.Agenda.TaskComplete();


                        }
                    }
                }
                else
                {
                    if (contents != null)
                    {
                        gameManager.Hold(this);
                        if (audioSource != null)
                        {
                            audioSource.PlayOneShot(pickUp);
                        }
                        isTaken = true;

                        if (isTask) {
                            gameManager.Agenda.TaskComplete(); 
                        }
                    }
                }
            }
            base.InteractAction();
        }
    }
}