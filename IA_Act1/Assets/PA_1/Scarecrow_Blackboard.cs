using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scarecrow_Blackboard : MonoBehaviour
{
    [Space(10)]
    [Header("RADIOUS")]
    public float enemyDetectableRadius = 150f;
    public float enemyReachedRadius = 10f;
    public float enemyVanishedRadius = 140f;
    public float pinReachedRadius = 10f;

    [Space(20)]
    [Header("STATS")]
    public float energy;
    public float fullEnergy = 20f;
    public float restingTime = 3f;
    public float screamDuration = 1;
    public float speedIncreaserSprint = 3f;

    [Space(20)]
    [Header("SPRITES")]
    public Sprite mainSprite;
    public Sprite screamSprite;
    public Sprite sprintSprite;

    [Space(20)]
    [Header("FX")]
    public GameObject screamFX;
    public GameObject sprintFX;

    //ELEMENTOS IMPORTADOS POR CÓDIGO con TAG/NAME (escondidos en el INSPECTOR)-----------------------------------
    [HideInInspector] public GameObject attractor;
    [HideInInspector] public GameObject pin;
    [HideInInspector] public MoveToClick click_controller;
    private SpriteRenderer spr;

    // ACTIVADOR DE GIZMOS----------------------------------------------------------------------------------------
    [Space(40)]
    [Header("---- Activate gizmos ------------------------------")]
    public bool gizmosActive;

    void Start()
    {
        attractor = GameObject.FindGameObjectWithTag("CENTER");
        pin = GameObject.FindGameObjectWithTag("PIN");
        screamFX = transform.GetChild(0).gameObject;
        screamFX.SetActive(false);
        mainSprite = GetComponent<SpriteRenderer>().sprite;
        click_controller = pin.GetComponent<MoveToClick>();
        click_controller.PinIsReached(false);
        spr = GetComponent<SpriteRenderer>();
        sprintFX.SetActive(false);
    }

    public void Scream(bool on)
    {
        screamFX.SetActive(on);
        if (on) spr.sprite = screamSprite;
        else spr.sprite = mainSprite;
    }

    public void Sprint (bool on)
    {
        sprintFX.SetActive(on);
        if (on) spr.sprite = sprintSprite;
        else spr.sprite = mainSprite;
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
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, pinReachedRadius);
        }
    }
}
