using System;
using UnityEngine;

public class AddChess
{
    public int _stepCount; //第几步，用以判断当前是下的白棋还是黑棋
    public readonly int _boardHalfRows; //棋盘一个象限内行列数


    private int[,] _chessPositions; //二维棋盘位置，索引指位置，值指无(-1)，黑(0)，白(1)
    private readonly int _boardRows; //棋盘总行列数
    private readonly float _boardLengthX; //棋盘图片x的长度
    private readonly float _boardLengthY; //棋盘图片x的长度
    private readonly double _boardCellLengthX; //棋盘格子X边长
    private readonly double _boardCellLengthY; //棋盘格子X边长
    private int _continuousCount; //该点的某一轴上连续的棋子数
    private bool _exitLoop; //用以判断是否需要退出for循环

    public AddChess(Vector2 inputVector, Vector2 inputVector1)
    {
        _exitLoop = false;
        var x = Math.Abs(inputVector.x - inputVector1.x);
        var y = Math.Abs(inputVector.y - inputVector1.y);
        _boardLengthX = x;
        _boardLengthY = y;
        _boardRows = 15;
        _boardHalfRows = (_boardRows - 1) / 2;
        _chessPositions = InitChessPositions(new int[_boardRows, _boardRows]);
        _boardCellLengthX = Math.Round(_boardLengthX) / (_boardRows - 1);
        _boardCellLengthY = Math.Round(_boardLengthY) / (_boardRows - 1);
    }

    // public AddChess()
    // {
    //     _exitLoop = false;
    //     _boardLengthX = _boardLengthY = 5;
    //     _boardCellLength = 0.7;
    //     _boardRows = 15;
    //     // _boardHalfRows = (_boardRows - 1) / 2;
    //     _chessPositions = InitChessPositions(new int[_boardRows, _boardRows]);
    // }

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

        inputVector2.x += _boardHalfRows;
        inputVector2.y += _boardHalfRows;

        var nearestChessPoint = NearestChessPoint(inputVector2);
        Debug.Log("near:" + nearestChessPoint);
        if (_chessPositions[(int) nearestChessPoint.x, (int) nearestChessPoint.y] != -1)
        {
            return new Vector2(-1, -1);
        }

        _chessPositions[(int) nearestChessPoint.x, (int) nearestChessPoint.y] = _stepCount % 2;
        return nearestChessPoint;
    }

    /// <summary>
    /// 统计某一方向上连续的本色棋子数
    /// </summary>
    private void CalculateContinuousCount(int x, int y)
    {
        if (_chessPositions[x, y] == _stepCount % 2)
        {
            _continuousCount++;
        }

        if (_chessPositions[x, y] != _stepCount % 2)
        {
            _exitLoop = true;
        }
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    public bool WinCheck(int x, int y)
    {
        Horizontal(x, y);
        if (_continuousCount >= 4)
        {
            return true;
        }

        _continuousCount = 0;

        Vertical(x, y);
        if (_continuousCount >= 4)
        {
            return true;
        }

        _continuousCount = 0;

        Diagonal(x, y);
        // Debug.Log(_continuousCount);
        if (_continuousCount >= 4)
        {
            return true;
        }

        _continuousCount = 0;

        Antidiagonal(x, y);
        if (_continuousCount >= 4)
        {
            return true;
        }

        _continuousCount = 0;
        _stepCount++;
        return false;
    }

    /// <summary>
    /// 水平方向
    /// </summary>
    private void Horizontal(int x, int y)
    {
        //右方判断
        for (var k = x + 1; k < _boardRows; k++)
        {
            CalculateContinuousCount(k, y);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        //左方判断
        for (var k = x - 1; k >= 0; k--)
        {
            CalculateContinuousCount(k, y);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }
    }

    /// <summary>
    /// 竖直方向
    /// </summary>
    private void Vertical(int x, int y)
    {
        //上方判断
        for (var k = y + 1; k < _boardRows; k++)
        {
            CalculateContinuousCount(x, k);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }

        //下方判断
        for (var k = y - 1; k >= 0; k--)
        {
            CalculateContinuousCount(x, k);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }
    }

    /// <summary>
    /// 对角线方向
    /// </summary>
    private void Diagonal(int x, int y)
    {
        //左上方判断
        if (x != 0 && y != _boardRows - 1)
        {
            for (var k = 1; k <= x && k <= _boardRows - 1 - y; k++)
            {
                CalculateContinuousCount(x - k, y + k);
                if (!_exitLoop)
                {
                    continue;
                }

                _exitLoop = false;
                break;
            }
        }

        //右下方判断
        if (x == _boardRows - 1 || y == 0)
            return;
        for (var k = 1; k <= _boardRows - 1 - x && k <= y; k++)
        {
            CalculateContinuousCount(x + k, y - k);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }
    }

    /// <summary>
    /// 反对角线方向
    /// </summary>
    private void Antidiagonal(int x, int y)
    {
        //右上方判断
        if (x != _boardRows - 1 && y != _boardRows - 1)
        {
            for (var k = 1; k <= _boardRows - 1 - x && k <= _boardRows - 1 - y; k++)
            {
                CalculateContinuousCount(x + k, y + k);
                if (!_exitLoop)
                {
                    continue;
                }

                _exitLoop = false;
                break;
            }
        }

        //左下方判断
        if (x == 0 || y == 0)
            return;
        for (var k = 1; k <= x && k <= y; k++)
        {
            CalculateContinuousCount(x - k, y - k);
            if (!_exitLoop)
            {
                continue;
            }

            _exitLoop = false;
            break;
        }
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
    private int FindClosestMultiple(double x, int k)
    {
        double boardCellLength = 0;
        switch (k)
        {
            case 0:
            {
                boardCellLength = _boardCellLengthX;
                break;
            }
            case 1:
            {
                boardCellLength = _boardCellLengthY;
                break;
            }
        }


        var nFloor = Math.Floor(x / boardCellLength);
        var nCeil = Math.Ceiling(x / boardCellLength);

        var distFloor = Math.Abs(boardCellLength * nFloor - x);
        var distCeil = Math.Abs(boardCellLength * nCeil - x);

        if (distFloor <= distCeil)
        {
            return (int) nFloor;
        }

        return (int) nCeil;
    }
}