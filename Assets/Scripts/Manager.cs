using UnityEngine;

namespace DefaultNamespace
{
    public class Manager : MonoBehaviour
    {
        private AddChess addChess;

        private void Start()
        {
            addChess = new AddChess();
            
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
            addChess.AddChessPoint(clickScreenPointVector2);
        }
        

    }
}