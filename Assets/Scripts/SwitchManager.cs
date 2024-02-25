using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace SwitchesGame
{
    public class SwitchManager : MonoBehaviour
    {
        static public SwitchManager Instance; 

        [SerializeField] protected int switchCount;
        [SerializeField] GameObject winText;
        [SerializeField] Image fillBar; 
        [SerializeField] int onCount = 0;

        void Awake() {
            Instance = this; 
        }

        public void SwitchChange(int points)
        {
            onCount += points; 
            fillBar.fillAmount = (float)onCount / switchCount; 
            
            if (onCount == switchCount)
            {
                winText.SetActive(true);
            }
            else {
                winText.SetActive(false); 
            }
        }
    }
}