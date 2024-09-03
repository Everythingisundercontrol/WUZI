using UnityEngine;

namespace DefaultNamespace
{
    public class Manager : MonoBehaviour
    {
        private AddChess _addChess;

        private void Start()
        {
            _addChess = new AddChess();
            
        }

        private void Update()
        {
            InputCheck();
        }

        
        // ReSharper disable Unity.PerformanceAnalysis
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
            var addChessPoint= _addChess.AddChessPoint(clickScreenPointVector2);
            if (addChessPoint == null)
            {
                return;
            }
            //加个添加棋子对象函数()
            addChessPrefab((Vector2) addChessPoint);
            var WinOrLoss = _addChess.WinCheck((int) addChessPoint.Value.x, (int) addChessPoint.Value.y);
            if (WinOrLoss)
            {
                Win();
            }
        }

        /// <summary>
        /// 添加棋子预制
        /// </summary>
        private void addChessPrefab(Vector2 chess)
        {
            Debug.Log(chess.x+":::"+chess.y);
        }
    
        /// <summary>
        /// 胜利的动作
        /// </summary>
        private void Win()
        {
            Debug.Log("win");
        }
        
    }
}