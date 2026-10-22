using System.Collections;
using UnityEngine;


// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class SpeedMultiplierPickup : MonoBehaviour
{

    
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float reappearCooldown = 30f;

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement player = other.GetComponentInParent<CharacterMovement>();
        if (player == null) return;

        if (collectSound != null)
        AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

        player.ApplySpeedBoost(speedMultiplier, duration);
        AbilityManager.Instance.DisableTemporarily(gameObject, reappearCooldown);
    }
}
