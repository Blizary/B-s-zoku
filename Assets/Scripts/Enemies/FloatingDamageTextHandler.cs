using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageTextHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 0.5f, 0); // Make text appear little bit above enemy
    }
}
