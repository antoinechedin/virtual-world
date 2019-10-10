using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBox 
{
    public Vector3 center;
    public float size;
    public bool enable;

    public CubeBox(Vector3 center, float size, bool enable){
        this.center = center;
        this.size = size;
        this.enable = enable;
    }
}
