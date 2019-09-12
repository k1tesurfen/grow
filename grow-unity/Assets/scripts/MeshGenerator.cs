using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    public Texture2D heightMap;

    Vector3[] vertices;
    int[] triangles;

    private int xSize = 60;
    private int zSize = 67;

    private int xOrigin = -30;
    private int zOrigin = -15;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x=0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(xOrigin + x, getHeight(x,z), zOrigin + z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
    }

    private float getHeight(int x, int z)
    {
        float height = heightMap.GetPixel(x, z).grayscale;
        return height>0.72f?height * 8f + (height * 20 * (0.5f - Mathf.PerlinNoise(x*0.3f,z*0.3f))):height*8f;
    }
}
