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
    public float breakingProba=0.2f;
    public float normalProba;
    private float[] probs;
    // private float amplitude=0.2f;
    private float amplitude=0.3f;
    // private float amplitude2 = 0.4f;
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
            /*
            if (nbplateform >= plateformThreshold)
            {
                // cleanPlateforms();
                nbplateform = this.transform.childCount;
            }
            */
            //generatePlateforms();
            cleanPlateforms();
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
        // Debug.Log("Calling Clean");
        nbplateform = this.transform.childCount;
        Vector3 bottomScreen = new Vector3(Screen.width / 2, 0, 0);
        Vector3 bottom = Camera.main.ScreenToWorldPoint(bottomScreen);
        for (int i=0; i < nbplateform; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            //
            if (child.transform.position.y < bottom.y)
            {
                Destroy(child);
                generatePlateforms();
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
            // float spawnY = UnityEngine.Random.Range(center.y+amplitude, topLeft.y+ amplitude2);
            float spawnY = topLeft.y + UnityEngine.Random.Range(0, 2.0f+ amplitude);
            while (!checkValidity(spawnX,spawnY))
            {
                spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
                // spawnY = UnityEngine.Random.Range(center.y , topLeft.y + amplitude2);
                spawnY = topLeft.y + UnityEngine.Random.Range(0, 2.0f+ amplitude);
                // Debug.Log("Retry");
            }
            
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

            int choice = Choose(probs);
        
            GameObject go = Instantiate(prefabsList[choice],spawnPos,Quaternion.identity,this.transform);

            // il faut au moins un platform différent de breakingPlatform qui peut être atteint
            if (choice == 2){
                // Debug.Log("platform choiced : "+prefabsList[choice]);
                float spawnX_norm = UnityEngine.Random.Range(topLeft.x, topRight.x);
                // float spawnY = UnityEngine.Random.Range(center.y+amplitude, topLeft.y+ amplitude2);
                float spawnY_norm = UnityEngine.Random.Range(spawnY-0.1f, spawnY+0.1f);
                while (!checkValidity(spawnX_norm,spawnY_norm))
                {
                    spawnX_norm = UnityEngine.Random.Range(topLeft.x, topRight.x);
                    // spawnY = UnityEngine.Random.Range(center.y , topLeft.y + amplitude2);
                    spawnY_norm = UnityEngine.Random.Range(spawnY-0.1f, spawnY+0.1f);
                    // Debug.Log("Retry norm");
                }
                
                Vector3 spawnPos_norm = new Vector3(spawnX_norm, spawnY_norm, 0);
                GameObject go_norm = Instantiate(prefabsList[0],spawnPos_norm,Quaternion.identity,this.transform);
            }
        }
        /*else
        {
            Debug.Log("Too much plateforms " + nbplateform);
        }*/

    }

    private bool checkValidity(float spawnX, float spawnY)
    {
        // Debug.Log("check");
        Vector3 pos = new Vector3(spawnX, spawnY,0);
        // Check distance
        float dist = 1.0f;
        foreach(Transform child in this.transform)
        {
            // Debug.Log("dist = "+(child.position.y - pos.y));
            if (Vector3.Distance(child.position, pos) < dist)
            {
                return false;
            }
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
