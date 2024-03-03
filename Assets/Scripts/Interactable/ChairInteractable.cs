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
        [SerializeField] protected Interactable target;
        public Transform ExitTransform { get { return _exitTransform; } protected set { _exitTransform = value; } }

        // Start is called before the first frame update
        void Start()
        {
            if (target != null)
            {
                target.enabled = false;
                target.DisableTrigger();
            }
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
            gameManager.GetUnSeated();
            gameManager.GetSeated(targetCamera);
            target.enabled = true;
            target.EnableTrigger();
            yield return new WaitForSeconds(2f);
            target.InteractAction();
            yield return new WaitForSeconds(2f);
            target.enabled = false;
            gameManager.CharacterMovement(true);
        }

        public void SetTarget(Interactable target)
        {
            this.target = target;
            target.enabled = false;
            target.DisableTrigger();
        }
    }
}