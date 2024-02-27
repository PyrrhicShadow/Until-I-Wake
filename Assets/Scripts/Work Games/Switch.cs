using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    public class Switch : MonoBehaviour
    {
        [SerializeField] GameObject upSwitch;
        [SerializeField] GameObject onLight;
        [SerializeField] protected bool isOn = false;
        [SerializeField] protected bool isUp = false;

        // Start is called before the first frame update
        void Start()
        {
            RandomizeSwitch(); 

            SetUp();

            if (isOn)
            {
                WorkGameManager.Instance.PointsChange(1);
            }
        }

        // Update is called once per frame
        void OnMouseUp()
        {
            // flips the toggles 
            isUp = !isUp;
            isOn = !isOn;
            if (isOn)
            {
                WorkGameManager.Instance.PointsChange(1);
            }
            else
            {
                WorkGameManager.Instance.PointsChange(-1);
            }
            SetUp();
        }

        void SetUp()
        {
            upSwitch.SetActive(isUp);
            onLight.SetActive(isOn);
        }

        protected void RandomizeSwitch() {
            if (Random.Range(0, 10) > 8)
            {
                isOn = true;
            }

            if (Random.Range(0, 10) > 4)
            {
                isUp = true;
            }
        }
    }
}