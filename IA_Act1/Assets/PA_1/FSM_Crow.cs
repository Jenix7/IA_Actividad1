using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Crow", menuName = "Finite State Machines/FSM_Crow", order = 1)]
public class FSM_Crow : FiniteStateMachine
{
    private Flee flee;
    private SteeringContext steeringContext;
    private Crow_Blackboard blackboard;


    public override void OnEnter()
    {
        flee = GetComponent<Flee>();
        steeringContext = GetComponent<SteeringContext>();
        blackboard = GetComponent<Crow_Blackboard>();

        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1----------------------------------------

        FiniteStateMachine STEALING_POTATOS = ScriptableObject.CreateInstance<FSM_CrowStealingPotatos>();
        STEALING_POTATOS.Name = "STEALING POTATOS";

        State FLEE = new State("FLEE",
           () =>
           {
               if (transform.childCount > 0) { transform.GetChild(0).tag = "POTATO"; transform.GetChild(0).transform.parent = null; }
               flee.target = blackboard.scarecrow;
               flee.enabled = true;
               steeringContext.maxSpeed *= blackboard.speedIncreaserFlee;
           },
           () => { },
           () => { flee.enabled = false;
               steeringContext.maxSpeed /= blackboard.speedIncreaserFlee;
           }
       );

        //STAGE 2----------------------------------------
        Transition scarecrowDetected = new Transition("Scarecrow Detected",
            () => {
                return SensingUtils.FindInstanceWithinRadius(gameObject, "SCARECROW", blackboard.scarecrowDetectionRadius);
            }
        );

        Transition safeDistance = new Transition("Safe Distance",
            () => {
                return !SensingUtils.FindInstanceWithinRadius(gameObject, "SCARECROW", blackboard.scarecrowSafeDistance);
            }
        );

        //STAGE 3----------------------------------------
            
        AddStates(STEALING_POTATOS,FLEE);

        AddTransition(STEALING_POTATOS, scarecrowDetected, FLEE);
        AddTransition(FLEE,safeDistance, STEALING_POTATOS);

        //STAGE 4----------------------------------------

        initialState = STEALING_POTATOS;

    }
}
