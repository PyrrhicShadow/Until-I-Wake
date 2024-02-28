using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PyrrhicSilva.Interactable;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace PyrrhicSilva
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Interact interact;
        [SerializeField] Canvas uiCanvas;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] GameObject hands;
        [SerializeField] bool _isHolding = false;
        public bool IsHolding { get { return _isHolding; } protected set { _isHolding = value; } }
        [Header("Agenda Setup")]
        [SerializeField] AgendaManager _agenda;
        public AgendaManager Agenda { get { return _agenda; } protected set { _agenda = value; } }
        [Header("Doors")]
        [SerializeField] AudioClip doorSound;
        [Header("Chairs")]
        CinemachineVirtualCamera currentChairCamera;
        [SerializeField] internal bool isSeated;
        [SerializeField] AudioClip chairSound;
        [Header("Save Data")]
        [SerializeField] CinemachineVirtualCamera currentActiveCamera;
        // cycle, day, task, capsule world transform

        void Awake()
        {
            if (playerInput == null)
            {
                playerInput = gameObject.GetComponent<PlayerInput>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // if restarting week day 
            Agenda.Asleep();  

            // if returning from computer 
            // Agenda: unwind 
            // current camera: computer chair
            // is seated: true

            // if returning from TV 
            // Agenda: get ready for bed 
            // current camera: couch 
            // is seated: true 

            // if returning from Travel
            // Agenda: unwind 
            // current camera: front door
            // is seated: false

            // if starting last day 
            // Nightmare(); 


        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnInteract()
        {
            interact.Press();

        }

        internal void WakeUpCheck()
        {
            // enable 3D interaction 
            CharacterMovement(true);
        }

        /// <summary>
        /// Transfers one gameObject in Container to player's "hands"
        /// </summary>
        /// <param name="thing">The Container to take from</param>
        public void Hold(Container container)
        {
            GameObject thing = container.contents.transform.GetChild(0).gameObject;
            thing.transform.SetParent(hands.transform);
            thing.transform.SetLocalPositionAndRotation(Vector3.zero, thing.transform.localRotation);
            IsHolding = true;
        }

        /// <summary>
        /// Transfers one gameObject in Container to player's "hands"
        /// </summary>
        /// <param name="container">The Container to take from</param>
        /// <param name="index">The index of the specific gameObject within the container</param>
        public void Hold(Container container, int index)
        {
            GameObject thing = container.contents.transform.GetChild(index).gameObject;
            thing.transform.SetParent(hands.transform);
            thing.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            IsHolding = true;
        }

        /// <summary>
        /// Transfers oldest gameObject from player's "hands" to a relative position of an <class>Interactable</class>
        /// </summary>
        /// <param name="newSpot">New parent object</param>
        /// <param name="relativePos">Position relative to new parent</param>
        public void Drop(Container newSpot)
        {
            GameObject dropItem = hands.transform.GetChild(0).gameObject;
            dropItem.transform.SetParent(newSpot.contents.transform);
            dropItem.transform.SetLocalPositionAndRotation(Vector3.zero, dropItem.transform.localRotation);

            if (hands.transform.childCount == 0)
            {
                IsHolding = false;
            }
        }

        /// <summary>
        /// Destroys all gameObjects in the player's "hand"
        /// </summary>
        [ContextMenu("Empty Hands")]
        public void EmptyHands()
        {
            for (int i = 0; i < hands.transform.childCount; i++)
            {
                Destroy(hands.transform.GetChild(i).gameObject);
            }
            IsHolding = false;
        }

        internal void GetSeated(CinemachineVirtualCamera chairCamera)
        {
            currentChairCamera = chairCamera;
            if (currentChairCamera != null)
            {
                StartCoroutine(getSeated());
            }
            else
            {
                Debug.Log("You are not sitting in a chair.");
            }
        }

        IEnumerator getSeated()
        {
            currentChairCamera.Priority += 10;
            // currentChairCamera.GetComponent<Animator>().Play("SitInChair");
            yield return new WaitForSeconds(2f);
            isSeated = true;
        }

        internal void GetUnSeated()
        {
            if (currentChairCamera != null)
            {
                StartCoroutine(getUnSeated());
            }
            else
            {
                Debug.Log("You are not sitting in a chair.");
            }
        }

        IEnumerator getUnSeated()
        {
            currentChairCamera.Priority -= 10;
            // currentChairCamera.GetComponent<Animator>().Play("StandFromChair");
            yield return new WaitForSeconds(2f);
            isSeated = false;
        }

        internal void CharacterMovement(bool state)
        {
            if (state)
            {
                Cursor.lockState = CursorLockMode.Locked;
                playerInput.SwitchCurrentActionMap("Player");
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                playerInput.SwitchCurrentActionMap("UI");
            }

            uiCanvas.enabled = state;

            Debug.Log(playerInput.currentActionMap.name);

        }
    }
}