using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public float health;


    private float maxhealth;
    // Start is called before the first frame update
    void Start()
    {
        maxhealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log("player took damage");
    }
}
