using UnityEngine;
using TMPro;
 
// Shows the score the player finished the level with.
public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;
 
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
 
        // The ScoreManager survives scene loads, so it's still here with the final score
        if (ScoreManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
            ScoreManager.Instance.ResetScore();
        } 
        else
            finalScoreText.text = "Final Score: 0";
        
    }
}
