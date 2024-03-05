using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    class Dream : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AudioSource dreamMusic; 
        [SerializeField] float _animationTime;
        public float animationTime { get { return _animationTime; } private set { _animationTime = value; } }

        public void Play()
        {
            animator.Play("dream");
            dreamMusic.Play(); 
        }
    }
}
