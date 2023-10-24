using UnityEngine;

/// <summary>
/// Circle spawner class 
/// spawns random amount of circles 
/// </summary>
public class CircleSpawner : MonoBehaviour
{
    [Header("Circle prefab Settings")]
    [SerializeField] private Circle circlePrefab;
    [SerializeField] private int minCircles = 5;
    [SerializeField] private int maxCircles = 10;

    [Space]
    [Header("Spawning Area Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;


    private float circleRadius; // Define the radius for circles

    private void Start()
    {
        // Calculate the radius based on the circle's size
        circleRadius = circlePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        SpawnRandomCircles();
    }

    //spawn given amount of circles
    private void SpawnRandomCircles()
    {
        int circleCount = Random.Range(minCircles, maxCircles + 1);

        for (int i = 0; i < circleCount; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            if (!IsOverlapping(spawnPosition))
            {
                Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    //random position for the circles
    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    //check if the circles are colliding each other
    private bool IsOverlapping(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, circleRadius);
        return colliders.Length > 0;
    }
}
