using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 defaultPosition; 
    [SerializeField] SpriteRenderer wireEnd;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.parent.position;
        defaultPosition = transform.position; 
    }

    void OnMouseDrag()
    {
        // convert mouse position to world point 
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;

        UpdateWire(newPosition); 

    }

    void OnMouseUp()
    {
        // reset wire to default position 
        UpdateWire(defaultPosition); 
    }

    void UpdateWire(Vector3 newPosition) {
        // move wire to new position 
        transform.position = newPosition;

        // connect wire from end to new position 
        Vector3 direction = newPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;

        // stretch wire between end and new position 
        float dist = Vector2.Distance(startPoint, newPosition);
        wireEnd.size = new Vector2(dist, wireEnd.size.y);
    }
}
