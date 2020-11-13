using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [Header ("Properties")]
    public float speed;
    public float health;

    public GameObject target;

    private float currentHealth;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");//needs replacement in the future to acomodate 2 players
        currentHealth = health;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        ChaseTarget();
        DeathCheck();
    }

    /// <summary>
    /// simple chases the target around the map
    /// </summary>
    void ChaseTarget()
    {
        if(canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
       
    }

    /// <summary>
    /// Checks the enemys current life 
    /// </summary>
    void DeathCheck()
    {
        if (currentHealth <= 0)
        {
            canMove = false;
            //Enemy is dead here
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        //QUEU ANIMATIONS AND PRATICLES HERE
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);

    }
}
