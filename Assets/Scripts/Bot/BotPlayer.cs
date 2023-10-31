using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : VolleyballPlayer
{
    public BotMove BotMove;
    public BotPass BotPass;
    public string managerTag;

    void Awake()
    {
        managerTag = team == Team.BLUE ? "BlueTeamManager" : "RedTeamManager";
    }

    void Start()
    {
        GameObject.FindGameObjectWithTag(managerTag).GetComponentInChildren<BotPlayerManager>().AddToTeam(this);
        BotMove = GetComponent<BotMove>();
        BotPass = GetComponent<BotPass>();
    }
}
