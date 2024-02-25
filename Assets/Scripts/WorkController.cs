using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PyrrhicSilva
{
    public class WorkController : MonoBehaviour
    {
        static public WorkController Instance;
        [SerializeField] GameObject[] games;
        [SerializeField] int currentGame = 0;
        [SerializeField] GameObject loading;
        GameObject workingGame;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            loading.SetActive(false);

            // instantiate first game
            workingGame = Instantiate(games[0]);
        }

        public void NextGame()
        {
            currentGame++;

            // show loading canvas 
            // destroy current game
            Destroy(workingGame);

            if (currentGame >= games.Length)
            {
                // end the computer session
                loading.SetActive(true);
                this.GetComponent<SplashController>().StartGame(); 
            }
            else
            {
                // instantiate next game
                workingGame = Instantiate(games[currentGame]);
            }
        }

        public void Submit() {
            loading.SetActive(false); 
        }
    }
}