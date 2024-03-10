using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable
{
    public class AudioPlayable : Interactable
    {
        [SerializeField] protected AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void InteractAction()
        {
            if (interactable)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                else
                {
                    audioSource.Play();
                }
                if (isTask)
                {
                    gameManager.Agenda.TaskComplete();
                }
                base.InteractAction();
            }
        }
    }
}