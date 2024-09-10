using System.Collections.Generic;
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


        private BaseFSM _baseFsm;

        private AddChess _addChess;

        private List<GameObject> _prefabHistory;

        private static Manager _manager;

        /// <summary>
        /// 右键悔棋,按钮还没做好，暂时先用右键去悔棋
        /// </summary>
        public void LeftMouseRetract()
        {
            if (_prefabHistory.Count == 0)
            {
                return;
            }

            _addChess.DeleteChess();
            Destroy(_prefabHistory[_prefabHistory.Count - 1]);
            _prefabHistory.RemoveAt(_prefabHistory.Count - 1);
        }

        /// <summary>
        /// 左键下棋
        /// </summary>
        public void LeftMousePlay()
        {
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

            _addChess.ChessHistory.Add(addChessPoint);
            addChessPrefab((Vector2) addChessPoint);
            EndCheck(addChessPoint);

            _addChess.StepCount++;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            gameOverPanel.SetActive(false); //游戏开始时不显示

            var position = boardTopRight.transform.position;
            vectorBoardTopRight =
                new Vector2(position.x, position.y);
            var position1 = boardBottomLeft.transform.position;
            vectorBoardBottomLeft =
                new Vector2(position1.x, position1.y);

            _addChess = new AddChess(vectorBoardTopRight, vectorBoardBottomLeft);

            //初始化状态机
            _baseFsm = new BaseFSM();
            var dictionary = new Dictionary<FsmStateEnum, FsmState>
            {
                {FsmStateEnum.Play, new Play()}, {FsmStateEnum.Retract, new Retract()}
            };
            _baseFsm.SetFsm(dictionary);
            _baseFsm.ChangeFsmState(FsmStateEnum.Play);

            _prefabHistory = new List<GameObject>();

            if (_manager == null)
            {
                _manager = this;
            }
        }

        /// <summary>
        /// 帧
        /// </summary>
        private void Update()
        {
            KeyInputCheck();
            if (gameOverPanel.activeSelf)
            {
                return;
            }

            // MouseInputCheck();
            
            _baseFsm.OnUpdate(_manager);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 按键输入检测
        /// </summary>
        private void KeyInputCheck()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _baseFsm.ChangeFsmState(FsmStateEnum.Play);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _baseFsm.ChangeFsmState(FsmStateEnum.Retract);
            }
            
            
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
            if (Input.GetMouseButtonDown(0))
            {
                // LeftMousePlay();
                _baseFsm.ChangeFsmState(FsmStateEnum.Play);
            }

            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }
            _baseFsm.ChangeFsmState(FsmStateEnum.Retract);
            // LeftMouseRetract();
        }

        private void EndCheck(Vector2 addChessPoint)
        {
            var winOrLoss = _addChess.WinCheck((int) addChessPoint.x, (int) addChessPoint.y);

            if (winOrLoss)
            {
                Win();
            }

            if (_addChess.StepCount == 224)
            {
                Draw();
            }
        }

        /// <summary>
        /// 添加棋子预制
        /// </summary>
        private void addChessPrefab(Vector2 chess)
        {
            if (_addChess.StepCount % 2 == 0)
            {
                _prefabHistory.Add(Instantiate(black,
                    new Vector3(vectorBoardBottomLeft.x + chess.x * _addChess.Borad.BoardCellLengthX,
                        vectorBoardBottomLeft.y + chess.y * _addChess.Borad.BoardCellLengthY, 1),
                    Quaternion.identity)
                );
            }

            if (_addChess.StepCount % 2 == 1)
            {
                _prefabHistory.Add(Instantiate(white,
                    new Vector3(vectorBoardBottomLeft.x + chess.x * _addChess.Borad.BoardCellLengthX,
                        vectorBoardBottomLeft.y + chess.y * _addChess.Borad.BoardCellLengthY, 1),
                    Quaternion.identity));
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

        /// <summary>
        /// 暂停
        /// </summary>
        private void Stop()
        {
            winnerText.text = "Stop";
            gameOverPanel.SetActive(true);
        }

        /// <summary>
        /// 继续
        /// </summary>
        private void Continue()
        {
            gameOverPanel.SetActive(false);
        }
    }
}