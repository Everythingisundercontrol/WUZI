using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class InstanceTest
{
    private static InstanceTest _instance;

    private static List<Action> _actionOnADown;
    private static List<Action> _actionOnDDown;
    private static List<Action> _actionOnEscDown;
    private static List<Action<Vector3>> _actionOnClickLeft;

    public static InstanceTest GetInstance()
    {
        return _instance ?? new InstanceTest();
    }

    /// <summary>
    /// 订阅ADown
    /// </summary>
    public static void AddListenerADown(Action action)
    {
        if (_actionOnADown == null)
        {
            _actionOnADown = new List<Action>();
        }
        _actionOnADown.Add(action);
    }

    /// <summary>
    /// 订阅DDown
    /// </summary>
    public static void AddListenerDDown(Action action)
    {
        if (_actionOnDDown == null)
        {
            _actionOnDDown = new List<Action>();
        }
        _actionOnDDown.Add(action);
    }
    
    /// <summary>
    /// 订阅EscDown
    /// </summary>
    public static void AddListenerEscDown(Action action)
    {
        if (_actionOnEscDown == null)
        {
            _actionOnEscDown = new List<Action>();
        }
        _actionOnEscDown.Add(action);
    }
    
    /// <summary>
    /// 订阅左键点击
    /// </summary>
    public static void AddListenerClickLeft(Action<Vector3> action)
    {
        if (_actionOnClickLeft == null)
        {
            _actionOnClickLeft = new List<Action<Vector3>>();
        }
        _actionOnClickLeft.Add(action);
    }
    
    
    
    /// <summary>
    /// 取消订阅左键点击
    /// </summary>
    public static void RemoveListenerClickLeft(Action<Vector3> action)
    {
        if (_actionOnClickLeft.Contains(action))
        {
            _actionOnClickLeft.Remove(action);
        }
    }
    
    
    
    
    
    /// <summary>
    /// 左键点击事件触发
    /// </summary>
    public static void CallListenerClickLeft(Vector3 pos)
    {
        foreach (var clickLeftEvent in _actionOnClickLeft)
        {
            clickLeftEvent.Invoke(pos);
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 按A事件触发
    /// </summary>
    public static void CallListenerADown()
    {
        foreach (var aDownEvent in _actionOnADown)
        {
            aDownEvent.Invoke();
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 按D事件触发
    /// </summary>
    public static void CallListenerDDown()
    {
        foreach (var dDownEvent in _actionOnDDown)
        {
            dDownEvent.Invoke();
        }
        
    }

    /// <summary>
    /// 按Esc事件触发
    /// </summary>
    public static void CallListenerEscDown()
    {
        foreach (var escDownEvent in _actionOnEscDown)
        {
            escDownEvent.Invoke();
        }
        
    }
}