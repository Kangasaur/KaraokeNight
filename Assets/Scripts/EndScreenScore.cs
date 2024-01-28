using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenScore : MonoBehaviour
{
    int score;
    int countUpScore;

    public TextMeshProUGUI scorecounter;
    public TextMeshProUGUI highscore;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        countUpScore = ScoreManager.instance.score / 150;
    }

    // Update is called once per frame
    void Update()
    {
        score += countUpScore;
        if (score > ScoreManager.instance.score)
        {
            score = ScoreManager.instance.score;
            highscore.text = "Highest score: " + ScoreManager.instance.highScore.ToString();
        }
        scorecounter.text = score.ToString();
    }
}
