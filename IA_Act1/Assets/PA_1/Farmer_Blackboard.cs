using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer_Blackboard : MonoBehaviour
{

    [Space(10)]
    [Header("RADIOUS")]
    public float farmerDetectionRadius = 110;
    public float potatoReachedRadius = 10;
    public float basketReachedRadius = 10;

    //ELEMENTOS IMPORTADOS POR CÓDIGO con TAG/NAME (escondidos en el INSPECTOR)-----------------------------------
    [HideInInspector] public GameObject BASKET_LOCATION;
    [HideInInspector] public GameObject ATTRACTOR;

    // ACTIVADOR DE GIZMOS----------------------------------------------------------------------------------------
    [Space(40)]
    [Header("---- Activate gizmos ------------------------------")]
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


