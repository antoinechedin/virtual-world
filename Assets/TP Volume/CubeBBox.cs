using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBBox : MonoBehaviour
{
    public float size = 1;
    [Range(1, 100)]
    public int detail = 1;
    public Sphere[] spheres;
    public GameObject cubePrefab;
    [Range(0, 1)]
    public float threshold;

    CubeBox[,,] subCubes;

    // public bool showSubCube = false;

    // Start is called before the first frame update
    void Start()
    {
        float subSize = size / detail;

        subCubes = new CubeBox[detail, detail, detail];
        for (int x = 0; x < detail; x++)
            for (int y = 0; y < detail; y++)
                for (int z = 0; z < detail; z++)
                {
                    CubeBox cube = Instantiate(
                         cubePrefab,
                         new Vector3(x * subSize + (subSize - size) / 2f, y * subSize + (subSize - size) / 2f, z * subSize + (subSize - size) / 2f),
                         Quaternion.identity,
                         transform
                    ).GetComponent<CubeBox>();

                    cube.gameObject.SetActive(false);
                    cube.transform.localScale = new Vector3(subSize, subSize, subSize);

                    subCubes[x, y, z] = cube;
                }
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < detail; x++)
            for (int y = 0; y < detail; y++)
                for (int z = 0; z < detail; z++)
                {
                    CubeBox cube = subCubes[x, y, z];
                    cube.value = 0f;
                    foreach (Sphere sphere in spheres)
                    {
                        // cube.value += sphere.computeCubeValueSimple(cube);
                        cube.value += sphere.computeCubeValueExp(cube);
                    }

                    if (cube.value > threshold)
                        cube.exist = true;
                    else
                    {
                        cube.exist = false;
                    }
                }

        // Remove invisible cube
        for (int x = 0; x < detail; x++)
            for (int y = 0; y < detail; y++)
                for (int z = 0; z < detail; z++)
                {
                    CubeBox cube = subCubes[x, y, z];
                    if (cube.exist)
                    {
                        bool isCubeSurrounded = false;
                        if (x > 0 && x < detail - 1 && y > 0 && y < detail - 1 && z > 0 && z < detail - 1)

                            isCubeSurrounded = subCubes[x + 1, y, z].exist && subCubes[x - 1, y, z].exist && subCubes[x, y + 1, z].exist && subCubes[x, y - 1, z].exist && subCubes[x, y, z + 1].exist && subCubes[x, y, z - 1].exist;

                        if (isCubeSurrounded)
                        {
                            cube.gameObject.SetActive(false);
                        }
                        else
                        {
                            cube.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        cube.gameObject.SetActive(false);
                    }
                }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, size, size));

        /*if (subCubes != null)
            for (int x = 0; x < detail; x++)
                for (int y = 0; y < detail; y++)
                    for (int z = 0; z < detail; z++)
                        Gizmos.DrawWireCube(subCubes[x, y, z].center, new Vector3(subCubes[x, y, z].size, subCubes[x, y, z].size, subCubes[x, y, z].size));
        */
    }
}
