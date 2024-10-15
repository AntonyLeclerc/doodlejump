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
        //offset = this.transform.position - player.transform.position;
        offset = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        
        if (player.transform.position.y > this.transform.position.y)
        {
            transform.position = player.transform.position+offset ;
        }
    }
}
