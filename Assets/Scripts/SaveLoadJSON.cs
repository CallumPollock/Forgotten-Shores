using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using Unity.VisualScripting;
using OdinSerializer;

[System.Serializable]
[SerializeField]
public class WorldData
{
    public EntityData player;
    public List<string> unlockedRecipes;
    public EntityData[] entites;
    public List<String> objectives;
    public long ticks;
}

public class SaveLoadJSON : MonoBehaviour
{

    public WorldData worldData;
    public static Action<WorldData> worldLoaded;

    [SerializeField] private WorldData newSaveData;

    string saveFilePath;

    //public static Action<EntityData> LoadedPlayer;
    //public static Action<List<String>, List<String>> LoadedObjectives;



    [SerializeField] VariableStorageBehaviour variableStorage;
    [SerializeField] Objective startingObjective;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/player.data";
        LoadGame();

        Player.OnPlayerSpawn += OnPlayerRespawn;

        //variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        
    }

    void OnPlayerRespawn(Entity entity)
    {
        worldData.player = entity.data;
    }

    public void CreateGame(string playerName)
    {
        worldData = newSaveData;
        worldData.player.name = playerName;

        string saveWorldData = JsonUtility.ToJson(worldData);
        File.WriteAllText(saveFilePath, saveWorldData);
        Debug.Log("Created game at " + saveFilePath);
    }

    public void SaveGame()
    {
        worldData.objectives.Clear();
        worldData.unlockedRecipes.Clear();
        foreach(Objective objective in ObjectiveManager.objectives)
        {
            worldData.objectives.Add(objective.name);
        }
        foreach (Item item in CraftMenuManager.recipeBook)
        {
            worldData.unlockedRecipes.Add(item.name);
        }

        worldData.ticks = FindAnyObjectByType<WorldTime>().GetTicks();

        string saveWorldData = JsonUtility.ToJson(worldData);
        File.WriteAllText(saveFilePath, saveWorldData);
        //byte[] bytes = SerializationUtility.SerializeValue(worldData, DataFormat.Binary);
        //File.WriteAllBytes(saveFilePath, bytes);
        Debug.Log("Saved to " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadWorldData = File.ReadAllText(saveFilePath);
            worldData = JsonUtility.FromJson<WorldData>(loadWorldData);

            //byte[] bytes = File.ReadAllBytes(saveFilePath);
            //worldData = SerializationUtility.DeserializeValue<WorldData>(bytes, DataFormat.Binary);

            worldLoaded?.Invoke(worldData);
            //LoadedObjectives?.Invoke(worldData.objectives, worldData.completedObjectives);
            Debug.Log("Loaded " + saveFilePath);

            variableStorage.SetValue("$name", worldData.player.name);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
