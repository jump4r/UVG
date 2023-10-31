using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtManager : MonoBehaviour
{
    public static CourtManager instance = null;

    [SerializeField]
    private Collider redTeamCourtBounds;

    [SerializeField]
    private Collider blueTeamCourtBounds;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Ball.OnOutOfPlay += HandleOnOutOfPlay;
    }

    void HandleOnOutOfPlay()
    {
        Ball volleyball = VolleyballGameManager.instance.currentBall;

        if (redTeamCourtBounds.bounds.Contains(volleyball.bouncePos))
        {
            // Add point to Blue team
            ScoreManager.instance.AddToScore(Team.BLUE);
        }

        else if (blueTeamCourtBounds.bounds.Contains(volleyball.bouncePos))
        {
            // Add point to Red Team
            ScoreManager.instance.AddToScore(Team.RED);
        }

        // If the ball didn't land in the court, the point goes to the team that DIDN'T touch the ball last.
        else
        {
            Team oppositeTeam = volleyball.lastTouchedBy.team == Team.BLUE ? Team.RED : Team.BLUE;
            ScoreManager.instance.AddToScore(oppositeTeam);

        }
    }
}
