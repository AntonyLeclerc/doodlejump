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
        Vector2 movement = new Vector2(movementX, movementY);
        rb.AddForce(movement * speed);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("normalPlatform"))
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

    // Gérer les collisions avec des plateformes
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("normalPlatform"))
        {
            rb.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter
        }

        if (other.gameObject.CompareTag("jumperPlatform"))
        {
            rb.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter
        }
    }
}
