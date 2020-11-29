using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainLevelManager : MonoBehaviour
{

    public GameObject instructionsUI;
    public GameObject deathUI;
    public TextAnimatorPlayer playerText;
    public GameObject enemyText;
    public GameObject conversationBlackBars;
    public GameObject blackScreen;
    public TextAnimatorPlayer blackScreenText;
    public Text pausedText;

    public List<BsZokuEvent> levelEvents;

    private GameObject spawner;
    private BsZokuEvent currentEvent;

    //wave event variables
    private bool wavesOn;
    private int enemiesKilledInWave;
    private int enemiesInWave;
    private bool enemiesSpawned;

    //conversaton event variables
    private bool conversationOn;
    private bool sentenceShown;

    //blackScreen event variables
    private bool blackScreenOn;

    private bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawners");
        UpdateEvent();
    }

    // Update is called once per frame
    void Update()
    {
        WaveController();
        ConversationController();
    }


    void UpdateEvent()
    {
        if (levelEvents.Count!=0)
        {
            currentEvent = levelEvents[0];
            switch (currentEvent.type)
            {
                case BsZokuEvent.BsZokuEventType.BlackScreen:
                    //STOP PLAYER MOVEMENT HERE
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject p in players)
                    {
                        p.GetComponent<PlayerController>().frozen = true;
                    }
                    blackScreenOn = true;
                    blackScreen.SetActive(true);
                    blackScreenText.ShowText(currentEvent.textToShow);
                    break;
                case BsZokuEvent.BsZokuEventType.Conversation:
                    //STOP PLAYER MOVEMNT HERE
                    GameObject[] gamers = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject p in gamers)
                    {
                        p.GetComponent<PlayerController>().frozen = true;
                    }
                    if (currentEvent.conversation.cinematicView)
                    {
                        conversationBlackBars.SetActive(true);
                    }
                    conversationOn = true;
                    break;
                case BsZokuEvent.BsZokuEventType.Waves:
                    wavesOn = true;
                    enemiesKilledInWave = 0;
                    break;

            }

        }
        else
        {
            Debug.Log("END OF SET EVENTS");
        }
        
    }


    void WaveController()
    {
        if(wavesOn)
        {
            if(currentEvent.waves.Count>0)
            {
                if(!enemiesSpawned)
                {
                    enemiesInWave = 0;
                    for (int i = 0; i < currentEvent.waves[0].enemies.Count; i++)
                    {
                        for (int j = 0; j < currentEvent.waves[0].enemies[i].amount; j++)
                        {
                            enemiesInWave += 1;
                        }
                    }

                    if (currentEvent.waves[0].allAtOnce)//Spawn enemies all at once
                    {
                        for(int i = 0;i<currentEvent.waves[0].enemies.Count;i++)
                        {
                            for(int j= 0;j<currentEvent.waves[0].enemies[i].amount;j++)
                            {
                                int randSpawn = Random.Range(0, spawner.transform.childCount);
                                spawner.transform.GetChild(randSpawn).GetComponent<EnemySpawner>().Spawn(currentEvent.waves[0].enemies[i].enemyType);
                            }
                        }
                    }
                    else// allow for timers for enemies
                    {
                        for (int i = 0; i < currentEvent.waves[0].enemies.Count; i++)
                        {
                            int randSpawn = Random.Range(0, spawner.transform.childCount);
                            spawner.transform.GetChild(randSpawn).GetComponent<EnemySpawner>().UpdateEnemy(currentEvent.waves[0].enemies[i].enemyType, currentEvent.waves[0].enemies[i].amount);
                        }

                    }
                    enemiesSpawned = true;
                    Debug.Log("spawned enemies for this wave");
                    Debug.Log("current enemies on this wave: " + enemiesInWave);
                }
                else
                {
                    if (enemiesKilledInWave>=enemiesInWave)//all enemies of this are killed
                    {
                        currentEvent.waves.RemoveAt(0);
                        enemiesKilledInWave = 0;
                        enemiesSpawned = false;
                        Debug.Log("all enemies killed for this wave");
                    }
                }
                
            }
            else // all enemies of this event are killed
            {
                levelEvents.RemoveAt(0);
                wavesOn = false;
                Debug.Log("wave event complete");
                UpdateEvent();
            }

        }
    }

    void ConversationController()
    {
        if(conversationOn)
        {
            if(currentEvent.conversation.sentences.Count>0)
            {
                if(!sentenceShown)
                {
                    switch(currentEvent.conversation.sentences[0].personSpeaking)
                    {
                        case "Player":
                            playerText.gameObject.transform.parent.gameObject.SetActive(true);
                            playerText.ShowText(currentEvent.conversation.sentences[0].textToShow);
                            break;
                        case "Boyfriend":
                            enemyText = GameObject.FindGameObjectWithTag("Boyfriend");
                            enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            break;
                        case "Mother":
                            enemyText = GameObject.FindGameObjectWithTag("Mother");
                            enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            break;
                        case "Tutorial":
                            enemyText = GameObject.FindGameObjectWithTag("Tutorial");
                            enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            break;
                    }
                    sentenceShown = true;
                }
            }
            else //conversation finished
            {
                levelEvents.RemoveAt(0);
                sentenceShown = false;
                conversationOn = false;
                conversationBlackBars.SetActive(false);
                Debug.Log("conversation event complete");
                UpdateEvent();
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("1_MainGame");
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gamePaused)
            {
                Time.timeScale = 1f;
                gamePaused = false;
                pausedText.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                gamePaused = true;
                pausedText.gameObject.SetActive(true);
            }
        } 
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

    public void UpdateEnemiesKilled()
    {
        enemiesKilledInWave += 1;
    }


    public void OnBlackScreenTextShown()
    {

        blackScreen.SetActive(false);
        blackScreenOn = false;
        //GIVE PLAYER CONTROLLS BACK
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerController>().frozen = false;
        }
        levelEvents.RemoveAt(0);
        Debug.Log("blackscreen event complete");
        UpdateEvent();

    }

    public void NextSentence()
    {
        playerText.gameObject.transform.parent.gameObject.SetActive(false);
        if(enemyText!=null)
        {
            enemyText.GetComponent<TextLocations>().SetAsleepDisplay();
        }

        sentenceShown = false;
        currentEvent.conversation.sentences.RemoveAt(0);
    }

}

