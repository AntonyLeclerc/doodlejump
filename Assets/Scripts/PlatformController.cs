using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rbplayer;
    private string go_tag;

    private float speed = 2.0f;
    private float impulseForce;
    private float leftBound;    // Limite gauche de l'écran (plateforme bleue)
    private float rightBound;   // Limite droite de l'écran (plateforme bleue)
    private float bottomBound;   // Limite en bas de l'écran, pour détruire les plateformes une fois qu'elles ne seront plus visibles
    private Vector2 direction;  // Direction du mouvement

    // Pour les plateformes qui se cassent
    public SpriteRenderer spriteRenderer;
    public Sprite[] breakingSprites; // Tableau pour stocker les sprites
    private float breaking_time = 0.0f;
    public bool is_desotrying = false;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
        // Pour faire rebondir le joueur lorsqu'il atterit sur une plateforme
        player = GameObject.FindWithTag("player");
        rbplayer = player.GetComponent<Rigidbody2D>();
        
        // Récupère le tag de la plateforme
        go_tag = gameObject.tag;
        
        // Pour gérer les déplacements des plateformes mobiles, et leur destruction lorsque non visible en bas
        cam = Camera.main;
        Vector3 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.transform.position.z));
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));

        // Définir les limites gauche et droite
        leftBound = screenLeft.x;
        rightBound = screenRight.x;
        bottomBound = screenBottom.y;

        // Direction actuelle des plateformes mobiles
        direction = Vector2.right;

        
        // Pour faire sauter le joueur s'il atterit sur une plateforme autre qu'une "breaking_platform"
        if (go_tag == "normalPlatform" || go_tag == "movingPlatform")
        {
            impulseForce = 9.0f;
        }


    }

    private void FixedUpdate()
    {
        // Gestion du mouvement de la plateforme
        if (go_tag == "movingPlatform")
        {
            MovePlatform();
        }

        // Gestion du changement de sprite de la plateforme brisée
        if (go_tag == "breakingPlatform" && is_desotrying)
        {
            UpdateBreakingSprite();
        }

        // Détruis une plateforme si cette dernière disparais (en bas) de l'écran
        if (gameObject.transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));
        bottomBound = screenBottom.y;

        

    }

    private void MovePlatform()
    // Sert pour les plateformes bleues (qui se déplacent de gauche à droite à gauche)
    {


        transform.Translate(speed * Time.deltaTime * direction);

        // Si l'objet atteint le bord gauche ou droit, inverser la direction
        if (transform.position.x <= leftBound)
        {
            direction = Vector2.right;  // Changer la direction vers la droite
        }
        else if (transform.position.x >= rightBound)
        {
            direction = Vector2.left;   // Changer la direction vers la gauche
        }
    }

    private void UpdateBreakingSprite()
    {
        breaking_time += Time.deltaTime;

        // Vérifier quel sprite doit être affiché en fonction du temps écoulé
        int spriteIndex = Mathf.FloorToInt(breaking_time * 10); // Multiplier par 10 pour correspondre à l'index
        if (spriteIndex < breakingSprites.Length)
        {
            spriteRenderer.sprite = breakingSprites[spriteIndex];
        }

        // Détruire l'objet après un certain temps
        if (breaking_time >= breakingSprites.Length * 0.1f) // Temps total avant destruction
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Fais sauter le joueur si la plateforme n'est pas une "breaking_platform"   
        if (go_tag != "breakingPlatform")
        {
            
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                rbplayer.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Fais sauter le joueur si la plateforme n'est pas une "breaking_platform"   
        if (go_tag != "breakingPlatform")
        {

            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                rbplayer.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Gérer les actions lorsque le joueur entre en collision avec la plateforme brisée
        if (go_tag == "breakingPlatform")
        {
            is_desotrying = true; // Commencer le processus de destruction
        }
    }
}
