using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Rigidbody2D rb;
    private GameManager gameManager;

    private float passedTime = 0f;
    private float attractionSpeed = 7.5f; // Vitesse d'attraction au trou noir
    private bool isAttracting = false;

    public float stoppingDistance = 0.2f;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("player")[0];
        rb = player.GetComponent<Rigidbody2D>();
        gameManager = transform.parent.GetComponent<PlateformManager>().getGameManagerPlateform();
    }

    void Update()
    {
        if (isAttracting)
        {
            passedTime += Time.deltaTime;

            // Calcule la distance entre le joueur et le trou noir pour d�clencher le gameover
            float distanceToBlackHole = Vector3.Distance(player.transform.position, transform.position);

            
            if (distanceToBlackHole <= stoppingDistance)
            {
                gameManager.gameOver(); // GameOver quand tr�s proche du centre du trou noir
            }
            else
            {
                // D�place le joueur vers le trou noir
                player.transform.position = Vector3.MoveTowards(player.transform.position,
                                                                 transform.position,
                                                                 attractionSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isAttracting = true;
            rb.velocity = Vector2.zero; // Met la v�locit� du joueur � 0 pour ne pas "g�ner" le d�but de l'attarction par le trou noir

        }
    }

}
