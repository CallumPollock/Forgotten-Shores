using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        worldTime = GetComponent<WorldTime>();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    //public GameObject droppedItem;
    public GameObject damageIndicator;
    public Item experienceGem;

    public Player player;
    [SerializeField] GameObject enemy;

    [SerializeField] private WorldTime worldTime;

    IEnumerator SpawnEnemy()
    {
        if (worldTime.GetCurrentTime().Hours >= 22 || worldTime.GetCurrentTime().Hours <= 6)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.position = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle * 20f;
        }
            
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemy());
    }
}
