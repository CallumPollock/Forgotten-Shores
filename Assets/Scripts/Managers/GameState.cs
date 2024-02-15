using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public static GameState instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    //public GameObject droppedItem;
    public GameObject damageIndicator;
    public GameObject experienceGem;
    float gameTime = 0f;
    float gameSpeed = 0.001f;
    public Image darknessOverlay;

    public TextMeshProUGUI timeUIText;

    public Player player;
    [SerializeField] GameObject enemy;

    private void Update()
    {
        gameTime += Time.deltaTime * gameSpeed;
        if (gameTime >= 0.9f) gameTime = 0f;
        darknessOverlay.color = new Color(0f,0f,0f,gameTime);

        timeUIText.text = Mathf.Round((gameTime * 23) + 1).ToString();
    }


    IEnumerator SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle * 20f;
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemy());
    }
}
