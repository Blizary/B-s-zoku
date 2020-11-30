using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    // Stuff to hide/show with the information button
    [SerializeField] private Text _information;

    public void StartGame()
    {
        SceneManager.LoadScene("1_MainGame");
        PlayerPrefs.SetInt("LastWave", 0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit request initiated - Only works in builds");
        Application.Quit();
    }

    public void ShowInformation()
    {
        // Show Information Stuff
        _information.gameObject.SetActive(!_information.gameObject.activeSelf);
    }
}
