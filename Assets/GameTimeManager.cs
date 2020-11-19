using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    private float gameTime;
    private bool gameActive = true;

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            GetComponent<Text>().text = "Time: " + Mathf.Round(gameTime).ToString();
        }
    }

    public void StopTimer()
    {
        gameActive = false;
    }
}
