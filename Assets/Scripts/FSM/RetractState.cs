using System;
using UnityEngine;


public class RetractState : FsmState
{
    private Manager _manager;

    public void OnEnter(Manager manager)
    {
        Debug.Log("Retract");
        _manager = manager;
        InstanceTest.AddListenerClickLeft(OnKeyboardClickLeft);
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

    private void OnKeyboardClickLeft()
    {
        if (!_manager.gameOverPanel.activeSelf)
        {
            _manager.LeftMouseRetract();
        }
    }
}