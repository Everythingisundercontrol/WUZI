using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.FSM;
using UnityEngine;

public class BaseFSM
{
    public string fsmName;
    private Dictionary<FsmStateEnum, FsmState> _fsmStateDic = new Dictionary<FsmStateEnum, FsmState>();
    private FsmState _curremtFsmState;
    
    public void SetFsm(Dictionary<FsmStateEnum, FsmState> states)
    {
        _fsmStateDic = states;
    }
    
    public void ChangeFsmState(FsmStateEnum stateName)
    {
        if (!_fsmStateDic.ContainsKey(stateName))
        {
            Debug.Log("fsm里没有这个状态");
            return;
        }
        
        _curremtFsmState?.OnExit();
        _curremtFsmState = _fsmStateDic[stateName];
        _curremtFsmState.OnEnter();
    }
    
    public void OnUpdate(Manager manager)
    {
        _curremtFsmState?.OnUpdate(manager);
    }
    
}
