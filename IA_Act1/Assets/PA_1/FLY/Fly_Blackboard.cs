using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Blackboard : MonoBehaviour
{
    public GameObject detectedPotato;
    public GameObject scareCrow;
    public GameObject centerPoint;

    public float eatingTime = 7f;
    public float speedIncreaserSprint = 2;

    [Header("RADIOUS")]
    public float potatoDetectionRadius = 40f;
    public float scarecrowDetectionRadius = 170f;
    public float scarecrowSafeDistance = 220f;
    public float placeReachedRadius = 80;

    [Header("Activate gizmos")]
    public bool gizmosActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        if (gizmosActive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, potatoDetectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, scarecrowDetectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, placeReachedRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, scarecrowSafeDistance);
        }
    }
}
