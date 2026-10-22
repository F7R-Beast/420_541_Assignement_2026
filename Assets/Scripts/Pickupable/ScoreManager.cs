using UnityEngine;


// Keeps track of the player's score.
// It's a singleton: there is only ever one, reachable from any script
// through ScoreManager.Instance.
public class ScoreManager : MonoBehaviour
{

    private int scoreMultiplier = 2;

    private float scoreBoostEndTime = 0f;
    public bool ScoreBoostActive => Time.time < scoreBoostEndTime;

    // The single shared instance that any script can reach.
    // "static" means it belongs to the class, not to one object.
    public static ScoreManager Instance { get; private set; }

    private int lastLevelFinalScore = 0;
 


    // Anyone can read the score, only ScoreManager can change it
    public int Score { get; private set; }


    // Awake runs before any Start(), so Instance is ready before coins need it
    private void Awake()
    {
        // If a ScoreManager already exists, remove this duplicate object
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // was Destroy(this)
            return;
        }

        Instance = this;

        // Keep this object (and the score) when a new scene loads
        DontDestroyOnLoad(gameObject);
    }

    public void setLastLevelFinalScore()
    {
        lastLevelFinalScore = Score;
    }


    // Called by coins when they're collected
    public void AddScore(int amount)
    {
        if (ScoreBoostActive)
        {
            amount *= scoreMultiplier;
        }

        Score += amount;


        // Print the new total to the Console window
        Debug.Log("Score: " + Score);
    }

    public void ActivateScoreBoost(float duration, int multiplier)
    {
        scoreBoostEndTime = Time.time + duration;
        scoreMultiplier = multiplier;
    }

    public void RestartCurrentLevelScore()
    {
        Score = lastLevelFinalScore;
        scoreMultiplier = 1;
        scoreBoostEndTime = 0f;
    }
    public void ResetScore()
    {
        Score = 0;
        lastLevelFinalScore = 0;   // add this
        scoreMultiplier = 1;
        scoreBoostEndTime = 0f;
    }
}
