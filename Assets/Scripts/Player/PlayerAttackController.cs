using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    /// <summary>
    ///  This list will hold all the gameobjects currently in range, when attacks get executed we will do a foreach on all the ones here and do the appropriate attack on each of the enemies.
    /// </summary>
    public List<GameObject> _inRange = new List<GameObject>();

    [SerializeField] private float Attack1Damage = 2;
    [SerializeField] private float Attack2Damage = 3;
    [SerializeField] private float ComboDamage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            _inRange.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            _inRange.Remove(collision.gameObject);
    }

    public void ExecuteAttack1()
    {
        foreach(GameObject go in _inRange)
        {
            go.GetComponent<EnemyControler>().Damage(Attack1Damage, gameObject.transform.parent.gameObject);
        }
    }

    public void ExecuteAttack2()
    {
        foreach (GameObject go in _inRange)
        {
            go.GetComponent<EnemyControler>().Damage(Attack2Damage, gameObject.transform.parent.gameObject);
        }
    }

    public void ExecuteCombo()
    {
        foreach (GameObject go in _inRange)
        {
            go.GetComponent<EnemyControler>().Damage(ComboDamage, gameObject.transform.parent.gameObject);
        }
    }
}
