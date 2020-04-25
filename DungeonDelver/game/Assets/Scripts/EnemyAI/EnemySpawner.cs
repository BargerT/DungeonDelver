using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();
    
    private float spawnTimer = 0f;
    private float waitTimer = 7f;
    private int spawned = 0;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(Vector3.Distance(player.transform.position, transform.position) < 30 && spawned < 2 && spawnTimer >= waitTimer)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Count - 1)]);
            enemy.transform.position = transform.position;
            spawned++;
            spawnTimer = 0f;
        }
    }
}
