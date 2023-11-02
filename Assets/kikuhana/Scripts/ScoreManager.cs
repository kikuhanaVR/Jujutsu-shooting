
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


public class ScoreManager : UdonSharpBehaviour
{
    public int score = 0;
    public int FinishScore = 0;
    public int scorePerBuilding = 0;
    public int scorePerEnemy = 5000;
    public Text ScoreText;
    public StartScoreAttack startScoreAttack;

    void FixedUpdate()
    {
        
        if (startScoreAttack.isScoreAttack) 
        {
            ScoreText.text = "Score: " + score.ToString();
        }
        else
        {
            ScoreText.text = "Result: " + FinishScore.ToString();
        }
    }
}
