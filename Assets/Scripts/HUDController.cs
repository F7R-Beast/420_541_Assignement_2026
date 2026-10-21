using UnityEngine;
using TMPro; // TMP_Text
 
// Updates the HUD every frame from the ScoreManager and PlayerPowerUps.
public class HUDController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private TMP_Text healthText;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
 
    private void Update()
    {
        UpdateHealth();
        UpdateScore();
    }
 
    private void UpdateScore()
    {
        // The Score property from Lab 4: anyone can read it
        scoreText.text = "Score: " + ScoreManager.Instance.Score;
    }

    private void UpdateHealth()
    {
        if (PlayerManager.Instance == null) return;
 
        healthText.text = "Health: " + PlayerManager.Instance.Health;
    }
 
}
