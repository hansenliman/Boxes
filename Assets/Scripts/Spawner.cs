using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] enemies; // array of cube prefabs 
    public float spawnWait; // time between cube spawns
    public int startWait; // amount of time before cube starts spawning
    public bool stop; // bool that can stop the cube spawns
    public GameManager gameManager; // reference for the GameManager. (GameManager tracks the cubes, present in the game, in an array)

    private GameObject cube; // temp variable that stores the cube GameObject
    private int randEnemy; // temp variable that stores the cube type
    public int cubeIteration;
    public float escalator;

    const float SPAWNWAITSTART = 1.0f;

    // const declarations of spawnWait escalations
    const float ESCALATE1 = 0.2f; // until 0.6s
    const float ESCALATE2 = 0.1f; // until 0.5s
    const float ESCALATE3 = 0.0333f; // until 0.4s
    const float ESCALATE4 = 0.01f; // until 0.3s
    const float ESCALATE5 = 0.002f; // until 0.25s
    const float ESCALATE6 = 0.001f; // until 0.21s
    const float ESCALATE7 = 0.0001f; // until the player loses

    // const declarations of when spawnWait escalations should be applied
    const float TIMETHRESHOLD2 = 0.6f;
    const float TIMETHRESHOLD3 = 0.5f;
    const float TIMETHRESHOLD4 = 0.4f;
    const float TIMETHRESHOLD5 = 0.3f;
    const float TIMETHRESHOLD6 = 0.25f;
    const float TIMETHRESHOLD7 = 0.21f;


    void Start()
    {
        StartCoroutine(WaitSpawner());
        cubeIteration = 0;
        spawnWait = SPAWNWAITSTART;
        escalator = ESCALATE1;
    }
    
    IEnumerator WaitSpawner()
    {
        randEnemy = Random.Range(0, 2); // 0 = RED, 1 = BLUE
        cube = Instantiate(enemies[randEnemy], transform.TransformPoint(0, 0, 0), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        gameManager.Enqueue(cube, randEnemy);
        //yield return new WaitForSeconds(startWait);
        while (!stop)
        {
            if (!gameManager.gameInitializedStatus)
            {
                yield return new WaitForSeconds(spawnWait);
                continue;
            }
            if (gameManager.gameOverStatus)
            {
                stop = true;
                continue;
            }
            randEnemy = Random.Range(0, 2); // 0 = RED, 1 = BLUE
            cube = Instantiate(enemies[randEnemy], transform.TransformPoint(0, 0, 0), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            gameManager.Enqueue(cube, randEnemy);
            IncreaseIteration();
            
            yield return new WaitForSeconds(spawnWait);
        }
    }

    void IncreaseIteration()
    {
        cubeIteration++;
        if(cubeIteration >= 8)
        {
            cubeIteration = 0;
            spawnWait -= escalator;
        }
        
        if(spawnWait <= TIMETHRESHOLD2 && escalator > ESCALATE2)
        {
            escalator = ESCALATE2;
        }
        else if (spawnWait <= TIMETHRESHOLD3 && escalator > ESCALATE3)
        {
            escalator = ESCALATE3;
        }
        else if (spawnWait <= TIMETHRESHOLD4 && escalator > ESCALATE4)
        {
            escalator = ESCALATE4;
        }
        else if (spawnWait <= TIMETHRESHOLD5 && escalator > ESCALATE5)
        {
            escalator = ESCALATE5;
        }
        else if (spawnWait <= TIMETHRESHOLD6 && escalator > ESCALATE6)
        {
            escalator = ESCALATE5;
        }
        else if (spawnWait <= TIMETHRESHOLD7 && escalator > ESCALATE7)
        {
            escalator = ESCALATE5;
        }

    }
}