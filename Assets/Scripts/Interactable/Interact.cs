using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva {
    public class Interact : MonoBehaviour {
        [SerializeField] GameManager gameManager; 

        private void Awake() {
            if (gameManager == null) {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>(); 
            }
        }

        public void Press() {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2);
            GameObject target = hit.transform?.gameObject; 

            // Interact action
            Interactable.Interactable interactable = target.GetComponent<Interactable.Interactable>();
            interactable?.InteractAction();
            interactable = null; 
        }
    }
}