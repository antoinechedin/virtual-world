using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public float computeCubeValueSimple(CubeBox cube)
    {
        bool inSphere =
            Mathf.Pow(cube.transform.position.x - transform.position.x, 2)
            + Mathf.Pow(cube.transform.position.y - transform.position.y, 2)
            + Mathf.Pow(cube.transform.position.z - transform.position.z, 2)
            - (radius * radius)
            <= 0;
        if (inSphere)
        {
            return 1f;
        }
        return 0f;
    }

    public float computeCubeValueExp(CubeBox cube)
    {
        float d = Vector3.Distance(cube.transform.position, transform.position);
        return Mathf.Exp(-d * d / 20f);
    }
}
