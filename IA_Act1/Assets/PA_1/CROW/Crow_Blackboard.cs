using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Crow_Blackboard : DynamicBlackboard
{
    public GameObject detectedPotato;
    [Space(10)]
    [Header("RADIOUS")]
    public float potatoDetectionRadius = 150f;
    public float scarecrowDetectionRadius = 170f;
    public float scarecrowSafeDistance = 220f;
    public float placeReachedRadius = 15;

    [Space(20)]
    [Header("STATS")]
    public float speedDecreaserTakingPotato = 2f;
    public float speedIncreaserFlee = 2f;

    //ELEMENTOS IMPORTADOS POR CÓDIGO con TAG/NAME (escondidos en el INSPECTOR)-----------------------------------
    [HideInInspector] public GameObject[] nestList;
    [HideInInspector] public GameObject centerPoint;
    [HideInInspector] public GameObject scarecrow;

    // ACTIVADOR DE GIZMOS----------------------------------------------------------------------------------------
    [Header("Activate gizmos")]
    public bool gizmosActive;


    void Awake()
    {
        nestList = GameObject.FindGameObjectsWithTag("NEST");
        centerPoint = GameObject.FindGameObjectWithTag("CENTER");
        scarecrow = GameObject.FindGameObjectWithTag("SCARECROW");
    }

    public GameObject GetTheNearestNest()
    {
        if (nestList == null || nestList.Length == 0) return null;

        GameObject nearestNest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject nest in nestList)
        {
            float distance = SensingUtils.DistanceToTarget(this.gameObject, nest.gameObject);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNest = nest;
            }
        }

        return nearestNest;
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