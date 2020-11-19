using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    public List<GameObject> playersInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if(col.CompareTag("Player"))
        {
            playersInRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Remove(other.gameObject);
        }
    }

    public void AttackTargets(float _damage)
    {
        Debug.Log("ATTACKED PLAYER");
        for(int i = 0;i<playersInRange.Count;i++)
        {
            playersInRange[i].GetComponent<PlayerStats>().Damage(_damage);
        }
    }
}
