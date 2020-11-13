using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float respawnTimer;
    public GameObject enemyPrefab;

    private float irespawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        irespawnTimer = respawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Spawner();
    }


    void Spawner()
    {
        if(irespawnTimer<=0)
        {
            irespawnTimer = respawnTimer;
            //spawn mob
            GameObject newMob = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, this.transform);
            Debug.Log("spawning");

        }
        else
        {
            irespawnTimer -= Time.deltaTime;
        }
    }
}
