using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLocations : MonoBehaviour
{
    public TextAnimatorPlayer textDisplay;
    public GameObject textBuble;
    public GameObject playerLocation;
    public GameObject spriteR;
    public AncorControl ancor;

    public string newText;
    private MainLevelManager manager;

    void Start()
    {
        manager=GameObject.FindGameObjectWithTag("Manager").GetComponent<MainLevelManager>();
    }


    public void DisplayText(string _text)
    {
        newText = _text;
        if (ancor!=null)
        {
            ancor.GetComponent<AncorControl>().LateDestroy();
        }
        spriteR.SetActive(true);
        MoveOrder();
        

    }

    public void MoveOrder()
    {
        if(Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, playerLocation.transform.position) >= 1)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IssueMovement(playerLocation.transform, this);
            //Debug.Log("Player is ordered to move");
        }
        else
        {
            ShowText();
            //Debug.Log("Player is there");
        }
        
    }

    public void ShowText()
    {
        textBuble.SetActive(true);
        textDisplay.ShowText(newText);
    }

    public void SetAsleepDisplay()
    {
        textBuble.SetActive(false);
    }

    public void DisableVisuals()
    {
        spriteR.SetActive(false);
    }
    public void ManagerNextSentence()
    {
        manager.NextSentence();
    }

}
