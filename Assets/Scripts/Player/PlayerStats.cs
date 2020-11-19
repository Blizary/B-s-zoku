using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public float health;


    private float maxhealth;
    private MainLevelManager manager;
    // Start is called before the first frame update
    void Start()
    {
        maxhealth = health;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MainLevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log("player took damage");
    }

    void Death()
    {
        if(health<=0)
        {
            Debug.Log("Player died");
            manager.DeathScreen();
            Destroy(this.gameObject);

        }
    }
}
