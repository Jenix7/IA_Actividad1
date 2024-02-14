using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Crow_Blackboard : DynamicBlackboard
{
    public float potatoDetectionRadius = 150f;
    public float scarecrowDetectionRadius = 170f;
    public float scarecrowSafeDistance = 220f;
    public float placeReachedRadius = 15;

    public GameObject[] nestList;
    public float speedDecreaserTakingPotato = 2f;
    public float speedIncreaserFlee = 2f;
    public GameObject centerPoint;
    public GameObject scarecrow;


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