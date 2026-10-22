using System.Collections;
using UnityEngine;
public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    private void Awake()
    {
        // If one already exists, remove this duplicate object
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Pickups call this: AbilityManager.Instance.DisableTemporarily(gameObject, 30f);
    public void DisableTemporarily(GameObject obj, float seconds)
    {
        StartCoroutine(DisableRoutine(obj, seconds));
    }

    private IEnumerator DisableRoutine(GameObject obj, float seconds)
    {
        obj.SetActive(false);
        yield return new WaitForSeconds(seconds);

        // The object could have been destroyed while it was waiting
        if (obj != null) obj.SetActive(true);
    }
}