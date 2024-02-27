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
    }

    public void EndMinigame()
    {
        StartCoroutine(fadeBackground()); 
    }

    IEnumerator fadeBackground() {
        mainAnimator.Play("FadeBackground");
        yield return new WaitForSeconds(1f); 
        gameManager.WakeUpCheck(); 
    }
}
