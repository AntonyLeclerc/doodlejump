using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject stars;
    private float movementX;
    private float movementY;
    public float speed = 3.0f;
    private float maxSpeed = 9.0f;

    private bool is_stun = false;

    public Sprite[] playerSprites; // Tableau pour stocker les sprites
    public SpriteRenderer playerRenderer;
    // playerSprites[0] : Sprite normal
    // playerSprites[1] : Sprite shoot
    // playerSprites[2] : Sprite jambes pliees


    public TextMeshProUGUI scoreText;

    private float leftBound;    // Limite gauche de l'�cran (plateforme bleue)
    private float rightBound;   // Limite droite de l'�cran (plateforme bleue)

    private Vector3 screenBottom;
    private float bottomBound;
    public float currentScore;
    private float highScore;
    private GameObject player;
    [SerializeField]
    private InputActionReference shoot;
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


        // D�finir les limites gauche et droite
        leftBound = screenLeft.x;
        rightBound = screenRight.x;
        bottomBound = screenBottom.y;
        // 
        gs = gameManager.getGameState();
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));
        bottomBound = screenBottom.y;

        if (shoot.action.triggered)
        {
            setNewSprite(1);
            SoundManager.Instance.PlaySound(AudioType.Shooting, AudioSourceType.Player);
            StartCoroutine(resetSpriteAfterDelay(0.1f));

            GameObject monster = GameObject.FindGameObjectWithTag("monster");

            if (monster != null)
            {
                Vector3 playerPos = player.transform.position;
                GameObject projectilePrefab = Resources.Load<GameObject>("projectile");
                GameObject projectile = Instantiate(projectilePrefab, playerPos, Quaternion.identity);

                ProjectileBehaviour projectileScript = projectile.GetComponent<ProjectileBehaviour>();
                if (projectileScript != null)
                {
                    projectileScript.target = monster;
                }
            }

            else
            {
                Debug.Log("Pas de monstre en vue");
            }
        }
    }

    void FixedUpdate()
    {
        
        gs = gameManager.getGameState();
        

        if ((gs == GAMESTATE.paused) || (gs == GAMESTATE.menu))
        {
            Vector2 pausedVelocity = Vector2.zero;
            rb.gameObject.GetComponent<Rigidbody2D>().velocity = pausedVelocity;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        if (gs == GAMESTATE.play)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 movement = new Vector2(movementX, movementY);
            rb.AddForce(movement * speed);
            Vector3 vel = rb.velocity;
            
            rb.velocity = new Vector3(Mathf.Clamp(vel.x, -maxSpeed, maxSpeed), vel.y, vel.z);

            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            Vector3 pos = player.transform.position;
            currentScore = Math.Max(pos.y, currentScore);
            scoreText.text = "Current score : " + ((int)(20 * currentScore)).ToString();

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
                if (PlayerPrefs.HasKey("highscore"))
                {
                    if ((PlayerPrefs.GetFloat("highscore")) < currentScore)
                    {
                        PlayerPrefs.SetFloat("highscore", currentScore);
                    }
                }
                else
                {
                    PlayerPrefs.SetFloat("highscore", currentScore);
                }
                gameManager.gameOver();
                gs = gameManager.getGameState();
                SoundManager.Instance.PlaySound(AudioType.Fall, AudioSourceType.Player);
            }
        }

        if (gs == GAMESTATE.gameOver)
        {
            
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (!is_stun)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    public float getCurrentScore()
    {
        // return currentScore;
        return currentScore;
    }

    public void setNewSprite(int index)
    {
        playerRenderer.sprite = playerSprites[index];
    }

    public void setStun()
    {
        is_stun = true;
        stars.GetComponent<starController>().setStarActive();
        rb.velocity = Vector2.down;
    }

    private IEnumerator resetSpriteAfterDelay(float value)
    {
        yield return new WaitForSeconds(value);
        // Debug.Log("D�pliage de jambes");

        setNewSprite(0);
    }
}
