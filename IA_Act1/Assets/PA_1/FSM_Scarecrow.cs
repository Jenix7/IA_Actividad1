using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_ScarecrowAll", menuName = "Finite State Machines/FSM_ScarecrowAll", order = 1)]
public class FSM_Scarecrow : FiniteStateMachine
{
    private Scarecrow_Blackboard blackboard;
    private SteeringContext steeringContext;
    private float initialSpeed;

    public override void OnEnter()
    {
        blackboard = GetComponent<Scarecrow_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        initialSpeed = steeringContext.maxSpeed;
        base.OnEnter();
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {

        FiniteStateMachine PROTECT =ScriptableObject.CreateInstance<FSM_ScarecrowProtect>();
        PROTECT.Name = "PROTECT";

        State RESTING = new State("RESTING",
           () => { steeringContext.maxSpeed = 0; },
           () => { blackboard.ChangeEnergy(blackboard.drainRate * Time.deltaTime); },
           () => { steeringContext.maxSpeed = initialSpeed; }
       );

        Transition energyDrained = new Transition("Energy Drained",
            () => { return blackboard.energy <= 0; }, 
            () => { } 
        );

        Transition fullEnergy = new Transition("Full Energy",
           () => { return blackboard.energy >= blackboard.maxEnergy; },
           () => { }
       );


        AddStates(PROTECT, RESTING);
        AddTransition(PROTECT, energyDrained, RESTING);
        AddTransition(RESTING, fullEnergy, PROTECT);

        initialState = PROTECT;

    }
}
