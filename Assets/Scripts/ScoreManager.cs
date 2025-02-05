using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Manages the score and highscore through the levels and saves it with "PlayerPrefs" */
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text highScoreT;

    int scoreN = 0;
    int highScoreN = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highScoreN = PlayerPrefs.GetInt("SavedHighScore", 0);
        highScoreT.text = "" + highScoreN.ToString();
    }

    public void HighScoreCheck(int score)
    {
        scoreN = score;
        if (highScoreN < scoreN)
        {
            PlayerPrefs.SetInt("SavedHighScore", scoreN); // save the high score
        }
    }
}
