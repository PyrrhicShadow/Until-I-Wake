using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() 
    {
        for (int i = 0; i < transform.childCount; i++) {
            int newSpot = Random.Range(0, transform.childCount); 

            // swap the ith child's pos with a random pos 
            Vector3 temp = transform.GetChild(i).position; 
            transform.GetChild(i).position = transform.GetChild(newSpot).position; 
            transform.GetChild(newSpot).position = temp; 
        }
    }
}
