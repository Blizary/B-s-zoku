using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float respawnTimer;
    public GameObject enemyPrefab;
    public bool timedSpawn;

    private float irespawnTimer;
    public int numToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        timedSpawn = false;
        irespawnTimer = respawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(timedSpawn && numToSpawn>0)
        {
            Spawner();
        }
       
    }


    void Spawner()
    {
        if(irespawnTimer<=0)
        {
            irespawnTimer = respawnTimer;
            //spawn mob
            GameObject newMob = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, this.transform);
            DifficulyOverTime();
            numToSpawn -= 1;
            //Debug.Log("spawning");

        }
        else
        {
            irespawnTimer -= Time.deltaTime;
        }
    }

    void DifficulyOverTime()
    {
        if(respawnTimer>=2)
        {
            respawnTimer -= 0.5f;
        }
        

    }

    public void Spawn(GameObject enemy)
    {
        timedSpawn = false;
        GameObject newMob = Instantiate(enemy, this.transform.position, Quaternion.identity, this.transform);
    }

    public void UpdateEnemy(GameObject enemy,int num)
    {
        enemyPrefab = enemy;
        numToSpawn = num;
        timedSpawn = true;
    }
}
