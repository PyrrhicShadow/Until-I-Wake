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
        // public static GameManager gameManager;
        [SerializeField] Interact interact;
        [SerializeField] Canvas uiCanvas;
        [SerializeField] PlayerInput playerInput;
        [Header("Agenda Setup")]
        [SerializeField] AgendaManager _agenda;
        public AgendaManager Agenda { get { return _agenda; } protected set { _agenda = value; } }
        [Header("Chairs")]
        CinemachineVirtualCamera currentChairCamera;
        [SerializeField] internal bool isSeated;
        [Header("Hands")]
        [SerializeField] GameObject hands;
        [SerializeField] bool _isHolding = false;
        public bool IsHolding { get { return _isHolding; } protected set { _isHolding = value; } }

        void Awake()
        {
            // if (gameManager == null)
            // {
            //     DontDestroyOnLoad(this.gameObject);
            //     gameManager = this;
            // }
            // else
            // {
            //     Destroy(this.gameObject);
            // }

            if (playerInput == null)
            {
                playerInput = gameObject.GetComponent<PlayerInput>();
            }

            if (uiCanvas == null) {
                this.enabled = false; 
            }
            LoadGame();
        }

        // Start is called before the first frame update
        void Start()
        {
            Agenda.TaskComplete(); 

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnExit()
        {
            SaveGame();
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

        [ContextMenu("Save Game")]
        public void SaveGame()
        {
            // cycle, day, task
            PlayerPrefs.SetInt("cycle", (int)Agenda.cycle);
            PlayerPrefs.SetInt("day", (int)Agenda.day);
            PlayerPrefs.SetInt("task", (int)Agenda.objective.task);
        }

        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            Agenda.cycle = (Cycle)PlayerPrefs.GetInt("cycle", 0);
            Agenda.day = (Day)PlayerPrefs.GetInt("day", 0);
            Agenda.objective.NewObjective((Task)PlayerPrefs.GetInt("task", 0));
        }

        [ContextMenu("Clear Save Data")]
        public void ClearSave()
        {
            PlayerPrefs.SetInt("cycle", 0);
            PlayerPrefs.SetInt("day", 0);
            PlayerPrefs.SetInt("task", 0);
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

            // Debug.Log(playerInput.currentActionMap.name);

        }

        public void TeleportCharacter(Transform newTransform)
        {
            this.transform.SetPositionAndRotation(newTransform.position, newTransform.rotation);
        }
    }
}