using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Yarn.Unity;

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

        
    }

    //public GameObject droppedItem;
    public GameObject damageIndicator;
    public Item experienceGem;

    public Player player;
    

    [SerializeField] private WorldTime worldTime;

    public AudioClip defaultPickupSound;

    public Material defaultLitSprite;

    public GameObject playerPrefab;

    public Grid grid;
    public Tilemap tilemap;

}
