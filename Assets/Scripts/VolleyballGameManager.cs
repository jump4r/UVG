using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolleyballGameManager : MonoBehaviour
{
    public static VolleyballGameManager instance;

    public Team currentPossesion;
    public int amountOfHits;
    public Ball currentBall;

    public float centerPosition;

    void Start()
    {
        if (instance == null) 
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }

        centerPosition = GameObject.FindGameObjectWithTag("Ground").transform.position.z; 
    }

    public void ChangePossesion()
    {
        amountOfHits = 0;
        currentPossesion = (currentPossesion == Team.RED) ? Team.BLUE : Team.RED;
    }

    public void HandleInteraction(VolleyballPlayer player)
    {
        if (player.team != currentPossesion)
        {
            ChangePossesion();
        }
        
        else {
            amountOfHits += 1;
        }
    }

    public Team FindTeamLandingZone()
    {
        return (currentBall.estimatedLandingPos.z > centerPosition) ? Team.BLUE : Team.RED;
    }
}
