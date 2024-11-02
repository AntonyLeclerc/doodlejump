using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerController playerController;
    private int nbMonster;
    private int monsterThreshold = 1;
    private GAMESTATE gs;
    private Vector3 topCamera;
    //Prefabs References
    [SerializeField]
    private GameObject monsterAPrefab;
    [SerializeField]
    private GameObject monsterBPrefab;
    [SerializeField]
    private GameObject monsterCPrefab;
    [SerializeField]
    private GameObject monsterDPrefab;
    [SerializeField]
    private GameObject monsterEPrefab;

    private List<GameObject> prefabsList;
    // spawn probabilities
    private float spawnProb = 0.02f;
    private double[] probs;

    // Start is called before the first frame update
    void Start()
    {
        nbMonster = this.transform.childCount;
        prefabsList = new List<GameObject>();
        //init prefab List
        prefabsList.Add(monsterAPrefab);
        prefabsList.Add(monsterBPrefab);
        prefabsList.Add(monsterCPrefab);
        prefabsList.Add(monsterDPrefab);
        prefabsList.Add(monsterEPrefab);

        probs = new double[5];
        for (int i = 0; i < 5; i++)
        {
            probs[i] = (1.0f/5.0f);
        }
    }

    void FixedUpdate()
    {
        if (gs == GAMESTATE.play)
        {
            cleanMonsters();
            nbMonster = this.transform.childCount;
        }
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        gs = gameManager.getGameState();
    }

    private void cleanMonsters()
    {
        nbMonster = this.transform.childCount;
        Vector3 bottomScreen = new Vector3(Screen.width / 2, 0, 0);
        Vector3 bottom = Camera.main.ScreenToWorldPoint(bottomScreen);
        for (int i=0; i < nbMonster; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            //
            if (child.transform.position.y < bottom.y)
            {
                Destroy(child);
            }
        }
        float randomvalue = UnityEngine.Random.value;
        if (randomvalue < spawnProb && nbMonster < monsterThreshold && (int)(20 * playerController.getCurrentScore()) > 1000)
        {
            generateMonsters();
        }
    }

    private void generateMonsters()
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
        
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        int choice = Choose(probs);
    
        GameObject go = Instantiate(prefabsList[choice],spawnPos,Quaternion.identity,this.transform);
    }

    int Choose(double[] probs)
    {

        double total = 0;

        foreach (double elem in probs)
        {
            total += elem;
        }

        double randomPoint = UnityEngine.Random.value * total;

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

    public GameManager getGameManagerPlateform()
    {
        return gameManager;
    }
}
