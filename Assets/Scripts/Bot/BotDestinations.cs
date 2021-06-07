using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDestinations : MonoBehaviour
{
    public Dictionary<Role, Vector3> setPositions = new Dictionary<Role, Vector3>();

    void Start()
    {
        BallTarget[] targets = GetComponentsInChildren<BallTarget>();

        foreach (BallTarget target in targets)
        {
            switch (target.targetType)
            {   
                case TargetType.Set:
                    setPositions.Add(target.role, target.transform.position);
                    break;
                
                default:
                    break;
            }
        }
    }

}