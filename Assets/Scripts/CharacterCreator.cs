using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] Slider bodySlider, heightSlider;
    [SerializeField] Transform body;
    [SerializeField] TMP_InputField nameField;
    private SaveLoadJSON saveLoadJSON;

    private void Start()
    {
        saveLoadJSON = GetComponent<SaveLoadJSON>();
    }

    public void BodySizeSlider()
    {
        body.localScale = new Vector3(bodySlider.value, body.localScale.y);
    }

    public void HeightSlider()
    {
        body.localScale = new Vector3(body.localScale.x, heightSlider.value);
    }

    public void ConfirmCharacter()
    {
        saveLoadJSON.CreateGame(nameField.text);
        SceneManager.LoadScene("Game");
    }
}
