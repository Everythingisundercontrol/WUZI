using UnityEngine;


public class Board
{
    // public readonly int BoardHalfRows; //棋盘一个象限内行列数
    public readonly float BoardCellLengthX; //棋盘格子X边长
    public readonly float BoardCellLengthY; //棋盘格子X边长
    public readonly int BoardRows; //棋盘总行列数

    /// <summary>
    /// 构造函数
    /// </summary>
    public Board(int boardRows, float boardLengthX, float boardLengthY)
    {
        BoardRows = boardRows;
        // BoardHalfRows = (BoardRows - 1) / 2;
        BoardCellLengthX = (float) (Mathf.Round(boardLengthX) / (BoardRows - 1));
        BoardCellLengthY = (float) (Mathf.Round(boardLengthY) / (BoardRows - 1));
    }
}