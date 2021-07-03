using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public int rocksAmount;
    public GameObject rockPrefab;
    public int twisterAmount;
    public GameObject twisterPrefab;
    public int sandPitsAmount;
    public GameObject sanPitPrefab;
    public BoxCollider2D myCollider;
    public BoxCollider2D playerSpawnPoint;
    public Camera myCamera;

    private List<Bounds> ObstaclesBounds;

    // Start is called before the first frame update
    void Start()
    {

        int i = 0;
        ObstaclesBounds = new List<Bounds>();

        ObstaclesBounds.Add(playerSpawnPoint.bounds);

        for (i = 0; i < rocksAmount; i++)
        {
            GameObject rock = Instantiate(rockPrefab, RandomPointInBounds(myCollider.bounds, ObstaclesBounds), Quaternion.identity);
            if (DestroyIfOverlapping(rock.GetComponent<BoxCollider2D>().bounds, ObstaclesBounds))
                Destroy(rock);
            else
                ObstaclesBounds.Add(rock.GetComponent<BoxCollider2D>().bounds);

        }

        for (i = 0; i < twisterAmount; i++)
        {
            GameObject twister = Instantiate(twisterPrefab, RandomPointInBounds(myCollider.bounds, ObstaclesBounds), Quaternion.identity);
            if (DestroyIfOverlapping(twister.GetComponent<CircleCollider2D>().bounds, ObstaclesBounds))
                Destroy(twister);
            else
                ObstaclesBounds.Add(twister.GetComponent<CircleCollider2D>().bounds);
        }

        for (i = 0; i < sandPitsAmount; i++)
        {
            GameObject sandPit = Instantiate(sanPitPrefab, RandomPointInBounds(myCollider.bounds, ObstaclesBounds), Quaternion.identity);
            if (DestroyIfOverlapping(sandPit.GetComponent<BoxCollider2D>().bounds, ObstaclesBounds))
                Destroy(sandPit);
            else
                ObstaclesBounds.Add(sandPit.GetComponent<BoxCollider2D>().bounds);
        }

    }

    private bool DestroyIfOverlapping(Bounds Obstacle, List<Bounds> obstaclesBounds)
    {
        foreach (Bounds obsBound in obstaclesBounds)
        {
            if (Obstacle.Intersects(obsBound))
                return true;
        }
        return false;
    }

    public static Vector3 RandomPointInBounds(Bounds boundsSpawn, List<Bounds> Obstacles)
    {
        Vector3 point;
        do
        {
            point = new Vector3(
                Random.Range(boundsSpawn.min.x, boundsSpawn.max.x),
                Random.Range(boundsSpawn.min.y, boundsSpawn.max.y),
                0
            );
        }
        while (CheckBounds(point, Obstacles)); //check not in player/another obstacle area 


        return point;

    }

    private static bool CheckBounds(Vector3 point, List<Bounds> obstacles)
    {
        foreach (Bounds obstacleBound in obstacles)
        {
            if (obstacleBound.Contains(point))
                return true;
        }

        return false;
    }
}
