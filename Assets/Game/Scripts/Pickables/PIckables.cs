using UnityEngine;

public abstract class Pickables : MonoBehaviour
{
  const string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Picked: " + gameObject.name);
            OnPickup(other.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(GameObject player);
}

