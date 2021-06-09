using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPassTargets : MonoBehaviour
{

    public Dictionary<Role, Vector3> setTargets = new Dictionary<Role, Vector3>();
    public Dictionary<Role, Vector3> hitTargets = new Dictionary<Role, Vector3>();
    public Dictionary<Role, Vector3> defensePositions = new Dictionary<Role, Vector3>();
    public Dictionary<Role, Vector3> hitPositions = new Dictionary<Role, Vector3>();
    public Vector3 passTarget = Vector3.zero;
    void Start()
    {
        BallTarget[] targets = GetComponentsInChildren<BallTarget>();

        foreach (BallTarget target in targets)
        {
            switch (target.targetType)
            {
                case TargetType.Pass:
                    passTarget = target.transform.position;
                    break;
                
                case TargetType.Set:
                    setTargets.Add(target.role, target.transform.position);
                    break;
                
                case TargetType.Hit:
                    hitTargets.Add(target.role, target.transform.position);
                    break;
                
                default:
                    break;
            }
        }
    }
}
