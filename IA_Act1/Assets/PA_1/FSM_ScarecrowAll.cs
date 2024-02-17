using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_ScarecrowAll", menuName = "Finite State Machines/FSM_ScarecrowAll", order = 1)]
public class FSM_ScarecrowAll : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private Scarecrow_Blackboard blackboard;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<Scarecrow_Blackboard>();
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

        FiniteStateMachine Protect =ScriptableObject.CreateInstance<FSM_Protect>();
        Protect.Name = "Protect";

        State resting = new State("Resting",
           () => { blackboard.restingTime = 0f; }, // write on enter logic inside {}
           () => { blackboard.restingTime += Time.deltaTime; }, // write in state logic inside {}
           () => { }  // write on exit logic inisde {}  
       );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition energyDrained = new Transition("Energy Drained",
            () => { return blackboard.energy < blackboard.fullEnergy; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition fullEnergy = new Transition("Full Energy",
           () => { return blackboard.energy >= blackboard.fullEnergy; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(Protect);
        AddTransition(Protect, energyDrained, resting);
        AddTransition(resting, fullEnergy, Protect);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = Protect;

    }
}
