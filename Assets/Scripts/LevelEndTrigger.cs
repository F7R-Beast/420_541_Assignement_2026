using UnityEngine;
using UnityEngine.SceneManagement;
// When the player enters it, save the score and load the End scene.
public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private string nextScene = "";
 
    private void OnTriggerEnter(Collider other)
{
    // Only the player can finish the level
    if (!other.CompareTag("Player")) return;

    ScoreManager.Instance.setLastLevelFinalScore();

    SceneManager.LoadScene(nextScene);
}
}
