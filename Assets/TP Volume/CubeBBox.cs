using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBBox : MonoBehaviour
{
    public enum Mode { Union, Intersection };


    public float size = 1;
    [Range(1, 100)]
    public int detail = 1;
    public Sphere[] spheres;
    public Mode mode;
    public GameObject cubePrefab;

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
                    CubeBox cube = new CubeBox(
                        new Vector3(x * subSize + (subSize - size) / 2f, y * subSize + (subSize - size) / 2f, z * subSize + (subSize - size) / 2f),
                        subSize,
                        false
                    );

                    bool enable = false;
                    switch (mode)
                    {
                        case (Mode.Intersection):
                            enable = true;
                            foreach (Sphere sphere in spheres)
                            {
                                bool inSphere =
                                    Mathf.Pow(cube.center.x - sphere.transform.position.x, 2)
                                    + Mathf.Pow(cube.center.y - sphere.transform.position.y, 2)
                                    + Mathf.Pow(cube.center.z - sphere.transform.position.z, 2)
                                    - (sphere.radius * sphere.radius)
                                    <= 0;
                                if (!inSphere)
                                {
                                    enable = false;
                                    break;
                                }
                            }
                            break;

                        case (Mode.Union):
                            enable = false;
                            foreach (Sphere sphere in spheres)
                            {
                                bool inSphere =
                                    Mathf.Pow(cube.center.x - sphere.transform.position.x, 2)
                                    + Mathf.Pow(cube.center.y - sphere.transform.position.y, 2)
                                    + Mathf.Pow(cube.center.z - sphere.transform.position.z, 2)
                                    - (sphere.radius * sphere.radius)
                                    <= 0;
                                if (inSphere)
                                {
                                    enable = true;
                                    break;
                                }
                            }
                            break;

                    }

                    cube.enable = enable;
                    subCubes[x, y, z] = cube;
                }

        for (int x = 0; x < detail; x++)
            for (int y = 0; y < detail; y++)
                for (int z = 0; z < detail; z++)
                {
                    bool isCubeSurrounded = false;
                    if (x > 0 && x < detail - 1 && y > 0 && y < detail - 1 && z > 0 && z < detail - 1)
                        isCubeSurrounded = subCubes[x + 1, y, z].enable && subCubes[x - 1, y, z].enable && subCubes[x, y + 1, z].enable && subCubes[x, y - 1, z].enable && subCubes[x, y, z + 1].enable && subCubes[x, y, z - 1].enable;

                    if (subCubes[x, y, z].enable && !isCubeSurrounded)
                    {
                        GameObject obj = Instantiate(cubePrefab, subCubes[x, y, z].center, Quaternion.identity, transform);
                        obj.transform.localScale = new Vector3(subCubes[x, y, z].size, subCubes[x, y, z].size, subCubes[x, y, z].size);
                    }
                }

    }

    // Update is called once per frame
    void Update()
    {

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
