using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterController : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rbplayer;

    private float speed = 3.0f;
    private float impulseForce = 9.0f;
    private float leftBound;    // Limite gauche de l'�cran (plateforme bleue)
    private float rightBound;   // Limite droite de l'�cran (plateforme bleue)
    private float bottomBound;   // Limite en bas de l'�cran, pour d�truire les plateformes une fois qu'elles ne seront plus visibles
    private Vector2 direction;  // Direction du mouvement
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites; // Tableau pour stocker les sprites
    private float time = 0.0f;
    private Camera cam;
    private GameManager gameManager;
    private GAMESTATE gs;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player");
        rbplayer = player.GetComponent<Rigidbody2D>();
        
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

        this.gameManager = this.transform.parent.gameObject.GetComponent<MonsterManager>().getGameManagerPlateform();
    }

    void FixedUpdate()
    {
        gs = gameManager.getGameState();
        
        if(!(gs==GAMESTATE.paused || gs==GAMESTATE.menu || gs==GAMESTATE.gameOver))
            Move();
            if (gameObject.tag != "movingMonster")
                UpdateSprite();
    }

    private void Move(){
        transform.Translate(speed * Time.deltaTime * direction);

        // Si l'objet atteint le bord gauche ou droit, inverser la direction
        if (transform.position.x <= leftBound)
        {
            direction = Vector2.right;  // Changer la direction vers la droite
            if (gameObject.tag == "movingMonster")
                spriteRenderer.sprite = sprites[0];
        }
        else if (transform.position.x >= rightBound)
        {
            direction = Vector2.left;   // Changer la direction vers la gauche
            if (gameObject.tag == "movingMonster")
                spriteRenderer.sprite = sprites[1];
        }
    }

    private void UpdateSprite()
    {
        time += Time.deltaTime;

        // V�rifier quel sprite doit �tre affich� en fonction du temps �coul�
        int spriteIndex = Mathf.FloorToInt(time * 10); // Multiplier par 10 pour correspondre � l'index
        spriteRenderer.sprite = sprites[spriteIndex % sprites.Length];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "projectile"){
            Destroy(this);
        }

        if (collision.transform.tag == "player"){
            float comp = player.transform.position.y - player.GetComponent<BoxCollider2D>().size.y / 2 - 0.0001f;
            if (this.transform.position.y > comp)
            {
                player.GetComponent<BoxCollider2D>().enabled = false;
                rbplayer.velocity = Vector2.zero;
                player.GetComponent<PlayerController>().setStun();
            }
            else{
                rbplayer.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
                Destroy(gameObject);
                player.GetComponent<PlayerController>().setNewSprite(2);
                SoundManager.Instance.PlaySound(AudioType.Platform, AudioSourceType.Game);
                StartCoroutine(resetSpriteAfterDelay());
            }
        }
    }

    private IEnumerator resetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        // Debug.Log("D�pliage de jambes");

        player.GetComponent<PlayerController>().setNewSprite(0);
    }
}
