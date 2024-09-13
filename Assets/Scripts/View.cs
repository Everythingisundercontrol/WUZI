using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public GameObject chess;
    public Sprite black;
    public Sprite white;
    
    public List<GameObject> ChessPrefabPool;
    
    public GameObject GameOverPanel;
    private Text WinnerText;
    
    /// <summary>
    /// 胜利的动作
    /// </summary>
    public void Win(bool ifBlackWin)
    {
        WinnerText.text = (ifBlackWin ? "black" : "white") + " Wins!";
        GameOverPanel.SetActive(true);
    }

    /// <summary>
    /// 平局的动作
    /// </summary>
    public void Draw()
    {
        WinnerText.text = "Draw";
        GameOverPanel.SetActive(true);
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Stop()
    {
        WinnerText.text = "Stop";
        GameOverPanel.SetActive(true);
    }

    /// <summary>
    /// 继续
    /// </summary>
    public void Continue()
    {
        GameOverPanel.SetActive(false);
    }
    
    /// <summary>
    /// 从List中获取棋子
    /// </summary>
    /// <param name="chessPosition"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public GameObject GetChess(Vector3 chessPosition, Sprite sprite)
    {
        foreach (var chessInPool in ChessPrefabPool)
        {
            if (chessInPool.activeInHierarchy)
            {
                continue;
            }

            chessInPool.SetActive(true);
            return SetChessAttributes(chessInPool, chessPosition, sprite);
        }

        return CreateChess(chessPosition, sprite);
    }
    
    /// <summary>
    /// 向List中归还棋子
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnChessPrefab(GameObject obj)
    {
        if (ChessPrefabPool.Contains(obj))
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 设置chess属性
    /// </summary>
    /// <param name="inputChess"></param>
    /// <param name="chessPosition"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private static GameObject SetChessAttributes(GameObject inputChess, Vector3 chessPosition, Sprite sprite)
    {
        inputChess.transform.position = chessPosition;
        var spriteRenderer = inputChess.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }

        return inputChess;
    }

    /// <summary>
    /// 创建棋子
    /// </summary>
    private GameObject CreateChess(Vector3 chessPosition, Sprite sprite)
    {
        var newChessObj = SetChessAttributes(Instantiate(chess), chessPosition, sprite);
        ChessPrefabPool.Add(newChessObj);
        return newChessObj;
    }
    
}
