using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scarecrow_Blackboard : MonoBehaviour
{
    public float enemyDetectableRadius = 150f;
    public float enemyReachedRadius = 10f;
    public float enemyVanishedRadius = 140f;
    public GameObject attractor;
    
    public float screamDuration = 1;

    [Header("Scream objects")]
    public GameObject scream;
    public Sprite screamSprite;
    private Sprite mainSprite;

    [Header("Activate gizmos")]
    public bool gizmosActive;

    void Start()
    {
        attractor = GameObject.FindGameObjectWithTag("CENTER");
        scream = transform.GetChild(0).gameObject;
        scream.SetActive(false);
        mainSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void Scream(bool on)
    {
        scream.SetActive(on);
        if (on) GetComponent<SpriteRenderer>().sprite = screamSprite;
        else GetComponent<SpriteRenderer>().sprite = mainSprite;
    }

    void OnDrawGizmos()
    {
        if (gizmosActive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyDetectableRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyReachedRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, enemyVanishedRadius);
        }
    }
}
