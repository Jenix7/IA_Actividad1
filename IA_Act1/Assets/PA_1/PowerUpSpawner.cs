using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    private GameObject sample;
    public float maxX = 0;
    public float maxY = 0;
    public float potatoRecolectedCounter = 0;
    public float reachCornNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        sample = Resources.Load<GameObject>("CORN");
        if(sample == null)
        {
            Debug.LogError("No CORN prefab found as a resource");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clone;
        if (potatoRecolectedCounter >= reachCornNumber)
        {

            // spawn creating an instance...
           
            clone = Instantiate(sample);
            clone.transform.position = new Vector3(maxX * Steerings.Utils.binomial(), maxY * Steerings.Utils.binomial(), 0);


            potatoRecolectedCounter = 0;
           
           
        }
        else
        {

        }
       
    }
}
