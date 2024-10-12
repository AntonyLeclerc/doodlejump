using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    public GameObject player;
    private Rigidbody2D rbplayer;

    // Start is called before the first frame update
    void Start()
    {
        rbplayer = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rbplayer.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse); // Appliquer une force vers le haut pour sauter

    }
}
