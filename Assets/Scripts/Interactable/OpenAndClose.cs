using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva.Interactable {
public class OpenAndClose : Interactable {
    public Animator openandclose;
    [SerializeField] string desc; 
    [SerializeField] internal bool open;
    [SerializeField] AudioSource effectSource;  
    [SerializeField] AudioClip[] effects; 
    [SerializeField] internal float animTime = 0.5f;

    protected override void Awake() {
        base.Awake(); 
        if (openandclose == null) {
            openandclose = this.gameObject.GetComponent<Animator>(); 
        }
        if (desc.Trim() == string.Empty) {
            desc = "Open and close"; 
        }
    }

    protected virtual void Start() {
        if (openandclose == null) {
            Debug.Log(this.name + " is missing its animator."); 
        }
        open = false;
    }

    /// <summary>Opens or closes this OpenAndClose object </summary>
    public override void InteractAction() {
        if (interactable) {
            if (open == false) {
                StartCoroutine(opening());
                if (effects != null && effects[0] != null) { 
                    effectSource.PlayOneShot(effects[0]); 
                    // gameManager.subtitles.DisplayAudioDescription(desc + " opening."); 
                }
            }
            else {
                if (open == true) {
                    StartCoroutine(closing());
                    if (effects != null && effects[1] != null) {
                        effectSource.PlayOneShot(effects[1]); 
                        // gameManager.subtitles.DisplaySubtitles(desc + " closing."); 
                    }
                }
            }
        }
    }

    protected virtual IEnumerator opening()
		{
			//Debug.Log("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(animTime);
		}

    protected virtual IEnumerator closing()
    {
        //Debug.Log("you are closing the door");
        openandclose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(animTime);
    }
}
}