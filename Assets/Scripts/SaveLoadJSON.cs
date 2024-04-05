using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using Unity.VisualScripting;

[System.Serializable]
[SerializeField]
public class WorldData
{
    public EntityData player;
    public EntityData[] entites;
    public List<String> objectives, completedObjectives;
}

public class SaveLoadJSON : MonoBehaviour
{

    public static WorldData worldData;
    string saveFilePath;

    public static Action<EntityData> LoadedPlayer;
    public static Action<List<String>, List<String>> LoadedObjectives;

    [SerializeField] VariableStorageBehaviour variableStorage;
    [SerializeField] Objective startingObjective;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/player.data";
        LoadGame();

        //variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        
    }

    public void CreateGame(string playerName)
    {
        worldData = new WorldData();
        worldData.player = new EntityData();
        worldData.player.name = playerName;
        worldData.player.health = 100;
        worldData.player.maxHealth = 100;
        worldData.player.damage = 1;
        worldData.player.speed = 8;

        worldData.objectives = new List<String>();
        worldData.objectives.Add("Talk to Trevor");

        SaveGame();
    }

    public void SaveGame()
    {
        string saveWorldData = JsonUtility.ToJson(worldData);
        File.WriteAllText(saveFilePath, saveWorldData);
        Debug.Log("Saved to " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadWorldData = File.ReadAllText(saveFilePath);
            worldData = JsonUtility.FromJson<WorldData>(loadWorldData);
            LoadedPlayer?.Invoke(worldData.player);
            LoadedObjectives?.Invoke(worldData.objectives, worldData.completedObjectives);
            Debug.Log("Loaded " + saveFilePath);

            variableStorage.SetValue("$name", worldData.player.name);
        }
    }
}
