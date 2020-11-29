using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public float health;
    public Slider healthSlider;


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
        Debug.Log("player took " + damage + " damage");
        healthSlider.GetComponent<Slider>().value = health;
    }

    void Death()
    {
        if(health<=0)
        {
            Debug.Log("Player died");
            manager.DeathScreen();
            manager.SetLastEvent();
            Destroy(this.gameObject);
            

        }
    }
}
