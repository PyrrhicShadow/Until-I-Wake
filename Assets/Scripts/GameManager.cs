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
        [Header("Agenda")]
        [SerializeField] Canvas agendaCanvas; 
        [SerializeField] TMP_Text objectiveText; 
        [SerializeField] TMP_Text subObjectiveText; 
        [Header("Wake Up")]
        [SerializeField] AudioPlayable alarmClock;
        [SerializeField] CinemachineVirtualCamera wakeUpCamera;
        [SerializeField] Canvas wakeUpCanvas;
        [Header("Doors")]
        [SerializeField] CinemachineVirtualCamera doorCamera;
        [SerializeField] AudioClip doorSound;
        [Header("Chairs")]
        CinemachineVirtualCamera currentChairCamera;
        [SerializeField] internal bool isSeated; 
        [SerializeField] AudioClip chairSound;
        [Header("Bedtime")]
        [SerializeField] AudioPlayable speaker;
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
            Asleep();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnInteract()
        {
            interact.Press();

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

            if (hands.transform.childCount == 0) {
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

        internal void Asleep()
        {
            wakeUpCamera.Priority += 20;
            wakeUpCanvas.enabled = true;
            CharacterMovement(false);
            alarmClock.InteractAction();
        }

        internal void WakeUp()
        {
            alarmClock.enabled = false;
            StartCoroutine(wakeUp());
        }

        IEnumerator wakeUp()
        {
            CharacterMovement(false); 
            wakeUpCamera.gameObject.GetComponent<Animator>().Play("WakeUp");
            yield return new WaitForSeconds(4.5f);
            wakeUpCamera.Priority -= 20;
            yield return new WaitForSeconds(2f); 
            CharacterMovement(true); 
        }

        internal void WakeUpCheck()
        {
            // enable 3D interaction 
            CharacterMovement(true);
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