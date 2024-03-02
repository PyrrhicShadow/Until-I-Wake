using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva;
using UnityEngine;

public class DreamManager : MonoBehaviour
{
    public static DreamManager dreamManager; 
    [SerializeField] SplashController startSplash; 
    [SerializeField] SplashController endSplash; 
    [SerializeField] AudioSource audioSource; 

    void Awake() {
        if (dreamManager == null) {
            DontDestroyOnLoad(this.gameObject); 
            dreamManager = this; 
        }
        else {
            Destroy(this.gameObject); 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDream() {
        startSplash.StartGame(); 
    }

    public void EndDream() {
        endSplash.StartGame(); 
    }
}
