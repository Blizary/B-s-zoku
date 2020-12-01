using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainLevelManager : MonoBehaviour
{
    public int TESTEVENT;

    public AudioSource audioPlayer;
    public GameObject instructionsUI;
    public GameObject deathUI;
    public TextAnimatorPlayer playerText;
    public GameObject enemyText;
    public GameObject conversationBlackBars;
    public GameObject blackScreen;
    public TextAnimatorPlayer blackScreenText;
    public Text pausedText;
    public GameObject background;
    public GameObject textBackground;
    public GameObject dayBackground;
    public GameObject eveningBackground;
    public GameObject nightBackground;
    public GameObject endButton;

    public List<BsZokuEvent> levelEvents;

    [Header("Prefabs")]
    public GameObject gangstaPoint;
    public GameObject gangstaBoss;
    public GameObject boyfriendPoint;
    public GameObject boyfriendBoss;
    public GameObject momPoint;
    public GameObject momBoss;


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
    public int eventCounter;

    private void Awake()
    {
        Debug.Log("manager awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawners");
        if (PlayerPrefs.HasKey("LastWave"))
        {
            eventCounter = PlayerPrefs.GetInt("LastWave");
            if(eventCounter>3)
            {
                eventCounter -= 2;
            }

            if( TESTEVENT!=0)
            {
                eventCounter = TESTEVENT-1;
            }
            for (int i=0;i< eventCounter; i++)
            {
                levelEvents.RemoveAt(0);
               
            }
            Debug.Log("key found");
            Debug.Log("num of events in store: " + levelEvents.Count);
        }
        else
        {
            Debug.Log("Non Existing key");
            eventCounter = 0;
        }


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
            Debug.Log("still has events");
            currentEvent = levelEvents[0];
            if (currentEvent.newMusic != null)
            {
                if(audioPlayer.clip!=currentEvent.newMusic)
                {
                    audioPlayer.clip = currentEvent.newMusic;
                    audioPlayer.Play();
                }
                
            }
            if (currentEvent.newBackground != null)
            {
                dayBackground.SetActive(false);
                eveningBackground.SetActive(false);
                nightBackground.SetActive(false);
                currentEvent.newBackground.SetActive(true);
            }

            switch (currentEvent.type)
            {
                case BsZokuEvent.BsZokuEventType.BlackScreen:
                    //STOP PLAYER MOVEMENT HERE
                    //Stop the audio
                    audioPlayer.Stop();
                    
                    if(GameObject.FindGameObjectWithTag("Player"))
                    {
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                        foreach (GameObject p in players)
                        {
                            if(p.GetComponent<PlayerController>())
                            {
                                p.GetComponent<PlayerController>().SetFreezeState(true);
                            }
                           
                        }
                    }
                   
                    blackScreenOn = true;
                    blackScreen.SetActive(true);
                    textBackground.SetActive(true);
                    blackScreenText.ShowText(currentEvent.textToShow);
                    break;
                case BsZokuEvent.BsZokuEventType.Conversation:
                    //STOP PLAYER MOVEMNT HERE
                    //check blackscreen
                    if(blackScreen.activeInHierarchy)
                    {
                        StartCoroutine(BlackScreenFade());
                    }

                    if (GameObject.FindGameObjectWithTag("Player"))
                    {
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                        foreach (GameObject p in players)
                        {
                            p.GetComponent<PlayerController>().SetFreezeState(true);
                        }
                    }
                    if (currentEvent.conversation.cinematicView)
                    {
                        conversationBlackBars.SetActive(true);
                    }
                    conversationOn = true;
                    break;
                case BsZokuEvent.BsZokuEventType.Waves:
                    //check blackscreen
                    if (blackScreen.activeInHierarchy)
                    {
                        StartCoroutine(BlackScreenFade());
                    }
                    wavesOn = true;
                    enemiesKilledInWave = 0;
                    break;

            }
            
            eventCounter += 1;

        }
        else
        {
            Debug.Log("END OF SET EVENTS");
            endButton.SetActive(true);
        }
        
    }

    IEnumerator BlackScreenFade()
    {
        textBackground.SetActive(false);
        blackScreen.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        blackScreen.SetActive(false);
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
                                if (currentEvent.spawnlocation == null)
                                {
                                    int randSpawn = Random.Range(0, spawner.transform.childCount);
                                    spawner.transform.GetChild(randSpawn).GetComponent<EnemySpawner>().Spawn(currentEvent.waves[0].enemies[i].enemyType);
                                }
                                else
                                {
                                    Debug.Log("spawn at location");
                                    GameObject newMob = Instantiate(currentEvent.waves[0].enemies[i].enemyType, currentEvent.spawnlocation.transform);
                                }
                                 
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
                }
                else
                {
                    if (enemiesKilledInWave>=enemiesInWave)//all enemies of this are killed
                    {
                        currentEvent.waves.RemoveAt(0);
                        enemiesKilledInWave = 0;
                        enemiesSpawned = false;
                    }
                }
                
            }
            else // all enemies of this event are killed
            {
                wavesOn = false;
                StartCoroutine(WaitAfterAllKilled());
            }

        }
    }

    IEnumerator WaitAfterAllKilled()
    {
        yield return new WaitForSeconds(1);
        levelEvents.RemoveAt(0);
        Debug.Log("wave event complete");
        UpdateEvent();
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
                            if (GameObject.FindGameObjectWithTag("Boyfriend"))
                            {
                                enemyText = GameObject.FindGameObjectWithTag("Boyfriend");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            }
                            else
                            {
                                GameObject newGangsta = Instantiate(boyfriendBoss, boyfriendPoint.transform);
                                enemyText = GameObject.FindGameObjectWithTag("Boyfriend");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            }

                            break;
                        case "Mother":
                            if (GameObject.FindGameObjectWithTag("Mother"))
                            {
                                enemyText = GameObject.FindGameObjectWithTag("Mother");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            }
                            else
                            {
                                GameObject newGangsta = Instantiate(momBoss, momPoint.transform);
                                enemyText = GameObject.FindGameObjectWithTag("Mother");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            }

                            break;
                        case "Tutorial":
                            enemyText = GameObject.FindGameObjectWithTag("Tutorial");
                            enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            break;
                        case "Other":
                            if (GameObject.FindGameObjectWithTag("Other"))
                            {
                                enemyText = GameObject.FindGameObjectWithTag("Other");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                            }
                            else
                            {
                                GameObject newGangsta = Instantiate(gangstaBoss, gangstaPoint.transform);
                                enemyText = GameObject.FindGameObjectWithTag("Other");
                                enemyText.GetComponent<TextLocations>().DisplayText(currentEvent.conversation.sentences[0].textToShow);
                                Debug.Log("thingy");
                            }
                            
                            
                            break;
                    }
                    sentenceShown = true;
                }
            }
            else //conversation finished
            {
                conversationOn = false;
                StartCoroutine(WaitAfterAllConvo());
            }
        }
    }
    IEnumerator WaitAfterAllConvo()
    {
        yield return new WaitForSeconds(1);
        playerText.gameObject.transform.parent.gameObject.SetActive(false);
        if (enemyText != null)
        {
            enemyText.GetComponent<TextLocations>().SetAsleepDisplay();
            enemyText.GetComponent<TextLocations>().DisableVisuals();
        }
        levelEvents.RemoveAt(0);
        
        sentenceShown = false;
        conversationBlackBars.SetActive(false);
        //GIVE PLAYER CONTROLLS BACK
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerController>().SetFreezeState(false);
            p.GetComponent<PlayerController>().RemoveControls(false);
        }
        Debug.Log("conversation event complete");
        UpdateEvent();
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
       // GameObject.Find("Timer").GetComponent<GameTimeManager>().StopTimer();
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

        //blackScreen.SetActive(false);
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
        if (enemyText != null)
        {
            enemyText.GetComponent<TextLocations>().SetAsleepDisplay();
        }
        sentenceShown = false;
        currentEvent.conversation.sentences.RemoveAt(0);
    }

    public void SetLastEvent()
    {
        PlayerPrefs.SetInt("LastWave", eventCounter);
        Debug.Log("KEY WAS SET");
    }

    public void QuitGame()
    {
        Debug.Log("Quit request initiated - Only works in builds");
        Application.Quit();
    }

}

