using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BlackHoleManager : MonoBehaviour
{

    public GameObject player;
    private Rigidbody2D rb;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        this.gameManager = this.transform.parent.gameObject.GetComponent<PlateformManager>().getGameManagerPlateform();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Entrée dans un trou noir");
        gameManager.gameOver();

    }
}
