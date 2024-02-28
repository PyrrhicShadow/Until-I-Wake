using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PyrrhicSilva.Interactable
{
    public class DoorInteractable : Interactable
    {
        [SerializeField] Canvas doorCanvas;
        [SerializeField] Animator canvasAnimator;
        [SerializeField] CinemachineVirtualCamera enterCamera;
        [SerializeField] CinemachineVirtualCamera exitCamera;
        [SerializeField] protected Transform _exitTransform;
        public Transform ExitTransform { get { return _exitTransform; } protected set { _exitTransform = value; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                StartCoroutine(doorEnter());
            }
            base.InteractAction();
        }

        IEnumerator doorEnter()
        {
            gameManager.CharacterMovement(false);
            enterCamera.Priority += 20;
            yield return new WaitForSeconds(2f);
            // canvasAnimator.Play("FadeIn"); 
            doorCanvas.enabled = true;
            enterCamera.Priority -= 20;
            gameManager.TeleportCharacter(ExitTransform);
        }

        [ContextMenu("Exit Screen")]
        public void ExitButton()
        {
            StartCoroutine(doorExit());
        }

        IEnumerator doorExit()
        {
            exitCamera.Priority += 20;
            // canvasAnimator.Play("FadeOut"); 
            doorCanvas.enabled = false;
            yield return new WaitForSeconds(3f);
            exitCamera.Priority -= 20;
            yield return new WaitForSeconds(2f);
            gameManager.CharacterMovement(true);

            switch (gameManager.Agenda.task)
            {
                case Task.MorningBathroom:
                    gameManager.Agenda.BeginBreakfast();
                    break;
                case Task.NightBathroom:
                    gameManager.Agenda.SleepTunes();
                    break;
                case Task.ArriveDorm:
                    break;
                case Task.ArriveHQ:
                    break;
                default:
                    break;
            }

        }
    }
}