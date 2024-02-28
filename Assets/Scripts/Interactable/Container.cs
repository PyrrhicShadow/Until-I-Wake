using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class Container : Interactable
    {
        [SerializeField] public GameObject contents;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip pickUp;
        [SerializeField] AudioClip putDown;
        [SerializeField] bool isTaken = false;
        [SerializeField] bool destroyOnDrop = false;

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
            base.InteractAction();


            if (isTaken)
            {
                if (gameManager.IsHolding)
                {
                    if (destroyOnDrop)
                    {
                        gameManager.EmptyHands();
                        switch (gameManager.Agenda.task)
                        {
                            case Task.MorningBathroom:
                                // gameManager.Agenda.BeginBreakfast();
                                break;
                            case Task.NightBathroom:
                                // gameManager.Agenda.SleepTunes();
                                break;
                            case Task.MakeBreakfast:
                                gameManager.Agenda.TakeBreakfast();
                                break;
                            case Task.MakeDinner:
                                // gameManager.Agenda.TakeDinner(); 
                                break;
                            default:
                                break;
                        }
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

                    switch (gameManager.Agenda.task)
                    {
                        case Task.TakeBreakfast:
                            gameManager.Agenda.EatBreakfast();
                            break;
                        case Task.TakeDinner:
                            gameManager.Agenda.EatDinner();
                            break;
                        case Task.TakeBreakfastHQ:
                            break;
                        case Task.TakeDinnerHQ:
                            break;
                        case Task.TakeLunch:
                            break;
                        default:
                            break;
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

                    switch (gameManager.Agenda.task)
                    {
                        case Task.MorningClothes:
                            gameManager.Agenda.MorningBathroom();
                            break;
                        case Task.NightClothes:
                            gameManager.Agenda.NightBathroom();
                            break;
                        default:
                            break;
                    }
                }
            }

        }
    }
}