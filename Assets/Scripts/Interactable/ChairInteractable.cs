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
        [SerializeField] MealInteractable target;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            base.InteractAction();

            // initiate animation that moves camera to seated view 
            gameManager.GetSeated(chairCamera);

            if (gameManager.isSeated)
            {
                // move to target view
                StartCoroutine(LookAtTarget());
            }
        }

        protected IEnumerator LookAtTarget()
        {
            gameManager.CharacterMovement(false);
            targetCamera.Priority += 20;
            yield return new WaitForSeconds(2f);

            target.InteractAction();
            yield return new WaitForSeconds(interactDelay);
            gameManager.CharacterMovement(true);
        }
    }
}