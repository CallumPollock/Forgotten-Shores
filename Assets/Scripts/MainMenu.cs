using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button loadGame;

    private void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/player.data"))
        {
            loadGame.interactable = true;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("CutScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
