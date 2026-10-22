using System.Collections;
using UnityEngine;


// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class ScoreMultiplierPickup : MonoBehaviour
{
    // How many points this coin is worth (change it in the Inspector)
    [Header("Ability Settings")]
    [SerializeField] private int scoreMultipler = 2;
    [SerializeField] private float duration = 10f;

    [Header("Pickable Ability Settings")]
    [SerializeField] private float reappearCooldown = 30f;

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
            ScoreManager.Instance.ActivateScoreBoost(duration, scoreMultipler);
        }

        AbilityManager.Instance.DisableTemporarily(gameObject, reappearCooldown);
        
    }
}
