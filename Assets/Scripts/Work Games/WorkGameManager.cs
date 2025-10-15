using UnityEngine;
using UnityEngine.UI;

namespace PyrrhicSilva
{
    public enum WorkType
    {
        switches, wires, questions
    }
    public class WorkGameManager : MonoBehaviour
    {
        static public WorkGameManager Instance;
        [SerializeField] WorkType game;
        [SerializeField] int currPoints = 0;
        [SerializeField] protected int reqPoints;
        [SerializeField] GameObject winText;
        [SerializeField] Image fillBar;

        void Awake()
        {
            Instance = this;
        }

        void Start() {
            winText.SetActive(false); 
            // unlock cursor 
            Cursor.lockState = CursorLockMode.None;
        }

        public void PointsChange(int points)
        {
            currPoints += points;
            if (game == WorkType.switches) {
                fillBar.fillAmount = (float)currPoints / reqPoints;
            }

            if (currPoints == reqPoints)
            {
                winText.SetActive(true);
            }
            else
            {
                winText.SetActive(false);
            }
        }

        public void SubmitButton() {
            WorkController.Instance.NextGame(); 
        }
    }
}