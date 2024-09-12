using System;
using UnityEngine;


public class PlayState : FsmState
{
    private Manager _manager;

    public void OnEnter(Manager manager)
    {
        Debug.Log("Play");
        _manager = manager;
        InstanceTest.AddListenerClickLeft(OnKeyboardClickLeft);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnKeyboardClickLeft()
    {
        if (!_manager.gameOverPanel.activeSelf)
        {
            _manager.LeftMousePlay();
        }
    }
}