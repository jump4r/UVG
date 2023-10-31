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
    private List<VolleyballPlayer> players = new List<VolleyballPlayer>();
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
        Ball.OnDeflect += UpdatePlayersOnHit;

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
        VolleyballPlayer playerToReceive = null;
        VolleyballPlayer playerToBlock = null;

        // Manage Players on other side of the net.
        if (team != landingZoneTeam)
        {
            playerToBlock = GetClosestPlayer(ball);
            foreach (VolleyballPlayer p in players)
            {
                if (p is BotPlayer)
                {
                    if (VolleyballGameManager.instance.amountOfHits == 2 && p.name == playerToBlock.name)
                    {
                        MoveToBlock(p as BotPlayer);
                    }

                    else
                    {
                        MoveToRecieve(p as BotPlayer);
                    }
                }
            }

            return;
        }

        // Find closest player to recieve ball
        playerToReceive = GetClosestPlayer(ball);

        if (ball.isLastTouchBlock)
        {
            Debug.Log("This is off of a block. Info: " + playerToReceive.name);
        }

        // Move other players to appropriate positions
        foreach (VolleyballPlayer p in players)
        {
            if (p is BotPlayer)
            {
                if (p.name != playerToReceive.name)
                {
                    MoveToNextPosition(p as BotPlayer);
                }

                else
                {
                    (p as BotPlayer).BotMove.CalculateAndMoveToDestinationPoint(ball);
                }
            }
        }
    }

    private VolleyballPlayer GetClosestPlayer(Ball volleyball)
    {
        float shortestDistance = Int64.MaxValue;
        VolleyballPlayer closestPlayer = null;

        foreach (VolleyballPlayer p in players)
        {
            if (p == volleyball.lastTouchedBy && !volleyball.isLastTouchBlock)
            {
                continue;
            }

            float distance = Vector3.Distance(p.transform.position, volleyball.estimatedLandingPos);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = p;
            }
        }

        return closestPlayer;
    }

    private void MoveToBlock(BotPlayer player)
    {
        player.BotPass.UpdatePassState(BotPassState.Pass);
        player.BotMove.MoveToBlockPoint(VolleyballGameManager.instance.currentBall);
    }

    private void MoveToRecieve(BotPlayer player)
    {
        player.BotPass.UpdatePassState(BotPassState.Pass);
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
                    // Debug.Log("We probably dont have a player role set for this position");
                }
               
                break;
            
            default:
                break;
        }
    }
}
