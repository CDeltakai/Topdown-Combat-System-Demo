using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] float spawnCooldown = 3;
    float currentSpawnTimer = 3;

    public float minDistanceFromPlayer = 10f;
    public float maxDistanceFromPlayer = 20f;

    public bool isActive = true;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive){ return; }
        currentSpawnTimer -= Time.deltaTime;
        if(currentSpawnTimer <= 0)
        {
            currentSpawnTimer = spawnCooldown;
            SpawnEnemy();
        }
    }


    void SpawnEnemy()
    {
        Vector3 spawnPosition = RandomPositionAroundPlayer(minDistanceFromPlayer, maxDistanceFromPlayer);
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.player = player.transform;
    }


    Vector3 RandomPositionAroundPlayer(float minDistance, float maxDistance)
    {
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the spawn distance
        float spawnDistance = Random.Range(minDistance, maxDistance);

        // Convert angle and distance to a position
        float x = player.position.x + spawnDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float z = player.position.z + spawnDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        return new Vector3(x, player.position.y + 2, z);        
    }

    public void ToggleActive()
    {
        if(isActive)
        {
            isActive = false;
        }else
        {
            isActive = true;
        }
    }



}
