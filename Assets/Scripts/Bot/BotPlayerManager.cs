using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayerManager : MonoBehaviour
{
    [SerializeField]
    private Team team;
    [SerializeField]
    private string managerTag;
    [SerializeField]
    private List<BotPlayer> players = new List<BotPlayer>();
    private string lastTouch;
    private BotPassTargets passTargets;
    private BotDestinations destinations;


    void Start()
    {
        BallLauncher.OnServe += UpdatePlayersOnHit;

        // Listen for any hits from other bots or player
        BotPass.OnBallHit += UpdatePlayersOnHit;
        PassPlatform.OnBallHit += UpdatePlayersOnHit;
        Hand.OnBallHit += UpdatePlayersOnHit;
        PlayerSetter.OnBallSet += UpdatePlayersOnHit;

        passTargets = GameObject.FindGameObjectWithTag(managerTag).GetComponentInChildren<BotPassTargets>();
        destinations = GameObject.FindGameObjectWithTag(managerTag).GetComponentInChildren<BotDestinations>();
    }


    public void AddToTeam(BotPlayer player)
    {
        this.players.Add(player);
    }

    public void UpdatePlayersOnHit(Ball ball)
    {
        Team landingZoneTeam = VolleyballGameManager.instance.FindTeamLandingZone();

        float shortestDistance = Int64.MaxValue;
        BotPlayer playerToReceive = null;

        if (team != landingZoneTeam)
        {
            foreach (BotPlayer p in players)
            {
                MoveToRecieve(p);
            }

            return;
        }
        
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
                playerToReceive.BotMove.CalculateAndMoveToDestinationPoint(ball, VolleyballGameManager.instance.amountOfHits);
            }
        }
    }

    private void MoveToRecieve(BotPlayer player)
    {
        player.BotMove.MoveToTarget(destinations.serveRecievePositions[player.role]);
    }

    private void MoveToNextPosition(BotPlayer player)
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
