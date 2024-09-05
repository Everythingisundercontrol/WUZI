using System;
using System.Collections.Generic;
using UnityEngine;

// public enum ChessType
// {
//     None,
//     White,
//     Black,
// }

public class AddChess
{
    //功能：
    //平局
    //对局结束后的处理
    //胜利显示，显示谁胜利
    //重新开始

    //细节：
    //public和private的字段命名
    //字段的含义nFloor,position1
    //代码复用
    //函数拆分
    //public private声明的func的上下文顺序

    //oop
    //魔数,枚举enum定义chessType


    public int StepCount; //第几步，用以判断当前是下的白棋还是黑棋
    public readonly int BoardHalfRows; //棋盘一个象限内行列数
    public readonly float BoardCellLengthX; //棋盘格子X边长
    public readonly float BoardCellLengthY; //棋盘格子X边长


    // private class Chess
    // {
    //     public ChessType chessType;
    //     public int stepIndex;
    // }
    //

    //
    // public void OnInit()
    // {
    //     _chessPositions = new Chess[15, 15];
    //     for (var x = 0; x < 15; x++)
    //     {
    //         for (var y = 0; y < 15; y++)
    //         {
    //             var chess = new Chess();
    //             chess.chessType = ChessType.None;
    //             //…………属性初始化
    //         }
    //     }
    // }


    private int[,] _chessPositions; //二维棋盘位置，索引指位置，值指无(-1)，黑(0)，白(1)
    private readonly int _boardRows; //棋盘总行列数
    // private int _continuousCount; //该点的某一轴上连续的棋子数
    private bool _exitLoop; //用以判断是否需要退出for循环

    /// <summary>
    /// 构造函数，初始化
    /// </summary>
    /// <param name="inputVector"></param>
    /// <param name="inputVector1"></param>
    public AddChess(Vector2 inputVector, Vector2 inputVector1)
    {
        _exitLoop = false;
        var x = Math.Abs(inputVector.x - inputVector1.x);
        var y = Math.Abs(inputVector.y - inputVector1.y);
        _boardRows = 15;
        BoardHalfRows = (_boardRows - 1) / 2;
        _chessPositions = InitChessPositions(new int[_boardRows, _boardRows]);
        BoardCellLengthX = (float) (Math.Round(x) / (_boardRows - 1));
        BoardCellLengthY = (float) (Math.Round(y) / (_boardRows - 1));
    }

    /// <summary>
    /// 数据中添加棋子
    /// </summary>
    /// <param name="inputVector2"></param>
    public Vector2 AddChessPoint(Vector2 inputVector2, Vector2 position1, Vector2 position2)
    {
        if (inputVector2.x < position1.x - 0.5 || inputVector2.x > position2.x + 0.5 ||
            inputVector2.y < position1.y - 0.5 ||
            inputVector2.y > position2.y + 0.5)
        {
            return new Vector2(-1, -1);
        }

        inputVector2.x += BoardHalfRows;
        inputVector2.y += BoardHalfRows;

        var nearestChessPoint = NearestChessPoint(inputVector2);
        if (_chessPositions[(int) nearestChessPoint.x, (int) nearestChessPoint.y] != -1)
        {
            return new Vector2(-1, -1);
        }

        _chessPositions[(int) nearestChessPoint.x, (int) nearestChessPoint.y] = StepCount % 2;
        return nearestChessPoint;
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    public bool WinCheck(int x, int y)
    {
        return Horizontal(x, y) || Vertical(x, y) || Diagonal(x, y) || Antidiagonal(x, y);
    }

    /// <summary>
    /// 初始化设置棋盘点位值为-1
    /// </summary>
    private static int[,] InitChessPositions(int[,] inputInts)
    {
        for (var i = 0; i < inputInts.GetLength(0); i++)
        {
            for (var j = 0; j < inputInts.GetLength(1); j++)
            {
                inputInts[i, j] = -1;
            }
        }

        return inputInts;
    }

    /// <summary>
    /// 判断是否胜利，复用？
    /// </summary>
    /// <param name="continuousCount"></param>
    /// <returns></returns>
    private static bool WinCheckResult(int continuousCount)
    {
        return continuousCount >= 4;
    }

    /// <summary>
    /// 统计某一方向上连续的本色棋子数
    /// </summary>
    private int CalculateContinuousCount(int x, int y)
    {
        if (_chessPositions[x, y] == StepCount % 2)
        {
            return 1;
        }

        _exitLoop = true;
        return 0;
    }

    /// <summary>
    /// 水平方向
    /// </summary>
    private bool Horizontal(int x, int y)
    {
        var continuousCount = 0;
        //右方判断
        for (var i = x + 1; i < _boardRows; i++)
        {
            continuousCount += CalculateContinuousCount(i, y);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        //左方判断
        for (var i = x - 1; i >= 0; i--)
        {
            continuousCount += CalculateContinuousCount(i, y);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        return WinCheckResult(continuousCount);
    }

    /// <summary>
    /// 竖直方向
    /// </summary>
    private bool Vertical(int x, int y)
    {
        var continuousCount = 0;
        //上方判断
        for (var i = y + 1; i < _boardRows; i++)
        {
            continuousCount += CalculateContinuousCount(x, i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        //下方判断
        for (var i = y - 1; i >= 0; i--)
        {
            continuousCount += CalculateContinuousCount(x, i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        return WinCheckResult(continuousCount);
    }

    /// <summary>
    /// 对角线方向
    /// </summary>
    private bool Diagonal(int x, int y)
    {
        var continuousCount = 0;
        //左上方判断
        if (x != 0 && y != _boardRows - 1)
        {
            continuousCount += DiagonalUpCheck(x, y);
        }

        //右下方判断
        if (x == _boardRows - 1 || y == 0)
        {
            return WinCheckResult(continuousCount);
        }

        continuousCount += DiagonalDownCheck(x, y);

        return WinCheckResult(continuousCount);
    }

    /// <summary>
    /// 对角线左上方判断
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int DiagonalUpCheck(int x, int y)
    {
        var continuousCount = 0;
        for (var i = 1; i <= x && i <= _boardRows - 1 - y; i++)
        {
            continuousCount += CalculateContinuousCount(x - i, y + i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            return continuousCount;
        }

        return 0;
    }

    /// <summary>
    /// 对角线右下方判断
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int DiagonalDownCheck(int x, int y)
    {
        var continuousCount = 0;
        for (var i = 1; i <= _boardRows - 1 - x && i <= y; i++)
        {
            continuousCount += CalculateContinuousCount(x + i, y - i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            return continuousCount;
        }

        return 0;
    }

    /// <summary>
    /// 反对角线方向
    /// </summary>
    private bool Antidiagonal(int x, int y)
    {
        var continuousCount = 0;
        //右上方判断
        if (x != _boardRows - 1 && y != _boardRows - 1)
        {
            continuousCount += AntidiagonalUpCheck(x, y);
        }

        //左下方判断
        if (x == 0 || y == 0)
        {
            return WinCheckResult(continuousCount);
        }

        continuousCount += AntidiagonalDownCheck(x, y);
        return WinCheckResult(continuousCount);
    }

    /// <summary>
    /// 反对角线左上方
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int AntidiagonalUpCheck(int x, int y)
    {
        var continuousCount = 0;

        for (var i = 1; i <= _boardRows - 1 - x && i <= _boardRows - 1 - y; i++)
        {
            continuousCount += CalculateContinuousCount(x + i, y + i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            return continuousCount;
        }

        return 0;
    }

    /// <summary>
    /// 反对角线右下方
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int AntidiagonalDownCheck(int x, int y)
    {
        var continuousCount = 0;

        for (var i = 1; i <= x && i <= y; i++)
        {
            continuousCount += CalculateContinuousCount(x - i, y - i);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            return continuousCount;
        }

        return 0;
    }

    /// <summary>
    /// 返回最近的点
    /// </summary>
    /// <param name="inputVector2"></param>
    /// <returns></returns>
    private Vector2 NearestChessPoint(Vector2 inputVector2)
    {
        return new Vector2(FindClosestMultiple(inputVector2.x, 0), FindClosestMultiple(inputVector2.y, 1));
    }

    /// <summary>
    /// 判断离x最近的boardCellLength整倍数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int FindClosestMultiple(float x, int k)
    {
        float boardCellLength = 0;
        switch (k)
        {
            case 0:
            {
                boardCellLength = BoardCellLengthX;
                break;
            }
            case 1:
            {
                boardCellLength = BoardCellLengthY;
                break;
            }
        }

        var floorIndex = Math.Floor(x / boardCellLength);
        var ceilIndex = Math.Ceiling(x / boardCellLength);

        var distance2Floor = Math.Abs(boardCellLength * floorIndex - x);
        var distance2Ceil = Math.Abs(boardCellLength * ceilIndex - x);

        return distance2Floor <= distance2Ceil ? (int) floorIndex : (int) ceilIndex;
    }
}