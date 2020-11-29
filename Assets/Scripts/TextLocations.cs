using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLocations : MonoBehaviour
{
    public TextAnimatorPlayer textDisplay;
    public GameObject textBuble;

    public void DisplayText(string _text)
    {
        textBuble.SetActive(true);
        textDisplay.ShowText(_text);
    }

    public void SetAsleepDisplay()
    {
        textBuble.SetActive(false);
    }

}
