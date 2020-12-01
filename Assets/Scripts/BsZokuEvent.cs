using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class BsZokuEvent
{
    public string name;
    public AudioClip newMusic;
    public GameObject newBackground;
    public enum BsZokuEventType
    {
        BlackScreen,
        Conversation,
        Waves
    }

    public BsZokuEventType type;

    [Header("FOR BLACKSCREEN")]
  
    [TextArea(3, 50), SerializeField]
    public string textToShow = " ";

    [Header("FOR CONVERSATIONS")]
    public Conversation conversation;
    

    [Header("FOR WAVES")]
    public List<Wave> waves;
    public GameObject spawnlocation;


}
