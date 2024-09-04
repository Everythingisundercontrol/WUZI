using UnityEngine;

namespace DefaultNamespace
{
    public class Manager : MonoBehaviour
    {
        public GameObject black;
        public GameObject white;

        public GameObject boardPositions1;
        public GameObject boardPositions2;

        public Vector2 vectorBoardPositions1;
        public Vector2 vectorBoardPositions2;

        private AddChess _addChess;


        private void Start()
        {
            vectorBoardPositions1 =
                new Vector2(boardPositions1.transform.position.x, boardPositions1.transform.position.y);
            vectorBoardPositions2 =
                new Vector2(boardPositions2.transform.position.x, boardPositions2.transform.position.y);
            _addChess = new AddChess(vectorBoardPositions1, vectorBoardPositions2);
            Debug.Log(vectorBoardPositions1 + ":::" + vectorBoardPositions2);
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
            Debug.Log("AAA:" + clickScreenPointVector2);
            var addChessPoint =
                _addChess.AddChessPoint(clickScreenPointVector2, vectorBoardPositions1, vectorBoardPositions2);
            if (addChessPoint.x < 0 || addChessPoint.y < 0)
            {
                return;
            }

            addChessPrefab((Vector2) addChessPoint);
            // Debug.Log("BBB:"+addChessPoint);
            var winOrLoss = _addChess.WinCheck((int) addChessPoint.x, (int) addChessPoint.y);
            if (winOrLoss)
            {
                Win();
            }
        }

        /// <summary>
        /// 添加棋子预制
        /// </summary>
        private void addChessPrefab(Vector2 chess)
        {
            Debug.Log("add:" + chess + " step:" + _addChess._stepCount);
            if (_addChess._stepCount % 2 == 0)
            {
                Instantiate(black,
                    new Vector3(chess.x - _addChess._boardHalfRows, chess.y - _addChess._boardHalfRows, 1),
                    Quaternion.identity);
            }

            if (_addChess._stepCount % 2 == 1)
            {
                Instantiate(white,
                    new Vector3(chess.x - _addChess._boardHalfRows, chess.y - _addChess._boardHalfRows, 1),
                    Quaternion.identity);
            }
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