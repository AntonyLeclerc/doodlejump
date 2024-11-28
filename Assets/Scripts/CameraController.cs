using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        Vector3 pos = player.transform.position;
        if (pos.y > this.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, -1);
        }
    }
}
