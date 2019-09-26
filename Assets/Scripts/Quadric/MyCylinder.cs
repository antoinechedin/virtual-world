using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCylinder : MonoBehaviour
{
    public float height;
    public float defaultRadius;
    [Range(1, 100)]
    public int numMeridians = 10;
    [Range(2, 100)]
    public int numParallels = 2;
    public float[] radiuses;

    private MeshFilter mf;
    private MeshRenderer mr;

    private void Start()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Standard"));
    }

    private void Update()
    {
        mf.mesh = CreateMesh();
    }

    Mesh CreateMesh()
    {
        Vector3[] vertices = new Vector3[numParallels * numMeridians + 2];
        for (int row = 0; row < numParallels; row++)
        {
            for (int col = 0; col < numMeridians; col++)
            {
                float radiusAtRow = row < radiuses.Length ? radiuses[row] : defaultRadius;

                float theta = col * 2 * Mathf.PI / (float)numMeridians;
                float x = radiusAtRow * Mathf.Cos(theta);
                float z = radiusAtRow * Mathf.Sin(theta);
                float y = height / 2 - row * height / (numParallels - 1);
                vertices[col + row * numMeridians] = new Vector3(x, y, z);
            }
        }

        vertices[vertices.Length - 2] = new Vector3(0f, height / 2f, 0f);
        vertices[vertices.Length - 1] = new Vector3(0f, -height / 2f, 0f);

        int[] triangles = new int[(numParallels - 1) * numMeridians * 6 + (numMeridians * 3) * 2];
        int k = 0;
        for (int p = 0; p < numParallels - 1; p++)
        {
            for (int m = 0; m < numMeridians - 1; m++)
            {
                triangles[k] = numMeridians * p + m;
                triangles[k + 1] = numMeridians * p + numMeridians + m + 1;
                triangles[k + 2] = numMeridians * p + numMeridians + m;
                triangles[k + 3] = numMeridians * p + m;
                triangles[k + 4] = numMeridians * p + m + 1;
                triangles[k + 5] = numMeridians * p + numMeridians + m + 1;
                k += 6;
            }
        }
        // Close cylinder
        for (int p = 0; p < numParallels - 1; p++)
        {
            triangles[k] = numMeridians * p + numMeridians - 1;
            triangles[k + 1] = numMeridians * p + numMeridians;
            triangles[k + 2] = numMeridians * p + numMeridians * 2 - 1;
            triangles[k + 3] = numMeridians * p + numMeridians - 1;
            triangles[k + 4] = numMeridians * p;
            triangles[k + 5] = numMeridians * p + numMeridians;
            k += 6;
        }

        // Close top
        for (int i = 0; i < numMeridians - 1; i++)
        {
            triangles[k] = vertices.Length - 2;
            triangles[k + 1] = i + 1;
            triangles[k + 2] = i;
            k += 3;
        }
        triangles[k] = vertices.Length - 2;
        triangles[k + 1] = 0;
        triangles[k + 2] = numMeridians - 1;
        k += 3;

        // Close bottom
        for (int i = 0; i < numMeridians - 1; i++)
        {
            triangles[k] = vertices.Length - 1;
            triangles[k + 1] = (numParallels - 1) * numMeridians + i;
            triangles[k + 2] = (numParallels - 1) * numMeridians + i + 1;
            k += 3;
        }
        triangles[k] = vertices.Length - 1;
        triangles[k + 1] = (numParallels - 1) * numMeridians + numMeridians - 1;
        triangles[k + 2] = (numParallels - 1) * numMeridians;
        k += 3;

        Mesh mesh = new Mesh();
        mesh.name = "my-cylinder";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
