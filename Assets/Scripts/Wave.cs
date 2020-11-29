using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Wave
{
    public List<WaveEnemy> enemies;
    public bool allAtOnce;
}
