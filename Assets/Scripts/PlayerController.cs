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

    private Vector3 screenBottom;
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
        //Debug.Log(" Velocit " + rb.gameObject.GetComponent<Rigidbody2D>().velocity);

        Camera cam = Camera.main;
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));
        bottomBound = screenBottom.y;
        Debug.Log(bottomBound);


    }
    void FixedUpdate()
    {
        
        gs = gameManager.getGameState();
        if ((gs == GAMESTATE.paused) || (gs == GAMESTATE.menu))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        if (gs == GAMESTATE.play)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 movement = new Vector2(movementX, movementY);
            rb.AddForce(movement * speed);
            Vector3 pos = player.transform.position;
            // Torus map
            if (pos.x < leftBound)
            {
                player.transform.position = new Vector3(rightBound, player.transform.position.y, player.transform.position.z);
            }
            else if (pos.x > rightBound)
            {
                player.transform.position = new Vector3(leftBound, player.transform.position.y, player.transform.position.z);
            }
            //Check GameOver
            if(pos.y < bottomBound)
            {
                gameManager.gameOver();
                gs = gameManager.getGameState();
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
}
