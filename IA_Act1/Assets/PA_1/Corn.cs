using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : MonoBehaviour
{
    Scarecrow_Blackboard blackboard;
    public float energyRecover = 0;
    public float reachRadius = 60.0f;
    // Start is called before the first frame update
    void Start()
    {
        blackboard = GameObject.FindGameObjectWithTag("SCARECROW").GetComponent<Scarecrow_Blackboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SensingUtils.FindInstanceWithinRadius(gameObject, "SCARECROW", reachRadius))
        {
            blackboard.ChangeEnergy(energyRecover);
            Destroy(gameObject);
            
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        //if (collision.tag == "SCARECROW")
        //{
            //blackboard.energy += energyRecover;
            //Destroy(this.gameObject);
        //}

    //}
}
