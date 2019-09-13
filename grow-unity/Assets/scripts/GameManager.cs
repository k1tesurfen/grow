using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public Scatter scatter;
    public GameObject hand;

    public GameObject snowball;

    //poisson disk spawning
    public float radius = 1;
    public Vector2 regionSize = new Vector2(15, 15);
    List<Vector2> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = PoissonDiskSpawn.GenerateSpawns(radius, regionSize);
        foreach (Vector2 point in spawnPoints)
        {
            Instantiate(snowball, new Vector3(-7 + point.x, 0.2f, -3 + point.y), Quaternion.identity);
        }
    }
}
