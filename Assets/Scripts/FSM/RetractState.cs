using System;
using UnityEngine;


public class RetractState : FsmState
{
    private Manager _manager;

    public void OnInit(Manager manager)
    {
        _manager = manager;
    }

    public void OnEnter()
    {
        if (!_manager)
        {
            return;
        }

        Debug.Log("Retract");
        InstanceTest.AddListenerClickLeft(OnKeyboardClickLeft);
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        InstanceTest.RemoveListenerClickLeft(OnKeyboardClickLeft);
        Debug.Log("RetractState.OnExit");
    }

    private void OnKeyboardClickLeft(Vector3 pos)
    {
        if (!_manager.IfStop)
        {
            _manager.LeftMouseRetract();
        }
    }
}