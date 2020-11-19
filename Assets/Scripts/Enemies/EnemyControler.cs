using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControler : MonoBehaviour
{
    [Header ("Properties")]
    public float speed;
    public float health;
    public float damage;
    public float minDistanceToTarget;
    public float attackRange;
    public float attackDelay;


    [Header("References")]
    public GameObject target;
    public GameObject explosionPS;
    public GameObject collisionBox;
    public GameObject floatingDamageNumber;
    public GameObject slider;
    

    private float currentHealth;
    private bool canMove;
    private bool closeToTarget;
    public bool inAttackRange;
    private Vector3 knockbackDir;
    private float innerAttackDelay;
    private bool attacking;

    // Cached component references
    private Animator myAnimator;
    private Rigidbody2D rb;
 

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

        CheckAttackRange();
        AttackPlayer();

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
                    if(Mathf.Abs( transform.position.x - target.transform.position.x) >= attackRange-0.2f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (speed / 2) * Time.deltaTime);
                    }
                    
                }
            }
           
           
        }
    }

    /// <summary>
    /// Checks the distance between the AI and the target player
    /// </summary>
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

    /// <summary>
    /// Checks if the player is within melee range
    /// </summary>
    void CheckAttackRange()
    {

        if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            inAttackRange = true;
        }
        else
        {
            inAttackRange = false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    void AttackPlayer()
    {
        if (!attacking)
        {
            if (inAttackRange == true)//enemy is in range
            {
                Debug.Log("close enough to attack");
                StartCoroutine(IEAttackPlayer());
            }
        }
    }

    IEnumerator IEAttackPlayer()
    {
        canMove = false;
        attacking = true;
        yield return new WaitForSeconds(attackDelay);
        collisionBox.GetComponent<EnemyCollider>().AttackTargets(damage);
        attacking = false;
        canMove = true;
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
    public void Damage(float _damage, GameObject _player)
    {
        myAnimator.SetTrigger("EnemyWasHit");

        // Every time the enemy gets hit we want to give them a slight knockback
        Vector2 _knockbackDir = transform.position- _player.transform.position;
        StartCoroutine(DamageKnowbackEffect(_knockbackDir));

        currentHealth -= _damage;
        GameObject floatingDamageNr = Instantiate(floatingDamageNumber, transform.position, Quaternion.identity);
        TextMesh floatingDamageNrTextMesh = floatingDamageNr.transform.GetChild(0).GetComponent<TextMesh>();
        floatingDamageNrTextMesh.text = "-" + _damage;
        
        switch(_damage)
        {
            case 2:
                floatingDamageNrTextMesh.color = Color.white;
                break;
            case 3:
                floatingDamageNrTextMesh.color = Color.green;
                break;
            case 5:
                floatingDamageNrTextMesh.color = Color.cyan;
                break;
        }

        
        slider.GetComponent<Slider>().value = currentHealth;
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
