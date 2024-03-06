using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    class Dream : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AudioSource dreamMusic;
        [SerializeField] float _animationTime = 5f;
        public float animationTime { get { return _animationTime; } private set { _animationTime = value; } }
        [SerializeField] Material _skybox;
        public Material skybox { get { return _skybox; } private set { _skybox = value; } }

        public void Play()
        {
            // mainCamera.skybox = skybox; 
            // animator.Play("dream");
            // dreamMusic.Play(); 
        }
    }
}
