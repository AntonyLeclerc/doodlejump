using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlateformManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerController playerController;
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
    [SerializeField]
    private GameObject blackHolePrefab;

    public List<GameObject> prefabsList = new List<GameObject>();
    public GameObject springPrefab;
    // plateform spawn probabilities
    public float movingProba=0.2f;
    public float breakingProba=0.1f;
    public float blackHoleProba=0.002f;
    public float springProba = 0.05f;
    public float normalProba;
    private float[] probs;

    // private float amplitude=0.2f;
    private float amplitude=0.3f;
    // private float amplitude2 = 0.4f;

    // Determine when you should start cleaning
    public int plateformThreshold= 9;      // nb plateform normal et moving

    private int nbBlackHole = 0;
    private int nbBreakingPlateform;

    // Start is called before the first frame update
    void Start()
    {
        nbplateform = this.transform.childCount;
        normalProba = 1-movingProba-breakingProba;
        probs = new float[]{ normalProba, movingProba, breakingProba};
        
    }

    private void FixedUpdate()
    {
        if (gs == GAMESTATE.play)
        {
            
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
    private void cleanPlateforms()
    {
        nbplateform = this.transform.childCount;
        nbBreakingPlateform = getNbBreakingPlateform();
        Vector3 bottomScreen = new Vector3(Screen.width / 2, 0, 0);
        Vector3 bottom = Camera.main.ScreenToWorldPoint(bottomScreen);
        for (int i=0; i < nbplateform; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            if (child.transform.position.y < bottom.y)
            {
                if (child.tag == "blackHolePlatform")
                {
                    nbBlackHole--;
                }
                Destroy(child);
                while (this.transform.childCount <= plateformThreshold + nbBlackHole + nbBreakingPlateform)
                {
                    generatePlateforms();
                }
            }
        }
        float randomvalue = UnityEngine.Random.value;
        if (randomvalue < blackHoleProba && nbBlackHole < 1 && (int)(20 * playerController.getCurrentScore()) > 1500)
        {
            generateBlackHole();
        }
    }
    private void generatePlateforms()
    {
        Vector3 screenTopLeft = new Vector3(0, Screen.height, 0);
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, 0);
        Vector3 topLeft=Camera.main.ScreenToWorldPoint(screenTopLeft); ;
        Vector3 topRight = Camera.main.ScreenToWorldPoint(screenTopRight);

        Vector3 screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        Vector3 center = Camera.main.ScreenToWorldPoint(screenCenter);
        float spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
        // float spawnY = UnityEngine.Random.Range(center.y+amplitude, topLeft.y+ amplitude2);
        float spawnY = topLeft.y + UnityEngine.Random.Range(-1.0f, 1.0f);
        while (!checkValidity(spawnX,spawnY))
        {
            spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
            spawnY = topLeft.y + UnityEngine.Random.Range(-1.0f-amplitude, 1.0f+amplitude);
        }
        
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        int choice = Choose(probs);
    
        GameObject go = Instantiate(prefabsList[choice],spawnPos,Quaternion.identity,this.transform);


        float probSpring = UnityEngine.Random.Range(0f, 1f);
        if (probSpring <= springProba && choice != 2)
        {
            Vector3 spawnPosSpring = new Vector3(spawnX, spawnY + 0.25f, 0);
            GameObject spring = Instantiate(springPrefab, spawnPosSpring, Quaternion.identity, go.transform);
        }

        // il faut au moins un platform différent de breakingPlatform qui peut être atteint
        if (choice == 2){
            float spawnX_norm = UnityEngine.Random.Range(topLeft.x, topRight.x);
            float spawnY_norm = UnityEngine.Random.Range(spawnY-0.1f, spawnY+0.1f);
            while (!checkValidity(spawnX_norm,spawnY_norm))
            {
                spawnX_norm = UnityEngine.Random.Range(topLeft.x, topRight.x);
                spawnY_norm = UnityEngine.Random.Range(spawnY-0.1f, spawnY+0.1f);
            }
            
            Vector3 spawnPos_norm = new Vector3(spawnX_norm, spawnY_norm, 0);
            GameObject go_norm = Instantiate(prefabsList[0],spawnPos_norm,Quaternion.identity,this.transform);
        }
    }

    private void generateBlackHole()
    {
        Vector3 screenTopLeft = new Vector3(0, Screen.height, 0);
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, 0);
        Vector3 topLeft=Camera.main.ScreenToWorldPoint(screenTopLeft); ;
        Vector3 topRight = Camera.main.ScreenToWorldPoint(screenTopRight);

        Vector3 screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        Vector3 center = Camera.main.ScreenToWorldPoint(screenCenter);
        float spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
        float spawnY = topLeft.y + UnityEngine.Random.Range(-1.0f, 1.0f);
        while (!checkValidity(spawnX,spawnY))
        {
            spawnX = UnityEngine.Random.Range(topLeft.x, topRight.x);
            spawnY = topLeft.y + UnityEngine.Random.Range(-1.0f-amplitude, 1.0f+amplitude);
        }
        
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
    
        GameObject go = Instantiate(blackHolePrefab,spawnPos,Quaternion.identity,this.transform);
        nbBlackHole++;

    }

    private bool checkValidity(float spawnX, float spawnY)
    {
        Vector3 pos = new Vector3(spawnX, spawnY,0);
        // Check distance
        float dist = 1.2f;
        foreach(Transform child in this.transform)
        {
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

    public int getNbBreakingPlateform()
    {
        Transform t = this.transform;
        int count = 0;

        for (int i = 0; i < t.childCount; i++) 
		{
			if(t.GetChild(i).gameObject.tag == "breakingPlatform")
			{
				count++;
			}	
		}
        return count;
    }

    public GameManager getGameManagerPlateform()
    {
        return gameManager;
    }
}
