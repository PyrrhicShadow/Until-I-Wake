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
        GameObject workingGame;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
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
                this.GetComponent<SplashController>().StartGame(); 
            }
            else
            {
                // instantiate next game
                workingGame = Instantiate(games[currentGame]);
            }
        }
    }
}