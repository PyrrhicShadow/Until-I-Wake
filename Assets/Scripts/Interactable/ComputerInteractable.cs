using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class ComputerInteractable : ChairInteractable
    {
        [SerializeField] Canvas defaultCanvas;
        [SerializeField] SplashController splashController;

        // Start is called before the first frame update
        void Start()
        {

        }

        protected new IEnumerator LookAtTarget()
        {
            gameManager.CharacterMovement(false); 
            targetCamera.Priority += 20;
            yield return new WaitForSeconds(2f);

            // begin loading other scene 
            defaultCanvas.enabled = false;
            splashController.StartGame();
        }

    }
}