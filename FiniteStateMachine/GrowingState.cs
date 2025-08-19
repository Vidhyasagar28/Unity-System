using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingState : BaseState
{
    public override void EnterState(StateManager state)
    {
        Debug.Log("Im just Growing ? from growing state");

    }

    public override void OnCollisionEnter(StateManager state)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(StateManager state)
    {
        if (state.transform.localScale.x < 1f)
        {
            state.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
        }
        else
        {
            state.SwitchState(state.wholeState);
        }
    }

 }
