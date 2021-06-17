using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDestinations : MonoBehaviour
{
    public Dictionary<Role, Vector3> setPositions = new Dictionary<Role, Vector3>();
    public Dictionary<Role, Vector3> serveRecievePositions = new Dictionary<Role, Vector3>();

    void Start()
    {
        BallTarget[] targets = GetComponentsInChildren<BallTarget>();

        foreach (BallTarget target in targets)
        {
            switch (target.targetType)
            {   
                case TargetType.Pass:
                    serveRecievePositions.Add(target.role, target.transform.position);
                    break;
                
                case TargetType.Set:
                    setPositions.Add(target.role, target.transform.position);
                    break;
                
                default:
                    break;
            }
        }

        Debug.Log("Is this running?");

        foreach (var item in serveRecievePositions)
        {
            Debug.Log("Serve Receive Target: " + item.Key + " : " + item.Value);
        }

        foreach (var item in setPositions)
        {
            Debug.Log("Set Target: " + item.Key + " : " + item.Value);
        }
    }

    public Vector3 GetServeReceivePosition(Role role)
    {
        return serveRecievePositions[role];
    }

}
