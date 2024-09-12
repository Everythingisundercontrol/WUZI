using System;
using System.Collections.Generic;
using UnityEngine;

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

public class Model
{
    public readonly Board Borad;
    public int StepCount; //第几步，用以判断当前是下的白棋还是黑棋
    public readonly List<Vector2> ChessHistory;

    private readonly Chess[,] _chessList; //二维棋盘位置，索引指位置，值指无(-1)，黑(0)，白(1)
    private bool _exitLoop; //用以判断是否需要退出for循环


    /// <summary>
    /// 构造函数，初始化
    /// </summary>
    /// <param name="inputVector"></param>
    /// <param name="inputVector1"></param>
    public Model(Vector2 inputVector, Vector2 inputVector1)
    {
        ChessHistory = new List<Vector2>();
        _exitLoop = false;
        var boardLengthX = Math.Abs(inputVector.x - inputVector1.x);
        var boardLengthY = Math.Abs(inputVector.y - inputVector1.y);
        Borad = new Board(15, boardLengthX, boardLengthY);
        _chessList = InitChessPositions(new Chess[Borad.BoardRows, Borad.BoardRows]);
    }

    /// <summary>
    /// 删除棋子
    /// </summary>
    public void DeleteChess()
    {
        if (ChessHistory.Count == 0)
        {
            return;
        }
        var x = ChessHistory[ChessHistory.Count - 1].x;
        var y = ChessHistory[ChessHistory.Count - 1].y;
        _chessList[(int) x, (int) y].chessType = ChessTypeEnum.None;
        ChessHistory.RemoveAt(ChessHistory.Count - 1);
    }

    /// <summary>
    /// 数据中添加棋子
    /// </summary>
    public Vector2 AddChessPoint(Vector2 inputVector2, Vector2 vectorBoardBottomLeft, Vector2 vectorBoardTopRight)
    {
  
        var inputValid = inputVector2.x < vectorBoardBottomLeft.x - 0.1 || inputVector2.x > vectorBoardTopRight.x + 0.1 ||
                   inputVector2.y < vectorBoardBottomLeft.y - 0.1 ||
                   inputVector2.y > vectorBoardTopRight.y + 0.1;
        if (inputValid)
        {
            return new Vector2(-1, -1);
        }

        var nearestChessPoint = NearestChessPoint(inputVector2, vectorBoardBottomLeft);
        var chess = _chessList[(int) nearestChessPoint.x, (int) nearestChessPoint.y];
        if (chess.chessType != ChessTypeEnum.None)
        {
            return new Vector2(-1, -1);
        }

        _chessList[(int) nearestChessPoint.x, (int) nearestChessPoint.y].chessType =
            StepCount % 2 == 0 ? ChessTypeEnum.Black : ChessTypeEnum.White;
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
    private static Chess[,] InitChessPositions(Chess[,] inputInts)
    {
        for (var i = 0; i < inputInts.GetLength(0); i++)
        {
            for (var j = 0; j < inputInts.GetLength(1); j++)
            {
                inputInts[i, j] = new Chess {chessType = ChessTypeEnum.None};
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
        if (_chessList[x, y].chessType == (StepCount % 2 == 0 ? ChessTypeEnum.Black : ChessTypeEnum.White))
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
        for (var i = x + 1; i < Borad.BoardRows; i++)
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
        for (var i = y + 1; i < Borad.BoardRows; i++)
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
        if (x != 0 && y != Borad.BoardRows - 1)
        {
            continuousCount += DiagonalUpCheck(x, y);
        }

        //右下方判断
        if (x == Borad.BoardRows - 1 || y == 0)
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
        for (var i = 1; i <= x && i <= Borad.BoardRows - 1 - y; i++)
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
        for (var i = 1; i <= Borad.BoardRows - 1 - x && i <= y; i++)
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
        if (x != Borad.BoardRows - 1 && y != Borad.BoardRows - 1)
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

        for (var i = 1; i <= Borad.BoardRows - 1 - x && i <= Borad.BoardRows - 1 - y; i++)
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
    private Vector2 NearestChessPoint(Vector2 inputVector2, Vector2 vectorBoardBottomLeft)
    {
        return new Vector2(FindClosestMultiple(inputVector2.x, 0, vectorBoardBottomLeft),
            FindClosestMultiple(inputVector2.y, 1, vectorBoardBottomLeft));
    }

    /// <summary>
    /// 判断离x最近的boardCellLength整倍数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int FindClosestMultiple(float x, int k, Vector2 vectorBoardBottomLeft)
    {
        float boardCellLength = 0;
        double bottomLeft = 0;
        switch (k)
        {
            case 0:
            {
                boardCellLength = Borad.BoardCellLengthX;
                bottomLeft = vectorBoardBottomLeft.x;
                break;
            }
            case 1:
            {
                boardCellLength = Borad.BoardCellLengthY;
                bottomLeft = vectorBoardBottomLeft.y;
                break;
            }
        }


        var floorIndex = Math.Floor((x - bottomLeft) / boardCellLength);
        var ceilIndex = Math.Ceiling((x - bottomLeft) / boardCellLength);

        var distance2Floor = Math.Abs(boardCellLength * floorIndex - (x - bottomLeft));
        var distance2Ceil = Math.Abs(boardCellLength * ceilIndex - (x - bottomLeft));
        return distance2Floor <= distance2Ceil ? (int) floorIndex : (int) ceilIndex;
    }
}