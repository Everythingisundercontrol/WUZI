using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public interface FsmState
{
    void OnEnter();

    void OnUpdate(Manager manager);

    void OnExit();
}


public class Play : FsmState
{
    public void OnEnter()
    {
        Debug.Log("Play");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnUpdate(Manager manager)
    {
        if (Input.GetMouseButtonDown(0))
        {
            manager.LeftMousePlay();
        }
    }

    public void OnExit()
    {
        
    }
}


public class Retract : FsmState
{
    public void OnEnter()
    {
        Debug.Log("Retract");
    }

    public void OnUpdate(Manager manager)
    {
        if (Input.GetMouseButtonDown(0))
        {
            manager.LeftMouseRetract();
        }
    }

    public void OnExit()
    {
        
    }
}