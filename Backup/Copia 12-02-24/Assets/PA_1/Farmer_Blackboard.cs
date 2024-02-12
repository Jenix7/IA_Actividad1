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
    void Start()
    {
        BASKET_LOCATION = GameObject.Find("Basket");
        ATTRACTOR = GameObject.Find("Attractor");
    }

}
