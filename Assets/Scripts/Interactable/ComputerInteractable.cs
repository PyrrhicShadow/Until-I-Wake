using System.Collections;
using System.Collections.Generic;
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
            base.InteractAction(); 

            // initiate animation that moves camera to seated view 

            // begin loading other scene 
            defaultCanvas.enabled = false; 
            splashController.StartGame(); 
        }


    }
}