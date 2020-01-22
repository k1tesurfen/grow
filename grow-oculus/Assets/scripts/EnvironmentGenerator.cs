using System.Linq;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    //the terrain mesh
    private Mesh mesh;
    //all vertices in the terrain mesh
    private Vector3[] vertices;
    //all normals in the terrain mesh
    private Vector3[] normals;
    //array with tree prefabs
    public GameObject[] trees;
    //array with rock prefabs
    public GameObject[] rocks;
    //gameobject that hold all seeded trees
    private GameObject treeObject;
    //gameobject that hold all placed rocks
    private GameObject rockObject;
    //scale of the perlin noise texture
    public float perlinRate = 0.0008f;

    //instantiates trees depending on the height profile of a terrain mesh 
    //trees get generated as child of treeObject
    public void GenerateEnvironment()
    {
        treeObject = new GameObject("trees");
        treeObject.transform.parent = transform;

        rockObject = new GameObject("rocks");
        rockObject.transform.parent = transform;

        mesh = GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices.Distinct().ToArray();

        //place trees
        foreach (Vector3 ver in vertices)
        {
            //the mesh model is sunken into the ground. -4.2f 
            if (ver.y > -4.2f && ver.y < 25f && ver.magnitude < 290f && Random.Range(0, 2) > 0)
            {
                if (Mathf.PerlinNoise(ver.x * perlinRate, ver.y * perlinRate) > 0.38f)
                {
                    //instantiate random tree from trees array. 
                    Instantiate(trees[Random.Range(0, trees.Length)],
                        (ApplyTransform(ver, transform) + new Vector3(Random.Range(-2f, 2f), Random.Range(-0.2f, -0.08f), Random.Range(-2f, 2f))),
                        Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0)), treeObject.transform);
                }
            }
        }
        
        //place rocks
        foreach (Vector3 ver in vertices)
        {
            if (ver.y > -4.3f && ver.y < 20f && ver.magnitude < 200f && Random.Range(0, 40) == 0)
            {
                if (Mathf.PerlinNoise(ver.x * perlinRate, ver.y * perlinRate) > 0.45f)
                {
                    //instantiate random stones from stones array
                    Instantiate(rocks[Random.Range(0, rocks.Length)],
                        (ApplyTransform(ver, transform) + new Vector3(Random.Range(-2f, 2f) + 1f, Random.Range(0f, 0.7f), Random.Range(-2f, 2f) - 1f)),
                        Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0)), rockObject.transform);
                }
            }
        }
    }
    
    //apply transform of terrain mesh gameobject onto tree objects.
    public Vector3 ApplyTransform(Vector3 point, Transform tform)
    {
        return (Quaternion.Euler(tform.eulerAngles) * point) + tform.position;
    }

    //removes tree and rock gameobjects
    public void Clear()
    {
        if(transform.Find("trees") != null)
        {
            DestroyImmediate(transform.Find("trees").gameObject);
        }
        if(transform.Find("rocks") != null)
        {
            DestroyImmediate(transform.Find("rocks").gameObject);
        }
    }
}
