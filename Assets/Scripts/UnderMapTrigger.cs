using UnityEngine;

public class UnderMapTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerManager>() == null) return;
 
        PlayerManager.Instance.Die();
    }
}
