using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [Header ("Properties")]
    public float speed;
    public float health;

    public GameObject target;
    public GameObject explosionPS;

    private float currentHealth;
    private bool canMove;

    // Cached component references
    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");//needs replacement in the future to acomodate 2 players
        currentHealth = health;
        canMove = true;

        myAnimator = GetComponent<Animator>();
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
        if (canMove)
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
        yield return new WaitForSeconds(0.0f);
        Instantiate(explosionPS, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }

    /// This function is called externally by the PlayerAttackController script to damage them the appropriate amount
    public void Damage(float damage, GameObject player)
    {
        // Every time the enemy gets hit we want to give them a slight knockback
        Vector2 moveDirection = player.GetComponent<Rigidbody2D>().transform.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * -120f);

        currentHealth -= damage;
        Debug.Log("Enemy Was Hit for " + damage + " - " + currentHealth + " health remaining.");
        myAnimator.SetTrigger("EnemyWasHit");
    }
}
