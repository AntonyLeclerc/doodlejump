using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlateformManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    private int nbplateform;
    private GAMESTATE gs;
    private Vector3 topCamera;
    //Prefabs References
    [SerializeField]
    private GameObject normalPlateformPrefab;
    [SerializeField]
    private GameObject movingPlateformPrefab;
    [SerializeField]
    private GameObject breakingPlateformPrefab;

    private List<GameObject> prefabsList;
    // plateform spawn probabilities
    public float movingProba=0.3f;
    public float breakingProba=0.1f;
    public float normalProba;
    private float[] probs;
    private float amplitude=0.2f;
    private float amplitude2 = 0.4f;
    // Determine when you should start cleaning
    public int plateformThreshold= 50;
    // Start is called before the first frame update
    void Start()
    {
        nbplateform = this.transform.childCount;
        normalProba =1-movingProba-breakingProba;
        probs =new float[]{ normalProba, movingProba, breakingProba};
        prefabsList = new List<GameObject>();
        //init prefab List
        prefabsList.Add(normalPlateformPrefab);
        prefabsList.Add(movingPlateformPrefab);
        prefabsList.Add(breakingPlateformPrefab);
    }

    private void FixedUpdate()
    {
        if (gs == GAMESTATE.play)
        {
            if (nbplateform >= plateformThreshold)
            {
                //cleanPlateforms();
                nbplateform = this.transform.childCount;
            }
            generatePlateforms();
            nbplateform = this.transform.childCount;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //
    }


    private void LateUpdate()
    {
        gs = gameManager.getGameState();
    }
    // 
    private void cleanPlateforms()
    {
        Debug.Log("Calling Clean");
        Vector3 bottomScreen = new Vector3(Screen.width / 2, 0, 0);
        Vector3 bottom = Camera.main.ScreenToWorldPoint(bottomScreen);
        for (int i=0; i < nbplateform; i++)
        {
            Transform child = this.transform.GetChild(i);
            //
            if (child.transform.position.y < bottom.y)
            {
                Destroy(child);
            }
            else
            {
                break;
            }
        }
    }
    private void generatePlateforms()
    {
        nbplateform = this.transform.childCount;
        if (nbplateform < plateformThreshold)
        {
            Vector3 screenTopLeft = new Vector3(0, Screen.height, 0);
            Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, 0);
            Vector3 topLeft=Camera.main.ScreenToWorldPoint(screenTopLeft); ;
            Vector3 topRight = Camera.main.ScreenToWorldPoint(screenTopRight);

            Vector3 screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
            Vector3 center = Camera.main.ScreenToWorldPoint(screenCenter);
            float spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
            float spawnY = UnityEngine.Random.Range(center.y+amplitude, topLeft.y+ amplitude2);
            while (!checkValidity(spawnX,spawnY))
            {
                spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
                spawnY = UnityEngine.Random.Range(center.y , topLeft.y + amplitude2);
            }
            
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

            int choice = Choose(probs);
        
            GameObject go = Instantiate(prefabsList[choice],spawnPos,Quaternion.identity,this.transform);
        }
        /*else
        {
            Debug.Log("Too much plateforms " + nbplateform);
        }*/

    }

    private bool checkValidity(float spawnX, float spawnY)
    {
        Vector3 pos = new Vector3(spawnX, spawnY,0);
        //Ray construction
        Ray up = new Ray(pos,pos+Vector3.up);
        Ray down = new Ray(pos, pos+Vector3.down);
        Ray right = new Ray(pos, pos + Vector3.right);
        Ray left = new Ray(pos, pos + Vector3.left);
        Ray TopL = new Ray(pos, pos + Vector3.up+ Vector3.left);
        Ray TopR = new Ray(pos, pos + Vector3.up+ Vector3.right);
        Ray DownL = new Ray(pos, pos + Vector3.down+ Vector3.left);
        Ray DownR = new Ray(pos, pos + Vector3.down+ Vector3.right);
        // Check distance
        float dist = 1f;
        Debug.DrawLine(pos,pos+Vector3.up);
        if ((Physics.Raycast(up,dist))||(Physics.Raycast(up, dist)) || (Physics.Raycast(up, dist)) || (Physics.Raycast(up, dist))
            || (Physics.Raycast(up, dist)) || (Physics.Raycast(up, dist)) || (Physics.Raycast(up, dist)) || (Physics.Raycast(up, dist)))
        {
            return false;
        }
        return true;
    }

    int Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = UnityEngine.Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
    public int getNbplateform()
    {
        return nbplateform;
    }
    public GameManager getGameManagerPlateform()
    {
        return gameManager;
    }
}
