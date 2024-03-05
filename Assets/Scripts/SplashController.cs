using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

namespace PyrrhicSilva {
    public class SplashController : MonoBehaviour {
        [SerializeField] string sceneToLoad; 
        [SerializeField] Canvas loadingScreen; 
        [SerializeField] Image loadingBar; 

        [ContextMenu("Start Game")]
        public void StartGame() {
            StartCoroutine(LoadMainScene()); 
            loadingScreen.enabled = true; 
            loadingBar.fillAmount = 0.0f; 
        }

        IEnumerator LoadMainScene() {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad); 
            while (!asyncLoad.isDone) {
                loadingBar.fillAmount = asyncLoad.progress; 
                yield return new WaitForEndOfFrame(); 
            }
            loadingScreen.enabled = false;  
        }
    }
}