using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class ComputerInteractable : Interactable
    {
        [SerializeField] Canvas defaultCanvas;
        [SerializeField] SplashController splashController;
        [SerializeField] CinemachineVirtualCamera chairCamera;
        [SerializeField] CinemachineVirtualCamera laptopCamera;

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
                // move to laptop view
                StartCoroutine(LookAtLaptop());
            }

        }

        IEnumerator LookAtLaptop()
        {
            gameManager.CharacterMovement(false); 
            laptopCamera.Priority += 20;
            yield return new WaitForSeconds(2f);

            // begin loading other scene 
            defaultCanvas.enabled = false;
            splashController.StartGame();
        }

    }
}