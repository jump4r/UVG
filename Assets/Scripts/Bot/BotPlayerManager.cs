using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayerManager : MonoBehaviour
{
    [SerializeField]
    private Team team;
    [SerializeField]
    private List<BotPlayer> players = new List<BotPlayer>();

    void Start()
    {
        BallLauncher.OnLaunch += FindClosestPlayerToLanding;
    }


    public static void AddToTeam(BotPlayer player)
    {
        if (player.team == Team.BLUE)
        {
            GameObject.FindGameObjectWithTag("BlueTeamManager").GetComponentInChildren<BotPlayerManager>().players.Add(player);
        }
    }

    public void FindClosestPlayerToLanding(Ball ball)
    {
        Team landingZoneTeam = VolleyballGameManager.instance.FindTeamLandingZone();

        float shortestDistance = Int64.MaxValue;
        BotPlayer playerToReceive = null;
        
        foreach (BotPlayer p in players)
        {
            float distance = Vector3.Distance(p.transform.position, ball.estimatedLandingPos);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                playerToReceive = p;
            }
        }

        if (playerToReceive != null)
        {
            playerToReceive.BotMove.CalculateAndMoveToDestinationPoint(ball);
        }
    }
}
