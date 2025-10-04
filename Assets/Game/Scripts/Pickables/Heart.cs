using UnityEngine;

public class Heart : Pickables
{
    [SerializeField] int healAmount = 1;

    protected override void OnPickup(GameObject player)
    {
        var attributes = player.GetComponent<AttributesComponent>();
        if (attributes != null)
        {
            attributes.Heal(healAmount);
            Debug.Log("Player healed by " + healAmount);
        }
    }
}
