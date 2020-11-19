using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelManager : MonoBehaviour
{

    public GameObject instructionsUI;
    public GameObject deathUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("1_MainGame");
    }

    public void DeathScreen()
    {
        instructionsUI.SetActive(false);
        deathUI.SetActive(true);
        GameObject.Find("Timer").GetComponent<GameTimeManager>().StopTimer();
        foreach (Transform child in GameObject.Find("SpawnerList").transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

