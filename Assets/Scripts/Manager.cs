using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Manager : MonoBehaviour
    {
        public GameObject black;
        public GameObject white;

        public GameObject boardTopRight;
        public GameObject boardBottomLeft;
        
        
        public GameObject gameOverPanel;
        public Text winnerText;

        public Vector2 vectorBoardTopRight;
        public Vector2 vectorBoardBottomLeft;
        

        private AddChess _addChess;


        private void Start()
        {
            gameOverPanel.SetActive(false);     //游戏开始时不显示
            
            vectorBoardTopRight =
                new Vector2(boardTopRight.transform.position.x, boardTopRight.transform.position.y);
            vectorBoardBottomLeft =
                new Vector2(boardBottomLeft.transform.position.x, boardBottomLeft.transform.position.y);
            
            _addChess = new AddChess(vectorBoardTopRight, vectorBoardBottomLeft);
        }

        private void Update()
        {
            KeyInputCheck();
            if (gameOverPanel.activeSelf)
            {
                return;
            }
            MouseInputCheck();
        }
        

        /// <summary>
        /// 按键输入检测
        /// </summary>
        private void KeyInputCheck()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            if (gameOverPanel.activeSelf)
            {
                
                Continue();
                return;
            }
            Stop();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 鼠标输入检测
        /// </summary>
        private void MouseInputCheck()
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
            var addChessPoint =
                _addChess.AddChessPoint(clickScreenPointVector2, vectorBoardBottomLeft, vectorBoardTopRight);
            if (addChessPoint.x < 0 || addChessPoint.y < 0)
            {
                return;
            }

            addChessPrefab((Vector2) addChessPoint);
            var winOrLoss = _addChess.WinCheck((int) addChessPoint.x, (int) addChessPoint.y);

            if (winOrLoss)
            {
                Win();
            }

            if (_addChess.StepCount == 224)
            {
                Draw();
            }

            _addChess.StepCount++;
        }

        /// <summary>
        /// 添加棋子预制
        /// </summary>
        private void addChessPrefab(Vector2 chess)
        {
            //_boardCellLengthX要乘吗？
            if (_addChess.StepCount % 2 == 0)
            {
                Instantiate(black,
                    new Vector3((float) ((chess.x - _addChess.Borad.BoardHalfRows) * _addChess.Borad.BoardCellLengthX),
                        (float) ((chess.y - _addChess.Borad.BoardHalfRows) * _addChess.Borad.BoardCellLengthY), 1),
                    Quaternion.identity);
            }

            if (_addChess.StepCount % 2 == 1)
            {
                Instantiate(white,
                    new Vector3((float) ((chess.x - _addChess.Borad.BoardHalfRows) * _addChess.Borad.BoardCellLengthX),
                        (float) ((chess.y - _addChess.Borad.BoardHalfRows) * _addChess.Borad.BoardCellLengthY), 1),
                    Quaternion.identity);
            }
        }


        /// <summary>
        /// 胜利的动作
        /// </summary>
        private void Win()
        {
            winnerText.text = (_addChess.StepCount % 2 == 0 ? "black" : "white") + " Wins!";
            gameOverPanel.SetActive(true);
        }

        /// <summary>
        /// 平局的动作
        /// </summary>
        private void Draw()
        {
            winnerText.text = "Draw";
            gameOverPanel.SetActive(true);
        }

        private void Stop()
        {
            winnerText.text = "Stop";
            gameOverPanel.SetActive(true);
        }

        private void Continue()
        {
            gameOverPanel.SetActive(false);
        }
    }
}