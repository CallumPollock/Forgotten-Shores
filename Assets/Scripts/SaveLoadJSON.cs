using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using Unity.VisualScripting;
using OdinSerializer;
using UnityEngine.SceneManagement;

[System.Serializable]
[SerializeField]
public class WorldData
{
    public Vector2 respawnPos;
    public List<string> unlockedRecipes;
    public EntityData[] entites;
    public List<String> objectives;
    public long ticks;
}

public class SaveLoadJSON : MonoBehaviour
{

    public WorldData worldData;
    public EntityData playerData;
    public static Action<WorldData> worldLoaded;
    public static Action<EntityData> playerLoaded;

    //public static Action<EntityData> LoadedPlayer;
    //public static Action<List<String>, List<String>> LoadedObjectives;

    [SerializeField] CraftMenuManager craftMenuManager;

    [SerializeField] VariableStorageBehaviour variableStorage;
    [SerializeField] Objective startingObjective;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Game") return;

        LoadGame();

        Player.OnPlayerSpawn += _ => LoadPlayer();
        Player.OnPlayerDied += SavePlayer;
        Player.OnPlayerDied += _ => ResetSpawnOnDeath();

        //variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        
    }

    private void ResetSpawnOnDeath()
    {
        playerData.worldPosition = worldData.respawnPos;
    }

    private void SavePlayer(Entity entity)
    {
        playerData = entity.data;

        SaveData(playerData, "player");
    }

    public void SaveGame()
    {
        worldData.objectives.Clear();
        worldData.unlockedRecipes.Clear();
        foreach(Objective objective in ObjectiveManager.objectives)
        {
            worldData.objectives.Add(objective.name);
        }
        foreach (Item item in craftMenuManager.recipeBook)
        {
            worldData.unlockedRecipes.Add(item.name);
        }

        worldData.ticks = FindAnyObjectByType<WorldTime>().GetTicks();

        SaveData(worldData, "world");
        SaveData(playerData, "player");
        //byte[] bytes = SerializationUtility.SerializeValue(worldData, DataFormat.Binary);
        //File.WriteAllBytes(saveFilePath, bytes);
        //Debug.Log("Saved to " + saveFilePath);
    }

    public void SaveData(object data, string fileName)
    {
        string saveData = JsonUtility.ToJson(data);
        File.WriteAllText(String.Format("{0}/{1}.data", Application.persistentDataPath, fileName), saveData);
    }

    private void LoadWorld()
    {
        string filePath = String.Format("{0}/{1}.data", Application.persistentDataPath, "world");

        if (File.Exists(filePath))
        {
            string loadWorldData = File.ReadAllText(filePath);
            worldData = JsonUtility.FromJson<WorldData>(loadWorldData);

            //byte[] bytes = File.ReadAllBytes(saveFilePath);
            //worldData = SerializationUtility.DeserializeValue<WorldData>(bytes, DataFormat.Binary);

            worldLoaded?.Invoke(worldData);
            //LoadedObjectives?.Invoke(worldData.objectives, worldData.completedObjectives);
            Debug.Log("Loaded " + filePath);
        }
    }

    private void LoadPlayer()
    {
        string filePath = String.Format("{0}/{1}.data", Application.persistentDataPath, "player");

        if (File.Exists(filePath))
        {
            string loadPlayerData = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<EntityData>(loadPlayerData);

            //byte[] bytes = File.ReadAllBytes(saveFilePath);
            //worldData = SerializationUtility.DeserializeValue<WorldData>(bytes, DataFormat.Binary);

            playerLoaded?.Invoke(playerData);
            //LoadedObjectives?.Invoke(worldData.objectives, worldData.completedObjectives);
            Debug.Log("Loaded " + filePath);

            variableStorage.SetValue("$name", playerData.name);
        }
    }

    public void LoadGame()
    {
        LoadWorld();
        LoadPlayer();
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            SaveGame();
        }
        
    }
}
