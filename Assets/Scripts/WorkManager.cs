using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkGame
{
    public enum WorkType
    {
        switches, wires
    }
    public class WorkManager : MonoBehaviour
    {
        static public WorkManager Instance;
        [SerializeField] WorkType game;
        [SerializeField] protected int pointsCount;
        [SerializeField] GameObject winText;
        [SerializeField] Image fillBar;
        [SerializeField] int onCount = 0;

        void Awake()
        {
            Instance = this;
        }

        public void PointsChange(int points)
        {
            onCount += points;
            if (game == WorkType.switches) {
                fillBar.fillAmount = (float)onCount / pointsCount;
            }

            if (onCount == pointsCount)
            {
                winText.SetActive(true);
            }
            else
            {
                winText.SetActive(false);
            }
        }
    }
}