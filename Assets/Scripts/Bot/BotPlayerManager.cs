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
    private string lastTouch;
    private BotPassTargets passTargets;
    private BotDestinations destinations;

    void Start()
    {
        BallLauncher.OnLaunch += UpdatePlayersOnHit;
        BotPass.OnBallHit += UpdatePlayersOnHit;

        passTargets = GameObject.FindGameObjectWithTag("BlueTeamManager").GetComponentInChildren<BotPassTargets>();
        destinations = GameObject.FindGameObjectWithTag("BlueTeamManager").GetComponentInChildren<BotDestinations>();
    }


    public static void AddToTeam(BotPlayer player)
    {
        if (player.team == Team.BLUE)
        {
            GameObject.FindGameObjectWithTag("BlueTeamManager").GetComponentInChildren<BotPlayerManager>().players.Add(player);
        }
    }

    public void UpdatePlayersOnHit(Ball ball)
    {
        Team landingZoneTeam = VolleyballGameManager.instance.FindTeamLandingZone();

        float shortestDistance = Int64.MaxValue;
        BotPlayer playerToReceive = null;
        
        // Find closest player to recieve ball
        foreach (BotPlayer p in players)
        {
            float distance = Vector3.Distance(p.transform.position, ball.estimatedLandingPos);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                playerToReceive = p;
            }
        }

        // Move other players to appropriate positions
        foreach (BotPlayer p in players)
        {
            if (p.name != playerToReceive.name)
            {
                MoveToNextPosition(p);
            }

            else
            {
                playerToReceive.BotMove.CalculateAndMoveToDestinationPoint(ball);
            }
        }
    }

    public void MoveToNextPosition(BotPlayer player)
    {
        int currentHit = VolleyballGameManager.instance.amountOfHits;
        
        switch (currentHit)
        {
            case 0:
                break;

            case 1:
                try 
                {
                    player.BotMove.MoveToTarget(destinations.setPositions[player.role]);
                }

                catch
                {
                    Debug.Log("We probably dont have a player role set for this position");
                }
               
                break;
            
            default:
                break;
        }
    }
}
