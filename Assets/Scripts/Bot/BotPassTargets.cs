using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPassTargets : MonoBehaviour
{

    public Dictionary<Role, Vector3> setTargets = new Dictionary<Role, Vector3>();

     // Targets bots can aim for, doesn't need to be a vector3 cause any bot can hit anywhere
    public List<Vector3> hitTargets = new List<Vector3>();
    public Dictionary<Role, Vector3> defensePositions = new Dictionary<Role, Vector3>();
    public Vector3 passTarget = Vector3.zero;
    
    private Role[] hittingRoles;
    void Start()
    {
        hittingRoles = new Role[3] { Role.OUTSIDE, Role.OPPOSITE, Role.MIDDLE };
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
                    hitTargets.Add(target.transform.position);
                    break;
                
                default:
                    break;
            }
        }
    }

    // Check & see if bots are ready to hit, and choose a random bot to set if they are.
    public Vector3 GetSetTargetFromAvailablePlayers()
    {
        // Todo: Remove after debugging
        if (setTargets.Count == 1)
        {
            return setTargets[Role.OUTSIDE];
        }

        // For now just get a random one
        Role randomRole = hittingRoles[Random.Range(0, hittingRoles.Length)];

        return setTargets[randomRole];

    }

    public Vector3 GetHitTarget()
    {
        // return hitTargets[0];
        return hitTargets[Random.Range(0, hitTargets.Count)];
    }
}
