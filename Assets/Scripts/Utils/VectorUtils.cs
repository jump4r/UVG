using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
  public static Vector3 NoiseVector(float noiseThreshold) {
    return new Vector3(
        Random.Range(noiseThreshold * -1f, noiseThreshold),
        Random.Range(noiseThreshold * -1f, noiseThreshold),
        Random.Range(noiseThreshold * -1f, noiseThreshold)
    );
  }

  public static Vector3 YPlanarVector(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    } 
    
  public static Vector3 YConstantPlanarVector(Vector3 v, Vector3 constant)
    {
        return new Vector3(v.x, constant.y, v.z);
    }

  
}
