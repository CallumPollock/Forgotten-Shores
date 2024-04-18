using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] SpriteRenderer trousers;
    private SaveLoadJSON saveLoadJSON;
    EntityData playerData;
    WorldData worldData;

    AudioSource audioSource;
    [SerializeField] AudioClip[] voiceClips;

    private void Start()
    {
        saveLoadJSON = GetComponent<SaveLoadJSON>();
        playerData = new EntityData();
        playerData.entitySize = new Vector2(0.8f, 1f);
        playerData.health = 100;
        playerData.maxHealth = 100;
        playerData.damage = 1;
        playerData.speed = 8;
        playerData.experienceToNextLevel = 10;
        playerData.voicePitch = 1f;
        playerData.color = new Color(0.3647059f, 0.3960785f, 0.9568628f);
        playerData.worldPosition = new Vector2(-47.1f, -72.89f);

        worldData = new WorldData();
        worldData.objectives = new List<string>
        {
            "Talk to Trevor"
        };
        worldData.ticks = 288180000000;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void NameFieldChange(string name)
    {
        playerData.name = name;
        Debug.Log("Player name: " + name);
    }

    public void SetFavColour(Color color)
    {
        playerData.color = color;
        trousers.color = color;
    }

    public void BodySizeSlider(float value)
    {
        body.localScale = new Vector3(value, body.localScale.y);
        playerData.entitySize.x = value;
    }

    public void HeightSlider(float value)
    {
        body.localScale = new Vector3(body.localScale.x, value);
        playerData.entitySize.y = value;
    }

    public void VoicePitch(float value)
    {
        audioSource.pitch = value;
        audioSource.PlayOneShot(voiceClips[Random.Range(0, voiceClips.Length)]);
        playerData.voicePitch = value;
    }

    public void ConfirmCharacter()
    {
        saveLoadJSON.SaveData(playerData, "player");
        saveLoadJSON.SaveData(worldData, "world");
        SceneManager.LoadScene("Game");
    }
}
