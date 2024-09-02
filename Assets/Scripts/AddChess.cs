using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChess
{
    //名字

    private int[,] _chessPositions;
    private int _boardRows;
    private int _boardHalfRows;
    private float _boardLength;
    private double _boardCellLength;
    private int _stepCount;

    private void Start()
    {
        _boardLength = 5;
        _boardCellLength = 0.7;
        _boardRows = 15;
        _boardHalfRows = (_boardRows - 1) / 2;
        _chessPositions = new int[_boardRows, _boardRows];
        SetChessPositions();
    }

    private void Update()
    {
        InputCheck();
    }

    /// <summary>
    /// 输入检测
    /// </summary>
    private void InputCheck()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (!Camera.main)
        {
            return;
        }
        
        var clickScreenPointVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var clickScreenPointVector2 = new Vector2(clickScreenPointVector3.x, clickScreenPointVector3.y);
        AddChessPoint(clickScreenPointVector2);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputVector2"></param>
    private void AddChessPoint(Vector2 inputVector2)
    {
        if (inputVector2.x > _boardLength || inputVector2.x < -_boardLength || inputVector2.y > _boardLength ||
            inputVector2.y < -_boardLength)
        {
            return;
        }
        var nearestChessPoint = NearestChessPoint(inputVector2);
        if (CheckChessPointValue(nearestChessPoint)!=-1)
        {
            return;
        }

        _chessPositions[(int) nearestChessPoint.x, (int) nearestChessPoint.y] = _stepCount % 2;
        
        //加个添加棋子对象函数()
        
        WinCheck(nearestChessPoint);
    }

    /// <summary>
    /// 初始化设置棋盘点位值为-1
    /// </summary>
    private void SetChessPositions()
    {
        for (var i = 0; i < _chessPositions.Length; i++)
        {
            for (var j = 0; j < _chessPositions.Length; j++)
            {
                _chessPositions[i, j] = -1;
            }
        }
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    private void WinCheck(Vector2 inputVector2)
    {
        
    }

    /// <summary>
    /// 水平方向
    /// </summary>
    private void Horizontal()
    {
        
    }

    /// <summary>
    /// 对角线方向
    /// </summary>
    private void Diagonal()
    {
        
    }

    /// <summary>
    /// 反对角线方向
    /// </summary>
    private void Antidiagonal()
    {
        
    }

    /// <summary>
    /// 竖直方向
    /// </summary>
    private void Vertical()
    {
        
    }
    
    /// <summary>
    /// 检查棋盘点位的值
    /// </summary>
    /// <param name="inputVector2"></param>
    /// <returns></returns>
    private int CheckChessPointValue(Vector2 inputVector2)
    {
        return _chessPositions[(int) inputVector2.x,(int) inputVector2.y];
    }
    
    /// <summary>
    /// 返回最近的点
    /// </summary>
    /// <param name="inputVector2"></param>
    /// <returns></returns>
    private Vector2 NearestChessPoint(Vector2 inputVector2)
    {
        return new Vector2(FindClosestMultiple(inputVector2.x),FindClosestMultiple(inputVector2.y));
    }

    /// <summary>
    /// 判断离x最近的boardCellLength整倍数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int FindClosestMultiple(double x)
    {
        
        var nFloor = Math.Floor(x / _boardCellLength);
        var nCeil = Math.Ceiling(x / _boardCellLength);
        
        var distFloor = Math.Abs(_boardCellLength * nFloor - x);
        var distCeil = Math.Abs(_boardCellLength * nCeil - x);
        
        if (distFloor <= distCeil)
        {
            return (int)nFloor;
        }
        return (int)nCeil;
    }
}
