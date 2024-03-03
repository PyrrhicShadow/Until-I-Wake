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

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                // begin loading other scene 
                defaultCanvas.enabled = false;
                splashController.StartGame();
            }
            base.InteractAction();
        }
    }
}