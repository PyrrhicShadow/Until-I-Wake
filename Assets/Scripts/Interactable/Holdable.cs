using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class Holdable : Interactable
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            base.InteractAction();

            gameManager.Hold(this.gameObject); 
            
        }

    }
}