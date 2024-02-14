using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer_Blackboard : MonoBehaviour
{
    // Start is called before the first frame update
    
    //[Header("Recolection")]
    public GameObject BASKET_LOCATION;
    public GameObject ATTRACTOR;
    public float farmerDetectionRadius = 110;
    public float potatoReachedRadius = 10;
    public float basketReachedRadius = 10;

    [Header("Activate gizmos")]
    public bool gizmosActive;

    void Start()
    {
        BASKET_LOCATION = GameObject.Find("Basket");
        ATTRACTOR = GameObject.Find("Attractor");
    }

    void OnDrawGizmos()
    {
        if (gizmosActive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, farmerDetectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, potatoReachedRadius);
        }
    }

}


