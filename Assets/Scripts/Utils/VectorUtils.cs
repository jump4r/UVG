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

  
}
