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
    private FlockingAroundPlusAvoidance flocking;
    //private Crow_Blackboard blackboard;
    private SteeringContext steeringContext;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        flocking = GetComponent<FlockingAroundPlusAvoidance>();
        //blackboard = GetComponent<Crow_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();

        //wanderAround.attractor = blackboard.centerPoint;
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
        State FLOCKING = new State("FLOCKING",
          () =>
          {
              if (transform.childCount > 0) transform.GetChild(0).transform.parent = null;
              arrive.target = blackboard.centerPoint;
              arrive.enabled = true;
              steeringContext.maxSpeed = blackboard.originalVelocity;
          },
          () => { },
          () => { arrive.enabled = false; }
          );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */


          /* STAGE 3: add states and transitions to the FSM 
           * ----------------------------------------------

          AddStates(...);

          AddTransition(sourceState, transition, destinationState);

           */


          /* STAGE 4: set the initial state

          initialState = ... 

           */

    }
}
