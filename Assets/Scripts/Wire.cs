using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WorkGame
{
    public class Wire : MonoBehaviour
    {
        Vector3 startPoint;
        Vector3 defaultPosition;
        [SerializeField] SpriteRenderer wireEnd;
        [SerializeField] GameObject lightOn;

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

            // check for nearby connection points
            Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, .2f);
            foreach (Collider2D c in colliders)
            {
                // make sure it's not myself 
                if (c.gameObject != gameObject)
                {
                    // snap to connection 
                    UpdateWire(c.transform.position);

                    //check color of connection 
                    if (transform.parent.name.Equals(c.transform.parent.name))
                    {
                        // add points 
                        WorkManager.Instance.PointsChange(1); 

                        // solder connection 
                        c.GetComponent<Wire>()?.Done();
                        Done();
                    }

                    return;
                }
            }

            UpdateWire(newPosition);

        }

        void Done()
        {
            // turn on light
            lightOn.SetActive(true);
            Destroy(this);
        }

        void OnMouseUp()
        {
            // reset wire to default position 
            UpdateWire(defaultPosition);
        }

        void UpdateWire(Vector3 newPosition)
        {
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
}