using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private string playerTag = "Player";


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            AttributesComponent attributesComponent = collision.gameObject.GetComponent<AttributesComponent>();
            if (attributesComponent != null)
            {
                attributesComponent.Death(); 
            }
        }
    }
}
