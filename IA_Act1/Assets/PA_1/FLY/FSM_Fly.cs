using FSMs;
using UnityEngine;
using Steerings;
using UnityEditor.Experimental.GraphView;

[CreateAssetMenu(fileName = "FSM_Fly", menuName = "Finite State Machines/FSM_Fly", order = 1)]
public class FSM_Fly : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private Arrive arrive;
    private FlockingAround flockingA;
    private WanderAround wanderAround;
    private Fly_Blackboard blackboard;
    private SteeringContext steeringContext;
    private float elapsedTime;
    private GameObject potato;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        flockingA = GetComponent<FlockingAround>();
        blackboard = GetComponent<Fly_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();

        flockingA.attractor = blackboard.centerPoint;
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        //State FLOCKING = new State("FLOCKING",
        //  () =>
        //  {
        //      if (transform.childCount > 0) transform.GetChild(0).transform.parent = null;
        //      flockingA.enabled = true;
        //  },
        //  () => { },
        //  () => { flockingA.enabled = false; }
        //  );

        State FLOCKING = new State("WANDER",
         () =>
         { wanderAround.enabled = true; },
         () => { },
         () => { wanderAround.enabled = false; }
         );

        State REACH_POTATO = new State("REACH POTATO",
            () => { arrive.target = blackboard.detectedPotato; arrive.enabled = true; },
            () => { },
            () => { arrive.enabled = false; }
        );

        State EAT_POTATO = new State("EAT POTATO",
            () => {
                elapsedTime = 0; steeringContext.maxSpeed /= blackboard.speedIncreaserSprint;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => { steeringContext.maxSpeed *= blackboard.speedIncreaserSprint;  Destroy(blackboard.detectedPotato); }
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition potatoDetected = new Transition("Potato Detected",
            () => {
                blackboard.detectedPotato = SensingUtils.FindInstanceWithinRadius(gameObject, "POTATO", blackboard.potatoDetectionRadius);
                return blackboard.detectedPotato != null;
            }
        );

        Transition potatoReached = new Transition("Potato Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.detectedPotato) < blackboard.placeReachedRadius; }
        );

        Transition foodEaten = new Transition("Food Eaten",
            () => {
                //return SensingUtils.DistanceToTarget(gameObject, blackboard.scareCrow) < blackboard.scarecrowDetectionRadius ||
                return elapsedTime >= blackboard.eatingTime;
            }
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------

        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(FLOCKING, REACH_POTATO, EAT_POTATO);

        //AddTransition(REACH_POTATO, potatoDetected, EAT_POTATO);
        AddTransition(FLOCKING, potatoReached, REACH_POTATO);
        AddTransition(REACH_POTATO, potatoDetected, EAT_POTATO);
        AddTransition(EAT_POTATO, foodEaten, FLOCKING);


        /* STAGE 4: set the initial state

        initialState = ... 

         */

        initialState = REACH_POTATO;

    }
}
