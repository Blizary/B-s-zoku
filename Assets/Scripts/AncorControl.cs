using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncorControl : MonoBehaviour
{
    public string ancor;
    public Animator myAnimator;

    private bool animationSet;
    // Start is called before the first frame update
    void Start()
    {
        animationSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ancor!=null)
        {
            if(GameObject.FindGameObjectWithTag(ancor))
            {
                transform.position = GameObject.FindGameObjectWithTag(ancor).transform.position;

                if (!animationSet)
                {
                    myAnimator.SetBool("PosDeath", true);
                    animationSet = true;
                }

            }
           
            
        }
    }
}
