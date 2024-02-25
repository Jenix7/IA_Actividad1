using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_CrowStealingPotatos", menuName = "Finite State Machines/FSM_CrowStealingPotatos", order = 1)]
public class FSM_CrowStealingPotatos : FiniteStateMachine
{
    private Arrive arrive;
    private WanderAround wanderAround;
    private Crow_Blackboard blackboard;
    private SteeringContext steeringContext;

    
    private GameObject detectedNest;


    public override void OnEnter()
    {
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        blackboard = GetComponent<Crow_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        
        wanderAround.attractor = blackboard.centerPoint;

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        if (transform.childCount > 0) transform.GetChild(0).transform.parent = null;
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1----------------------------------------

        State REACH_CENTER = new State("REACH CENTER",
           () => {
               if (transform.childCount > 0) transform.GetChild(0).transform.parent = null;
               steeringContext.maxSpeed = blackboard.originalVelocity;
               arrive.target = blackboard.centerPoint;
               arrive.enabled = true; },
           () => { },
           () => { arrive.enabled = false; }
       );

        State WANDERING_AROUND = new State("WANDERING AROUND",
            () => { wanderAround.enabled = true; },
            () => { },
            () => { wanderAround.enabled = false; }
        );

        State REACH_POTATO = new State("REACH POTATO",
            () => { arrive.target = blackboard.detectedPotato; arrive.enabled = true; },
            () => { },
            () => { arrive.enabled = false; }
        );

        State TAKE_POTATO = new State("TAKE POTATO",
            () => {
                blackboard.detectedPotato.transform.parent = transform;
                blackboard.detectedPotato.tag = "STEALED_POTATO";
                detectedNest = blackboard.GetTheNearestNest();
                arrive.target = detectedNest;
                arrive.enabled = true;
                steeringContext.maxSpeed = steeringContext.maxSpeed/blackboard.speedDecreaserTakingPotato;
            },
            () => { },
            () => { arrive.enabled = false; steeringContext.maxSpeed = blackboard.originalVelocity; Score.potatoRecolectedInScene++; }
        );

        //STAGE 2----------------------------------------

        Transition potatoDetected = new Transition("Potato Detected",
            () => {
                blackboard.detectedPotato = SensingUtils.FindInstanceWithinRadius(gameObject, "POTATO", blackboard.potatoDetectionRadius);
                return blackboard.detectedPotato != null;
            }
        );

        Transition centerReached = new Transition("Center Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.centerPoint) <= blackboard.placeReachedRadius; }
        );

        Transition potatoVanished = new Transition("Potato Vanished",
            () => { 
                return blackboard.detectedPotato == null || blackboard.detectedPotato.Equals(null) || blackboard.detectedPotato.tag != "POTATO"; }
        );

        Transition potatoReached = new Transition("Potato Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.detectedPotato) < blackboard.placeReachedRadius; }
        );

        Transition nestReached = new Transition("Nest Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, detectedNest) < blackboard.placeReachedRadius; }
        );


        //STAGE 3----------------------------------------

        AddStates(REACH_CENTER,WANDERING_AROUND,REACH_POTATO,TAKE_POTATO);

        AddTransition(REACH_CENTER, centerReached, WANDERING_AROUND);
        AddTransition(REACH_CENTER, potatoDetected, REACH_POTATO);
        AddTransition(WANDERING_AROUND, potatoDetected, REACH_POTATO);
        AddTransition(REACH_POTATO,potatoVanished,REACH_CENTER);
        AddTransition(REACH_POTATO,potatoReached,TAKE_POTATO);
        AddTransition(TAKE_POTATO, nestReached, REACH_CENTER);


        //STAGE 3----------------------------------------

        initialState = REACH_CENTER;

    }

}
