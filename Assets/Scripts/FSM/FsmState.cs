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