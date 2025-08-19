using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    public GrowingState growingState = new GrowingState();
    public WholeState wholeState = new WholeState();
    public ChewedState chewedState = new ChewedState();
    public RottenState rottenState = new RottenState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = growingState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
