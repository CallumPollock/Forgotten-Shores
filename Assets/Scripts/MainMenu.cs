using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button loadGame;

    [SerializeField] TextMeshProUGUI continueText;
    [SerializeField] TextMeshProUGUI fullscreenText;

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/player.data") && File.Exists(Application.persistentDataPath + "/world.data"))
        {
            loadGame.interactable = true;
            string loadEntityData = File.ReadAllText(Application.persistentDataPath + "/player.data");
            EntityData playerData = JsonUtility.FromJson<EntityData>(loadEntityData);

            string loadWorldData = File.ReadAllText(Application.persistentDataPath + "/world.data");
            WorldData worldData = JsonUtility.FromJson<WorldData>(loadWorldData);

            continueText.text += string.Format("<br><size=12>{0} (LVL {1}) - Day {2}</size>", playerData.name, playerData.level, TimeSpan.FromTicks(worldData.ticks).Days).ToUpper();
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

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if(!Screen.fullScreen)
        {
            fullscreenText.text = "WINDOWED";
        }
        else
        {
            fullscreenText.text = "FULLSCREEN";
        }
    }

}
