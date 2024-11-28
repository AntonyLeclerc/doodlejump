using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public GameObject target;
    public float speed = 1.0f;

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        else if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("monster") || collision.CompareTag("movingMonster"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Destroy(target);

        }
    }
}
