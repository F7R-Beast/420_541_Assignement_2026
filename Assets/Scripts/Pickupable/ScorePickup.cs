using UnityEngine;


// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class ScorePickup : MonoBehaviour
{
    // How many points this coin is worth (change it in the Inspector)
    [SerializeField] private int points = 50;

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    // Called automatically when something enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

            // Add our points through the ScoreManager singleton
            ScoreManager.Instance.AddScore(points);


            // Remove the coin from the scene: it's collected once and gone
            Destroy(gameObject);
        }
    }
}
