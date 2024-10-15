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

    private float leftBound;    // Limite gauche de l'écran (plateforme bleue)
    private float rightBound;   // Limite droite de l'écran (plateforme bleue)
    private float bottomBound;

    private GameObject player;
    // add game states
    [SerializeField]
    private GameManager gameManager;
    private GAMESTATE gs;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("player");

        Camera cam = Camera.main;
        Vector3 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.transform.position.z));
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));


        // Définir les limites gauche et droite
        leftBound = screenLeft.x;
        rightBound = screenRight.x;
        bottomBound = screenBottom.y;
        // 
        gs = gameManager.getGameState();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(movementX, movementY);
        rb.AddForce(movement * speed);

        if (player.transform.position.x < leftBound)
        {
            player.transform.position = new Vector3(rightBound, player.transform.position.y, player.transform.position.z);
        }

        else if (player.transform.position.x > rightBound)
        {
            player.transform.position = new Vector3(leftBound, player.transform.position.y, player.transform.position.z);
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
}
