using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCylinder : MonoBehaviour
{
    public float height;
    public float radius;
    [Range(1, 100)]
    public int numMeridians = 10;
    [Range(2, 100)]
    private int numParallels = 2;

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
                float theta = col * 2 * Mathf.PI / (float)numMeridians;
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                float y = height / 2 - row * height / (numParallels - 1);
                vertices[col + row * numMeridians] = new Vector3(x, y, z);
            }
        }

        vertices[vertices.Length - 2] = new Vector3(0f, height / 2f, 0f);
        vertices[vertices.Length - 1] = new Vector3(0f, -height / 2f, 0f);

        int[] triangles = new int[numMeridians * 6 + (numMeridians * 3) * 2];
        int k = 0;
        for (int i = 0; i < numMeridians - 1; i++)
        {
            triangles[k] = i;
            triangles[k + 1] = numMeridians + i + 1;
            triangles[k + 2] = numMeridians + i;
            triangles[k + 3] = i;
            triangles[k + 4] = i + 1;
            triangles[k + 5] = numMeridians + i + 1;
            k += 6;
        }
        triangles[k] = numMeridians - 1;
        triangles[k + 1] = numMeridians;
        triangles[k + 2] = numMeridians * 2 - 1;
        triangles[k + 3] = numMeridians - 1;
        triangles[k + 4] = 0;
        triangles[k + 5] = numMeridians;
        k += 6;

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
            triangles[k + 1] = numMeridians + i;
            triangles[k + 2] = numMeridians + i + 1;
            k += 3;
        }
        triangles[k] = vertices.Length - 1;
        triangles[k + 1] = numMeridians * 2 - 1;
        triangles[k + 2] = numMeridians;
        k += 3;

        Mesh mesh = new Mesh();
        mesh.name = "my-cylinder";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    /*Mesh CreateCircleMesh()
    {
        int numVertex = detailLevel + 1;
        Vector3[] vertices = new Vector3[numVertex];
        float angleStep = 2 * Mathf.PI / detailLevel;

        // Construct Vertices
        vertices[0] = transform.position;
        for (int i = 1; i < numVertex; i++)
        {
            float theta = -(i - 1) * angleStep;
            vertices[i] = vertices[0] + new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
        }

        // Construct triangle
        int numTriangle = detailLevel;
        int[] triangles = new int[numTriangle * 3];
        int k = 0;
        for (int i = 0; i < numTriangle - 1; i++)
        {
            triangles[k] = 0;
            triangles[k + 1] = i + 1;
            triangles[k + 2] = i + 2;
            k += 3;
        }
        triangles[k] = 0;
        triangles[k + 1] = numTriangle;
        triangles[k + 2] = 1;

        Mesh mesh = new Mesh();
        mesh.name = "my-cylinder";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }*/
}
