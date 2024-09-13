using System;
using UnityEngine;


public class PlayState : FsmState
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
        Debug.Log("Play");
        InstanceTest.AddListenerClickLeft(OnKeyboardClickLeft);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        InstanceTest.RemoveListenerClickLeft(OnKeyboardClickLeft);
        Debug.Log("PlayState.OnExit");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnKeyboardClickLeft(Vector3 mousePosition)
    {
        if (!_manager.IfStop)
        {
            _manager.LeftMousePlay(mousePosition);
        }
    }
}