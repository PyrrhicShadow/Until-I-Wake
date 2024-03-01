using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class ChairInteractable : Interactable
    {
        [SerializeField] protected CinemachineVirtualCamera chairCamera;
        [SerializeField] protected CinemachineVirtualCamera targetCamera;
        [SerializeField] protected Transform _exitTransform;
        public Transform ExitTransform { get { return _exitTransform; } protected set { _exitTransform = value; } }
        [SerializeField] Interactable target;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                // initiate animation that moves camera to seated view 
                gameManager.GetSeated(chairCamera);

                StartCoroutine(WaitUntilSeated());
            }
            base.InteractAction();
        }

        protected virtual IEnumerator WaitUntilSeated()
        {
            yield return new WaitForSeconds(2f);

            // move to target view
            StartCoroutine(LookAtTarget());

        }

        protected virtual IEnumerator LookAtTarget()
        {
            gameManager.CharacterMovement(false);
            targetCamera.Priority += 20;
            yield return new WaitForSeconds(2f);

            target.enabled = true;
            target.EnableTrigger();
            target.InteractAction();
            yield return new WaitForSeconds(interactDelay);
            gameManager.CharacterMovement(true);
        }
    }
}