using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FarmerRecolection", menuName = "Finite State Machines/FSM_FarmerRecolection", order = 1)]
public class FSM_FarmerRecolection : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
       Farmer_Blackboard blackboard;
       Arrive arrive;
       WanderAround wanderAround;
       GameObject thePotato;
       PotatoSpawner potatoSpawner;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<Farmer_Blackboard>();
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        potatoSpawner = GetComponent<PotatoSpawner>();
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

        State WANDERING = new State("WANDERING",
            () => { wanderAround.attractor = blackboard.ATTRACTOR; wanderAround.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wanderAround.enabled = false; }  // write on exit logic inisde {}  
        );

        State REACH_POTATO = new State("REACH_POTATO",
           () => { arrive.target = thePotato; arrive.enabled = true; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; thePotato.transform.parent = transform; thePotato.tag = "NOPOTATO";  }  // write on exit logic inisde {}  
       );

        State REACH_HOME = new State("REACH_HOME",
           () => { arrive.target = blackboard.BASKET_LOCATION; arrive.enabled = true; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; thePotato.transform.parent = null; thePotato.tag = "NOPOTATO"; potatoSpawner.potatoRecolectedCounter++; }  // write on exit logic inisde {}  
       );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition potatoDetected = new Transition("Potato Detected",
          () => {thePotato = SensingUtils.FindInstanceWithinRadius(gameObject, "POTATO", blackboard.farmerDetectionRadius);
              return thePotato != null;
          }, // write the condition checkeing code in {}
          () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
      );

        Transition potatoReached = new Transition("Potato Reached",
           () => { return SensingUtils.DistanceToTarget(gameObject, thePotato) <= blackboard.potatoReachedRadius; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition potatoVanished = new Transition("Potato Vanished",
            () => {
                return thePotato == null || thePotato.Equals(null) || thePotato.tag != "POTATO";
            }
        );

        Transition homeReached = new Transition("Home Reached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.BASKET_LOCATION) <= blackboard.basketReachedRadius; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(WANDERING, REACH_POTATO, REACH_HOME);

        AddTransition(WANDERING, potatoDetected, REACH_POTATO);
        AddTransition(REACH_POTATO, potatoReached, REACH_HOME);
        AddTransition(REACH_HOME, homeReached, WANDERING);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = WANDERING;
    }
}
