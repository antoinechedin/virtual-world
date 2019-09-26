using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlan : MonoBehaviour
{
    public float width;
    public float height;

    [Range(1, 50)]
    public int detailLevel;

    public Material material;

    private MeshFilter mf;
    private MeshRenderer mr;

    void Start()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = material;
    }

    Mesh UpdateMesh(float width, float height, int detailLevel)
    {
        int triangleNumber = detailLevel * detailLevel * 2;

        Vector3[,] verticesMatrix = new Vector3[detailLevel + 1, detailLevel + 1];
        Vector3[] vertices = new Vector3[(detailLevel + 1) * (detailLevel + 1)];

        float widthStep = width / (float)detailLevel;
        float heightStep = height / (float)detailLevel;

        int i = 0;
        for (int row = 0; row < detailLevel + 1; row++)
        {
            for (int col = 0; col < detailLevel + 1; col++)
            {
                Vector3 vertex = new Vector3(col * widthStep - width / 2f, row * heightStep - height / 2f, .0f);
                verticesMatrix[row, col] = vertex;
                vertices[i] = vertex;
                i++;
            }
        }

        int[] triangles = new int[triangleNumber * 3];
        i = 0;
        for (int row = 0; row < detailLevel; row++)
        {
            for (int col = 0; col < detailLevel; col++)
            {
                triangles[i] = row * (detailLevel + 1) + col;
                triangles[i + 1] = (row + 1) * (detailLevel + 1) + col ;
                triangles[i + 2] = (row + 1) * (detailLevel + 1) + col + 1;
                triangles[i + 3] = row * (detailLevel + 1) + col;
                triangles[i + 4] = (row + 1) * (detailLevel + 1) + col + 1;
                triangles[i + 5] = row * (detailLevel + 1) + col + 1;

                i += 6;
            }
        }

        // Vector3[] secondPlanVertices = (Vector3[])planVertices.Clone();
        // Vector3 move = new Vector3(2, 2, 3);
        // for (int i = 0; i < secondPlanVertices.Length; i++)
        // {

        //     secondPlanVertices[i] = secondPlanVertices[i] + move;
        // }

        // Vector3[] vertices = new Vector3[planVertices.Length + secondPlanVertices.Length];
        // planVertices.CopyTo(vertices, 0);
        // secondPlanVertices.CopyTo(vertices, planVertices.Length);

        Mesh mesh = new Mesh();
        mesh.name = "my-plan";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }

    void Update()
    {
        mf.mesh = UpdateMesh(width, height, detailLevel);
    }
}
