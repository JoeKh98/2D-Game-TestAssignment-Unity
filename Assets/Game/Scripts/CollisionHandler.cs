using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private AttributesComponent attributesComponent;


    void Awake()
    {
        attributesComponent = GetComponent<AttributesComponent>();
    }
    
    // private void OnCollisionEnter2D(Collision2D other) 
    // {
    //     switch(other.gameObject.tag)
    //     {
    //         case "Pickable": 
    //         {
    //             Debug.Log("Pickable");
    //             break; 
    //         }
    //         case "Trap":
    //         {
    //             Debug.Log("Trap");
    //             break; 
    //         }

    //         // case "Finish": 
    //         // {
    //         //     // if we add finish point
    //         //     Debug.Log("Finish");
    //         //     break; 
    //         // }
    //         default:
    //         {
    //             break; 
    //         }
    //     }
    // }
}
