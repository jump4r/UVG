using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : VolleyballPlayer
{
    public BotMove BotMove;
    void Start()
    {
        BotPlayerManager.AddToTeam(this);
        BotMove = GetComponent<BotMove>();
    }
}
