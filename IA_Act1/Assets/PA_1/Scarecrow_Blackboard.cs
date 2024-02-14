using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scarecrow_Blackboard : MonoBehaviour
{
    public float enemyDetectableRadius = 150f;
    public float enemyReachedRadius = 10f;
    public GameObject attractor;
    // Start is called before the first frame update
    void Start()
    {
        attractor = GameObject.FindGameObjectWithTag("CENTER");
    }
}
