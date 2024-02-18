using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Protect", menuName = "Finite State Machines/FSM_Protect", order = 1)]
public class FSM_ScarecrowProtect : FiniteStateMachine
{
    private ArrivePlusOA arrive;
    private Scarecrow_Blackboard blackboard;
    private SteeringContext steeringContext;

    public override void OnEnter()
    {
        arrive = GetComponent<ArrivePlusOA>();
        blackboard = GetComponent<Scarecrow_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        base.OnEnter(); 
    }

    public override void OnExit()
    {

        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //Stage1

        FiniteStateMachine PATROL = ScriptableObject.CreateInstance<FSM_ScarecrowPatrol>();
        PATROL.Name = "PATROL";

        State REACH_PIN = new State("REACH PIN",
           () => { arrive.target = blackboard.pin; arrive.enabled = true; steeringContext.maxSpeed *= blackboard.speedIncreaserSprint; blackboard.Sprint(true); }, // write on enter logic inside {}
           () => { blackboard.ChangeEnergy(-blackboard.drainRate * Time.deltaTime);
    }, // write in state logic inside {}
           () => { arrive.enabled = false; steeringContext.maxSpeed /= blackboard.speedIncreaserSprint; blackboard.Sprint(false); }  // write on exit logic inisde {}  
       );


       //Stage2

        Transition pinDetected = new Transition("Pin Detected",
            () => { blackboard.pin = GameObject.FindGameObjectWithTag("PIN"); ; return blackboard.pin != null;  }, // write the condition checkeing code in {}
            () => {  }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition pinReached = new Transition("Pin Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.pin) < blackboard.pinReachedRadius; }, // write the condition checkeing code in {}
            () => { blackboard.pin.GetComponent<MoveToClick>().PinIsReached(false); }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        //Stage3

        AddStates(PATROL,REACH_PIN);
        AddTransition(PATROL, pinDetected, REACH_PIN);
        AddTransition(REACH_PIN,pinReached, PATROL);

        //Stage4

        initialState = PATROL;
    }
}
