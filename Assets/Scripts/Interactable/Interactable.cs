using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable {
    public abstract class Interactable : MonoBehaviour {
        [SerializeField] protected GameManager gameManager; 
        [SerializeField] protected Collider _col; 
        [SerializeField] protected bool isTask = false; 
        [SerializeField] protected bool repeatable = false; 
        [SerializeField] protected float interactDelay = 0.5f; 
        [SerializeField] protected bool interactable = true; 
        private float timer = 0; 
        public Collider col { get { return _col; } protected set { _col = value; } }

        /// <summary>>Awake is called before Start. Make sure to call <c>base.Awake()</c> when overriding.</summary>
        protected virtual void Awake() {
            if (gameManager == null) {
                gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>(); 
            }
            if (col == null) {
                col = gameObject.GetComponent<Collider>();
            }
        }

        protected virtual void Update() {
            if (repeatable && !interactable) {
                if (timer < interactDelay) {
                    timer += Time.deltaTime; 
                }
                else {
                    EnableTrigger(); 
                    Debug.Log(this.name + "'s trigger was reenabled."); 
                    timer = 0; 
                }
            }
        }

        // <summary>By default, disables the trigger collider</summary>
        [ContextMenu("Interact")]
        public virtual void InteractAction() {
            if (interactable) {
                Debug.Log(name + " has been triggered."); 
                DisableTrigger(); 
            }
        }

        /// <summary>After calling InteractAction, disable the trigger to prevent trigger spamming.</summary>
        public virtual void DisableTrigger() {
            interactable = false; 
            Debug.Log(name + " has been disabled.");
        }

        public virtual void EnableTrigger() {
            interactable = true; 
            Debug.Log(name + " has been enabled.");
        }
    }
}