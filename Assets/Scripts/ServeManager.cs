using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
    {
    // Start is called before the first frame update

    public static ServerManager instance;

    public VolleyballPlayer currentServer;
    
    void Start()
    {
        instance = this;
        currentServer = GameObject.FindGameObjectWithTag("Player").GetComponent<VolleyballPlayer>();
    }

    public void InitializeServeBall(Ball volleyball)
    {
        if (!currentServer)
        {
            return;
        }

        VolleyballGameManager.instance.amountOfHits = 2;
        VolleyballGameManager.instance.currentBall = volleyball;
        VolleyballGameManager.instance.currentPossesion = currentServer.team;
    }
}
