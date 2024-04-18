using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombie;
    Player player;
    [SerializeField] Camera cam;

    private void Start()
    {
        Player.OnPlayerSpawn += UpdatePlayer;
        WorldTime.OnDayBegin += ToggleZombieSpawn;
    }

    private void UpdatePlayer(Player _player) { player = _player; }

    private void ToggleZombieSpawn(bool isDay)
    {
        if(isDay)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(SpawnTimer());
        }
    }

    IEnumerator SpawnTimer()
    {
        //Vector2 newPos = cam.ViewportToWorldPoint(Vector3.left);

        EntityData newZombie = new EntityData();
        newZombie.entitySize = new Vector2(Random.Range(0.2f, 1.5f), 1f);
        
        newZombie.maxHealth = 100;
        newZombie.damage = 5;
        newZombie.worldPosition = new Vector2(player.transform.position.x, player.transform.position.y) + Random.insideUnitCircle * 30f;
        newZombie.level = 1;
        if(player != null)
        {
            newZombie.level = Mathf.Max(Random.Range(player.data.level - 2, player.data.level + 3), 1);
            
        }
        newZombie.damage += newZombie.level;
        newZombie.maxHealth += newZombie.level*10;

        newZombie.health = newZombie.maxHealth;

        SpawnZombie(newZombie);

        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnTimer());
    }

    void SpawnZombie(EntityData _entityData)
    {
        Zombie newZombie = Instantiate(zombie).GetComponent<Zombie>();
        newZombie.LoadEntityData(_entityData);
    }
}
