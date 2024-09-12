using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public void ChangeFsmState(FsmStateEnum stateName,Manager manager)
    {
        if (!_fsmStateDic.ContainsKey(stateName))
        {
            Debug.Log("fsm里没有这个状态");
            return;
        }
        
        _curremtFsmState?.OnExit();
        _curremtFsmState = _fsmStateDic[stateName];
        _curremtFsmState.OnEnter(manager);
    }
    
    public void OnUpdate()
    {
        _curremtFsmState?.OnUpdate();
    }
    
}
