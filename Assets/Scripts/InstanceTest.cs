using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class InstanceTest
{
    private static InstanceTest _instance;

    private static Action _actionOnADown;
    private static Action _actionOnDDown;
    private static Action _actionOnEscDown;
    private static Action _actionOnClickLeft;

    public static InstanceTest GetInstance()
    {
        return _instance == null ? _instance : new InstanceTest();
    }

    /// <summary>
    /// 订阅ADown
    /// </summary>
    public static void AddListenerADown(Action action)
    {
        _actionOnADown = action;
    }

    /// <summary>
    /// 订阅DDown
    /// </summary>
    public static void AddListenerDDown(Action action)
    {
        _actionOnDDown = action;
    }
    
    /// <summary>
    /// 订阅EscDown
    /// </summary>
    public static void AddListenerEscDown(Action action)
    {
        _actionOnEscDown = action;
    }
    
    /// <summary>
    /// 订阅左键点击
    /// </summary>
    public static void AddListenerClickLeft(Action action)
    {
        _actionOnClickLeft = action;
    }
    
    
    
    
    
    /// <summary>
    /// 左键点击事件触发
    /// </summary>
    public static void CallListenerClickLeft()
    {
        _actionOnClickLeft?.Invoke();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 按A事件触发
    /// </summary>
    public static void CallListenerADown()
    {
        _actionOnADown?.Invoke();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 按D事件触发
    /// </summary>
    public static void CallListenerDDown()
    {
        _actionOnDDown?.Invoke();
    }

    /// <summary>
    /// 按Esc事件触发
    /// </summary>
    public static void CallListenerEscDown()
    {
        _actionOnEscDown?.Invoke();
    }
}