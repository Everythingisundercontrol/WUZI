using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    private void Start()
    {
        var restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
        
        var retractButton = GameObject.Find("RetractButton").GetComponent<Button>();
        retractButton.onClick.AddListener(Retract);
    }

    /// <summary>
    /// 悔棋
    /// </summary>
    private void Retract()
    {
        
    }

    /// <summary>
    /// 重载游戏
    /// </summary>
    private static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); // 退出游戏
    }
}
