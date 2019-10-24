using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class OFFLoader : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    public TextAsset offFile;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = LoadMeshFromFile(offFile);
    }

    public Mesh LoadMeshFromFile(TextAsset offFile)
    {
        string[] lines = offFile.text.Split('\n');
        string[] meshInfos = lines[1].Split(' ');

        int numVertex = int.Parse(meshInfos[0]);
        int numFacet = int.Parse(meshInfos[1]);
        int numEdge = int.Parse(meshInfos[2]);

        //Debug.Log("Loading mesh with: [" + numVertex + ", " + numFacet + ", " + numEdge + "]");

        Vector3[] vertices = new Vector3[numVertex];
        for (int i = 0; i < numVertex; i++)
        {
            string[] vertexInfo = lines[2 + i].Split(' ');
            vertices[i] = new Vector3(
                float.Parse(vertexInfo[0], CultureInfo.InvariantCulture),
                float.Parse(vertexInfo[1], CultureInfo.InvariantCulture),
                float.Parse(vertexInfo[2], CultureInfo.InvariantCulture)
                );
        }

        int[] triangles = new int[numFacet * 3];
        for (int i = 0; i < numFacet; i++)
        {
            string[] triangleInfo = lines[2 + numVertex + i].Split(' ');
            triangles[i * 3] = int.Parse(triangleInfo[1]);
            triangles[i * 3 + 1] = int.Parse(triangleInfo[2]);
            triangles[i * 3 + 2] = int.Parse(triangleInfo[3]);
        }

        //Debug.Log("Mesh loaded");

        Mesh mesh = new Mesh();
        mesh.name = offFile.name;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
