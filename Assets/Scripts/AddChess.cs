using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChess : MonoBehaviour
{
    private List<Vector2> ChessPositionsV2;

    public List<Vector3> ChessPositionsV3;

    private float boardRadius = 4.9f;
    private double boardCellLength = 0.7;
    private Vector2 clickP2;
    private Vector3 clickP3;

    void Start()
    {
        ChessPositionsV2 = new List<Vector2>();
        ChessPositionsV3 = new List<Vector3>();
    }

    void Update()
    {
        InputCheck();
    }

    /// <summary>
    /// 
    /// </summary>
    private void InputCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("World:"+Camera.main.ScreenToWorldPoint(Input.mousePosition)+" Screen:"+Input.mousePosition);
            if (!(Camera.main is null)) clickP3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickP2 = new Vector2(clickP3.x, clickP3.y);
            // Debug.Log(clickP3.x+":::"+FindClosestMultiple((double)clickP3.x));
            if (clickP2.x<=boardRadius&&-boardRadius<=clickP2.x&&clickP2.y<=boardRadius&&-boardRadius<=clickP2.y)
            {
                NearestChesspoint(clickP2);
            }
        }

        
    }

    /// <summary>
    /// 返回最近的点
    /// </summary>
    /// <param name="inpV2"></param>
    /// <returns></returns>
    private Vector2 NearestChesspoint(Vector2 inpV2)
    {
        return new Vector2(FindClosestMultiple(inpV2.x),FindClosestMultiple(inpV2.y));
    }

    /// <summary>
    /// 判断最近的boardCellLength整倍数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int FindClosestMultiple(double x)
    {
        
        double nFloor = Math.Floor(x / boardCellLength);
        double nCeil = Math.Ceiling(x / boardCellLength);
        
        double distFloor = Math.Abs(boardCellLength * nFloor - x);
        double distCeil = Math.Abs(boardCellLength * nCeil - x);
        
        if (distFloor <= distCeil)
        {
            return (int)nFloor;
        }
        else
        {
            return (int)nCeil;
        }

    }
}
