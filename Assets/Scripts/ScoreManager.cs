using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    private int redTeamScore = 0;
    private int blueTeamScore = 0;

    public TMPro.TextMeshPro redTeamScoreText;
    public TMPro.TextMeshPro blueTeamScoreText;

    private void Start()
    {
        instance = this;


        // Todo: listen to on ball out of play, update score with appropriate team
        // Ignore double hits by player for now.
    }

    public void AddToScore(Team team)
    {
        if (team == Team.BLUE)
        {
            blueTeamScore += 1;
            blueTeamScoreText.text = blueTeamScore.ToString("00");
        }

        else if (team ==  Team.RED)
        {
            redTeamScore += 1;
            redTeamScoreText.text = redTeamScore.ToString("00");
        }

        ServerManager.instance.UpdateServer(team);
    }


}
