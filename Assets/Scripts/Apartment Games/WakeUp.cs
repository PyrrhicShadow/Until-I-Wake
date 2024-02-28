using System.Collections;
using System.Collections.Generic;
using PyrrhicSilva;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Animator mainAnimator; 

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        }

        if (mainCanvas == null)
        {
            mainCanvas = gameObject.GetComponent<Canvas>();
        }

        if (mainAnimator == null) 
        {
            mainAnimator = gameObject.GetComponent<Animator>(); 
        }

        StartCoroutine(BypassMinigame()); // button doesn't work right now, let's move on for now
    }

    [ContextMenu("End Minigame")]
    public void EndMinigame()
    {
        Debug.Log("Minigame complete!"); 
        StartCoroutine(fadeBackground()); 
    }

    IEnumerator fadeBackground() {
        mainAnimator.Play("FadeBackground");
        yield return new WaitForSeconds(1f); 
        gameManager.WakeUpCheck(); 
    }

    IEnumerator BypassMinigame() {
        yield return new WaitForSeconds(2f); 
        EndMinigame(); 
    }
}
