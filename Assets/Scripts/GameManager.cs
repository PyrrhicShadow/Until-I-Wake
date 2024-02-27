using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva.Interactable;
using UnityEditor.Search;
using UnityEngine;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Interact interact;
        [SerializeField] GameObject hands;
        [SerializeField] AudioPlayable speaker; 
        [SerializeField] AudioPlayable alarmClock; 
        [SerializeField] Animator playerAnimator; 

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            WakeUp(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnInteract()
        {
            interact.Press();
        }

        public void Hold(GameObject thing) {
            thing.transform.SetParent(hands.transform); 
            thing.transform.SetLocalPositionAndRotation(Vector3.zero, thing.transform.localRotation); 
        }

        public void Drop(Interactable.Interactable newSpot, Vector3 relativePos) {
            GameObject dropItem = hands.transform.GetChild(0).gameObject; 
            dropItem.transform.SetParent(newSpot.transform); 
            dropItem.transform.SetLocalPositionAndRotation(relativePos, dropItem.transform.localRotation); 
        } 

        void WakeUp() {
            playerAnimator.enabled = true; 
            StartCoroutine(wakeUp()); 
        }

        IEnumerator wakeUp() {
            playerAnimator.Play("WakeUp"); 
            yield return new WaitForSeconds(5.5f); 
            playerAnimator.enabled = false; 
        }
    }
}