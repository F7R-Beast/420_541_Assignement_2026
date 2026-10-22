using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // The single shared instance that any script can reach.
    // "static" means it belongs to the class, not to one object.
    public static PlayerManager Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;

    // Anyone can read the score, only ScoreManager can change it
    public int Health { get; private set; }


    private bool isDead;

    // Awake runs before any Start(), so Instance is ready before traps need it
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        // Otherwise, this is the one and only PlayerManager
        Instance = this;
        Health = maxHealth;
    }

    // Called by red traps
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        Health = Mathf.Max(0, Health - amount);

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        ScoreManager.Instance.RestartCurrentLevelScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}