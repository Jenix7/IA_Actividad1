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
    private Seek seek;
    private Scarecrow_Blackboard blackboard;

    private GameObject raven;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<Scarecrow_Blackboard>();
        wander = GetComponent<WanderAroundPlusAvoid>();
        seek = GetComponent<Seek>();
        
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
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        State wandering = new State("Wandering",
            () => { wander.enabled = true; },
            () => { },
            () => { wander.enabled = false; }
         );

        State reachingEnemy = new State("Reaching Enemy",
           () => { seek.target = raven; seek.enabled = true; },
           () => { },
           () => { seek.enabled = false; }
        );

        State scare = new State("Scare",
           () => { },
           () => { },
           () => { }
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition enemyReached = new Transition("Enemy Reached",
            () => { raven = SensingUtils.FindInstanceWithinRadius(gameObject, "ENEMY", blackboard.enemyDetectableRadius); 
                return raven != null; }, 
            () => { } 
        );

        Transition closeEnough = new Transition("Close Enough",
            () => { return SensingUtils.DistanceToTarget(gameObject, raven) < blackboard.enemyReachedRadius; },
            () => { }
            );

        Transition farEnough = new Transition("FarEnough",
            () => { return SensingUtils.DistanceToTarget(gameObject, raven) > blackboard.enemyReachedRadius; },
            () => { }
            );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(wandering, reachingEnemy, scare);
        AddTransition(wandering, enemyReached, reachingEnemy);
        AddTransition(reachingEnemy, closeEnough, scare);
        AddTransition(scare, farEnough, wandering);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = wandering;

    }
}
