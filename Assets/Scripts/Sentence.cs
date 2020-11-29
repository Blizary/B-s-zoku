using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sentence
{
    public string personSpeaking;
    [TextArea(3, 50), SerializeField]
    public string textToShow = " ";
}
