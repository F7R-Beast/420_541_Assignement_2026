using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager health = collision.gameObject.GetComponent<PlayerManager>();

            health.TakeDamage(damage);
        }
    }
}