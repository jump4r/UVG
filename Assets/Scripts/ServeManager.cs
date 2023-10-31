using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
    {
    public static ServerManager instance;

    public VolleyballPlayer currentServer;

    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Transform ballSpawnPosition;

    [SerializeField]
    private List<VolleyballPlayer> serverList;
    private int serverIndex = 0;

    private bool waitingToServe = true;
    
    void Start()
    {
        instance = this;
        Ball.OnOutOfPlay += HandleBallOutOfPlay;

        currentServer = GameObject.FindGameObjectWithTag("Player").GetComponent<VolleyballPlayer>();

        serverList.Add(currentServer);
        serverList.Add(GameObject.Find("Blue Team Server").GetComponent<VolleyballPlayer>());
    }

    private void Update()
    {
        if (currentServer.tag == "Player")
        {
            return;
        }

        if (waitingToServe && currentServer.GetComponent<BotMove>().ArrivedAtDestination())
        {
            Debug.Log("Serve Ball..." + currentServer.GetComponent<BotMove>().destinationPoint + "  - " + currentServer.transform.position);
            InitializeBotServeBall();
        }
    }

    private void HandleBallOutOfPlay()
    {
        waitingToServe = true;
    }

    public void InitializeServeBall(Ball volleyball)
    {
        if (!currentServer)
        {
            return;
        }

        volleyball.SetLastTouchedBy(currentServer);

        VolleyballGameManager.instance.amountOfHits = 2;
        VolleyballGameManager.instance.currentBall = volleyball;
        VolleyballGameManager.instance.currentPossesion = currentServer.team;

        waitingToServe = false;
    }


    private void InitializeBotServeBall()
    {
        Ball volleyball = Instantiate(ballPrefab, ballSpawnPosition.position, ballSpawnPosition.rotation).GetComponent<Ball>();
        volleyball.GetComponent<Rigidbody>().velocity = new Vector3(0f, 2f, 0f);

        VolleyballGameManager.instance.amountOfHits = 2;
        VolleyballGameManager.instance.currentBall = volleyball;
        VolleyballGameManager.instance.currentPossesion = currentServer.team;

        currentServer.GetComponent<BotMove>().CalculateAndMoveToDestinationPoint(volleyball);

        waitingToServe = false;
    }

    public void UpdateServer(Team pointRecievedTeam)
    {
        if (pointRecievedTeam != currentServer.team)
        {
            serverIndex = (serverIndex + 1) % serverList.Count;
            currentServer = serverList[serverIndex];
        }
    }

}
