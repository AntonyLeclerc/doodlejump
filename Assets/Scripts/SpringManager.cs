using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringManager : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rbplayer;
    private string go_tag;

    private float speed = 2.0f;
    private float impulseForce;
    private float leftBound;    // Limite gauche de l'�cran (plateforme bleue)
    private float rightBound;   // Limite droite de l'�cran (plateforme bleue)
    private float bottomBound;   // Limite en bas de l'�cran, pour d�truire les plateformes une fois qu'elles ne seront plus visibles
    private Vector2 direction;  // Direction du mouvement
    public bool withDEBUG = false;
    // Pour les plateformes qui se cassent
    public bool is_desotrying = false;
    private Camera cam;

    [Header("UI References")]
    public Sprite[] springSprites; // Tableau pour stocker les sprites
    public SpriteRenderer springRenderer;
    // springSrpites[0] : Sprite normal
    // springSrpites[1] : Sprite spring étiré


    // Start is called before the first frame update
    void Start()
    {

        // Pour faire rebondir le joueur lorsqu'il atterit sur une plateforme
        player = GameObject.FindWithTag("player");
        rbplayer = player.GetComponent<Rigidbody2D>();

        // R�cup�re le tag de la plateforme
        go_tag = gameObject.tag;

        // Pour g�rer les d�placements des plateformes mobiles, et leur destruction lorsque non visible en bas
        cam = Camera.main;
        Vector3 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.transform.position.z));
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));

        // D�finir les limites gauche et droite
        leftBound = screenLeft.x;
        rightBound = screenRight.x;
        bottomBound = screenBottom.y;

        // Direction actuelle des plateformes mobiles
        direction = Vector2.right;

        // Pour faire sauter le joueur s'il atterit sur une plateforme autre qu'une "breaking_platform"

        if (go_tag == "spring")
        {
            impulseForce = 15.0f;
        }
    }

    public void setNewSprite(int index)
    {
        springRenderer.sprite = springSprites[index];
    }


    private void FixedUpdate()
    {
        
        // Gestion du mouvement de la plateforme
        float comp = player.transform.position.y - player.GetComponent<BoxCollider2D>().size.y / 2 - 0.0001f;

        

        // D�truis une plateforme si cette derni�re disparais (en bas) de l'�cran
        if (player.transform.position.y > gameObject.transform.position.y + player.GetComponent<BoxCollider2D>().size.y / 2)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenBottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z));
        bottomBound = screenBottom.y;



    }

    private void MovePlatform()
    // Sert pour les plateformes bleues (qui se d�placent de gauche � droite � gauche)
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

    

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {

            rbplayer.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter

            player.GetComponent<PlayerController>().setNewSprite(2);
            setNewSprite(1);
            SoundManager.Instance.PlaySound(AudioType.Spring, AudioSourceType.Game);
            StartCoroutine(resetSpriteAfterDelay());

        }
        
    }
    

    private IEnumerator resetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);

        setNewSprite(0);
    }
}
