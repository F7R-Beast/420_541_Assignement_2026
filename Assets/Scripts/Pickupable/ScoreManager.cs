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
    
        // "static" means this value belongs to the ScoreManager CLASS,
    // not to one ScoreManager object. Objects are destroyed when a
    // new scene loads, but static values stay in memory.
    public static int FinalScore { get; private set; }
 
    // Called just before we leave the level
    public void SaveFinalScore()
    {
        FinalScore = Score;
    }


    // Anyone can read the score, only ScoreManager can change it
    public int Score { get; private set; }


    // Awake runs before any Start(), so Instance is ready before coins need it
    private void Awake()
    {
        // If a ScoreManager already exists, remove this extra component
        // (only the script, not the whole Player)
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }


        // Otherwise, this is the one and only ScoreManager
        Instance = this;
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
}
