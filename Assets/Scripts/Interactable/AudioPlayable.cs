using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class AudioPlayable : Interactable
    {
        [SerializeField] AudioSource audioSource; 

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            base.InteractAction();


            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                gameManager.Agenda.AlarmOff(); 
            }
            else
            {
                audioSource.Play();
            }
        }   
    }
}