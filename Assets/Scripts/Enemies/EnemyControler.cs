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
    public float minDistanceToTarget;
    public float attackRange;

    private float currentHealth;
    private bool canMove;
    private bool closeToTarget;
    private bool inAttackRange;

    // Cached component references
    private Animator myAnimator;
    private Rigidbody2D rb;
    private Vector3 knockbackDir;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");//needs replacement in the future to acomodate 2 players
        currentHealth = health;
        canMove = true;

        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DeathCheck();
        CloseToTarget();
        KnockBack();
        ChaseTarget();
       
    }

    /// <summary>
    /// simple chases the target around the map
    /// </summary>
    void ChaseTarget()
    {
        if (canMove)
        {
            if (!closeToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                if (Mathf.Abs( transform.position.y - target.transform.position.y) >= 0.2f)//not on the same line as player
                {            
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.transform.position.y, transform.position.z), (speed/2) * Time.deltaTime);
                }
                else
                {
                    if(Mathf.Abs( transform.position.x - target.transform.position.x) >= attackRange)
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                }
            }
           
           
        }
    }

    void CloseToTarget()
    {
        if(Vector3.Distance(transform.position,target.transform.position)<= minDistanceToTarget)
        {
            closeToTarget = true;
        }
        else
        {
            closeToTarget = false;
        }
    }

    void CheckAttackRange()
    {
        if(closeToTarget == true)
        { 
        }
    }

    void AttackPlayer()
    {
        
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
        Vector2 _knockbackDir = transform.position- player.transform.position;
        StartCoroutine(DamageKnowbackEffect(_knockbackDir));
        //GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * -120f);

        currentHealth -= damage;
        Debug.Log("Enemy Was Hit for " + damage + " - " + currentHealth + " health remaining.");
        myAnimator.SetTrigger("EnemyWasHit");
    }

    IEnumerator DamageKnowbackEffect(Vector3 dir)
    {
        canMove = false;
        dir = dir.normalized;
        dir = transform.position + (dir*0.5f);
        knockbackDir = dir;
        //particle on hit
        yield return new WaitForSeconds(1);
        knockbackDir = Vector3.zero;
        canMove = true;
          
    }

    void KnockBack()
    {
        if(knockbackDir!=Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, knockbackDir, speed * Time.deltaTime*2);
        }
    }
}
