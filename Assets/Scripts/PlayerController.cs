using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movementX;
    private float movementY;
    public float speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector2 movement = new Vector2(movementX, movementY);
        rb.AddForce(movement * speed);

                

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("jumperPlatform"))
        {
            if (rb.transform.position.y > go.transform.position.y)
            {
                Rigidbody2D goRb = go.GetComponent<Rigidbody2D>();
                if (goRb != null)
                {
                    goRb.simulated = true;
                }

            }

            
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("normalPlatform"))
        {
            if (rb.transform.position.y > go.transform.position.y)
            {
                Rigidbody2D goRb = go.GetComponent<Rigidbody2D>();
                if (goRb != null)
                {
                    goRb.simulated = false;
                }

            }

            
        }*/


    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    private void FixedUpdate()
    {
        
    }
}
