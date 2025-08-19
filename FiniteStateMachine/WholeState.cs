using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeState : BaseState
{
    public override void EnterState(StateManager state)
    {
        Debug.Log("Im whole now");
    }

    public override void OnCollisionEnter(StateManager state)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(StateManager state)
    {
        throw new System.NotImplementedException();
    }
}
