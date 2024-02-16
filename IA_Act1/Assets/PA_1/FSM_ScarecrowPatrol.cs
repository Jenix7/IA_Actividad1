using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_ScarecrowPatrol", menuName = "Finite State Machines/FSM_ScarecrowPatrol", order = 1)]
public class FSM_ScarecrowPatrol : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private WanderAroundPlusAvoid wander;
    private ArrivePlusOA arrive;
    private Scarecrow_Blackboard blackboard;

    private GameObject enemy;
    private float elapsedTime;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<Scarecrow_Blackboard>();
        wander = GetComponent<WanderAroundPlusAvoid>();
        arrive = GetComponent<ArrivePlusOA>();
        
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        ///* STAGE 1: create the states with their logic(s)
         
        State WANDERING = new State("WANDERING",
            () => { wander.attractor = blackboard.attractor; wander.enabled = true; },
            () => { },
            () => { wander.enabled = false; }
         );

        State REACH_ENEMY = new State("REACH ENEMY",
           () => { arrive.target = enemy; arrive.enabled = true; },
           () => { },
           () => { arrive.enabled = false; }
        );

        State SCARE = new State("SCARE",
           () => { elapsedTime = 0; blackboard.Scream(true); },
           () => { elapsedTime += Time.deltaTime; },
           () => { elapsedTime = 0; blackboard.Scream(false); }
        );


        ///* STAGE 2: create the transitions with their logic(s)

        Transition enemyDetected = new Transition("Enemy Detected",
            () => {
                enemy = SensingUtils.FindInstanceWithinRadius(gameObject, "CROW", blackboard.enemyDetectableRadius);
                return enemy != null; }, 
            () => { } 
        );

        Transition enemyReached = new Transition("Enemy Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, enemy) < blackboard.enemyReachedRadius; },
            () => { }
            );

        Transition enemyVanished = new Transition("Enemy Vanished",
            () => { return SensingUtils.DistanceToTarget(gameObject, enemy) > blackboard.enemyVanishedRadius; },
            () => { }
            );

        Transition endScream = new Transition("FarEnough",
            () => { return elapsedTime >= blackboard.screamDuration; },
            () => { }
            );

        ///* STAGE 3: add states and transitions to the FSM 

        AddStates(WANDERING, REACH_ENEMY, SCARE);

        AddTransition(WANDERING, enemyDetected, REACH_ENEMY);
        AddTransition(REACH_ENEMY, enemyReached, SCARE);
        AddTransition(REACH_ENEMY, enemyVanished, WANDERING);
        AddTransition(SCARE, endScream, WANDERING);


        ///* STAGE 4: set the initial state
        
        initialState = WANDERING;

    }
}
