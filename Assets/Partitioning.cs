using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Partitioning : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    [Range(1, 20)]
    public int gridSize;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        Dictionary<int, int> indexDict = new Dictionary<int, int>();
        Mesh currentMesh = meshFilter.mesh;
        Vector3[] currentVertices = currentMesh.vertices;
        List<Vector3>[,,] grid = new List<Vector3>[gridSize, gridSize, gridSize];

        Vector3 min = currentVertices[0];
        Vector3 max = currentVertices[0];
        for (int i = 1; i < currentVertices.Length; i++)
        {
            Vector3 vertex = currentVertices[i];
            if (min.x > vertex.x) min.x = vertex.x;
            if (min.y > vertex.y) min.y = vertex.y;
            if (min.z > vertex.z) min.z = vertex.z;
            if (max.x < vertex.x) max.x = vertex.x;
            if (max.y < vertex.y) max.y = vertex.y;
            if (max.z < vertex.z) max.z = vertex.z;
        }

        float x = (max.x - min.x) / gridSize;
        float y = (max.y - min.y) / gridSize;
        float z = (max.z - min.z) / gridSize;
        Debug.Log("min: " + min);
        Debug.Log("max: " + max);
        Debug.Log("x: " + x);
        Debug.Log("y: " + y);
        Debug.Log("z: " + z);

        foreach (Vector3 vertex in currentVertices)
        {
            Vector3 p = vertex - min;
            int u = (int)(p.x / x);
            int v = (int)(p.y / y);
            int w = (int)(p.z / z);
            u = u < gridSize ? u : gridSize - 1;
            v = v < gridSize ? v : gridSize - 1;
            w = w < gridSize ? w : gridSize - 1;

            if (grid[u, v, w] == null) grid[u, v, w] = new List<Vector3>();
            grid[u, v, w].Add(vertex);
        }

        List<Vector3> newVerticesList = new List<Vector3>();
        for (int u = 0; u < gridSize; u++)
        {
            for (int v = 0; v < gridSize; v++)
            {
                for (int w = 0; w < gridSize; w++)
                {
                    Vector3 newVertex = Vector3.zero;
                    List<Vector3> cell = grid[u, v, w];
                    if (cell != null)
                    {
                        foreach (Vector3 vertex in cell)
                        {
                            indexDict.Add(Array.IndexOf(currentVertices, vertex), currentVertices.Length + newVerticesList.Count);
                            //Debug.Log(Array.IndexOf(currentVertices, vertex) + " -> " + (currentVertices.Length + newVerticesList.Count));
                            newVertex += vertex;
                        }
                        newVertex /= cell.Count;
                        newVerticesList.Add(newVertex);
                    }
                }
            }
        }

        Mesh newMesh = new Mesh();
        Vector3[] newVertices = new Vector3[currentVertices.Length + newVerticesList.Count];
        int k = 0;
        foreach (Vector3 vertex in currentVertices)
        {
            newVertices[k] = vertex;
            k++;
        }
        foreach (Vector3 vertex in newVerticesList)
        {
            newVertices[k] = vertex;
            k++;
        }

        newMesh.vertices = newVertices;

        int[] currentTriangle = currentMesh.triangles;
        List<int> triangleList = new List<int>();
        for (int i = 0; i < currentTriangle.Length; i += 3)
        {
            int t1 = currentTriangle[i];
            int t2 = currentTriangle[i + 1];
            int t3 = currentTriangle[i + 2];
            
            t1 = indexDict[t1];
            t2 = indexDict[t2];
            t3 = indexDict[t3];

            if (t1 != t2 && t1 != t3 && t2 != t3)
            {
                triangleList.Add(t1);
                triangleList.Add(t2);
                triangleList.Add(t3);
            }
        }

        int[] newTriangles = new int[triangleList.Count];
        for (int i = 0; i < triangleList.Count; i++)
        {
            newTriangles[i] = triangleList[i];
        }

        newMesh.triangles = newTriangles;

        newMesh.RecalculateNormals();
        meshFilter.mesh = newMesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
