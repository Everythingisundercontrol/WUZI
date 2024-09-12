using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
    public GameObject chess;
    public Sprite black;
    public Sprite white;

    public GameObject boardTopRight;
    public GameObject boardBottomLeft;


    public GameObject gameOverPanel;
    public Text winnerText;

    public Vector2 vectorBoardTopRight;
    public Vector2 vectorBoardBottomLeft;


    private BaseFSM _baseFsm;

    private Model _model;

    private List<GameObject> _prefabHistory;

    private List<GameObject> _chessPrefabPool;

    private static Manager _manager;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        if (_manager == null)
        {
            _manager = this;
        }

        gameOverPanel.SetActive(false); //游戏开始时不显示

        _prefabHistory = new List<GameObject>();
        _chessPrefabPool = new List<GameObject>();

        InitListener();
        InitModel();
        InitFsm();
    }

    /// <summary>
    /// 帧
    /// </summary>
    private void Update()
    {
        if (gameOverPanel.activeSelf)
        {
            return;
        }

        _baseFsm.OnUpdate();
    }

    /// <summary>
    /// 左键悔棋
    /// </summary>
    public void LeftMouseRetract()
    {
        if (_prefabHistory.Count == 0)
        {
            return;
        }

        _model.DeleteChess();
        _model.StepCount--;
        ReturnChessPrefab(_prefabHistory[_prefabHistory.Count - 1]);
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
            _model.AddChessPoint(clickScreenPointVector2, vectorBoardBottomLeft, vectorBoardTopRight);

        if (addChessPoint.x < 0 || addChessPoint.y < 0)
        {
            return;
        }

        _model.ChessHistory.Add(addChessPoint);
        AddChessPrefab(addChessPoint);
        EndCheck(addChessPoint);

        _model.StepCount++;
    }

    /// <summary>
    /// 初始化listener
    /// </summary>
    private void InitListener()
    {
        InstanceTest.AddListenerADown(OnKeyboardADown);
        InstanceTest.AddListenerDDown(OnKeyboardDDown);
        InstanceTest.AddListenerEscDown(OnKeyboardEscDown);
    }

    /// <summary>
    /// 初始化model
    /// </summary>
    private void InitModel()
    {
        var position = boardTopRight.transform.position;
        vectorBoardTopRight =
            new Vector2(position.x, position.y);
        var position1 = boardBottomLeft.transform.position;
        vectorBoardBottomLeft =
            new Vector2(position1.x, position1.y);

        _model = new Model(vectorBoardTopRight, vectorBoardBottomLeft);
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    private void InitFsm()
    {
        _baseFsm = new BaseFSM();
        var dictionary = new Dictionary<FsmStateEnum, FsmState>
        {
            {FsmStateEnum.Play, new PlayState()},
            {FsmStateEnum.Retract, new RetractState()}
        };
        _baseFsm.SetFsm(dictionary);
        _baseFsm.ChangeFsmState(FsmStateEnum.Play, _manager);
    }

    /// <summary>
    /// 键盘按下esc，切换gameOverPanel关闭或开启
    /// </summary>
    private void OnKeyboardEscDown()
    {
        if (gameOverPanel.activeSelf)
        {
            Continue();
            return;
        }
        Stop();
    }

    /// <summary>
    /// 键盘输入A,切换状态机状态为下棋
    /// </summary>
    private void OnKeyboardADown()
    {
        _baseFsm.ChangeFsmState(FsmStateEnum.Play, _manager);
    }

    /// <summary>
    /// 键盘输入D，切换状态机状态为悔棋
    /// </summary>
    private void OnKeyboardDDown()
    {
        _baseFsm.ChangeFsmState(FsmStateEnum.Retract, _manager);
    }


    /// <summary>
    /// 从List中获取棋子
    /// </summary>
    /// <param name="chessPosition"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private GameObject GetChess(Vector3 chessPosition, Sprite sprite)
    {
        foreach (var chessInPool in _chessPrefabPool)
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
    /// 设置chess属性
    /// </summary>
    /// <param name="inputChess"></param>
    /// <param name="chessPosition"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private GameObject SetChessAttributes(GameObject inputChess, Vector3 chessPosition, Sprite sprite)
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
        _chessPrefabPool.Add(newChessObj);
        return newChessObj;
    }

    /// <summary>
    /// 向List中归还棋子
    /// </summary>
    /// <param name="obj"></param>
    private void ReturnChessPrefab(GameObject obj)
    {
        if (_chessPrefabPool.Contains(obj))
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 结局检查
    /// </summary>
    /// <param name="addChessPoint"></param>
    private void EndCheck(Vector2 addChessPoint)
    {
        var winOrLoss = _model.WinCheck((int) addChessPoint.x, (int) addChessPoint.y);

        if (winOrLoss)
        {
            Win();
        }

        if (_model.StepCount == 224)
        {
            Draw();
        }
    }

    /// <summary>
    /// 添加棋子预制
    /// </summary>
    private void AddChessPrefab(Vector2 chess)
    {
        var position = new Vector3(vectorBoardBottomLeft.x + chess.x * _model.Borad.BoardCellLengthX,
            vectorBoardBottomLeft.y + chess.y * _model.Borad.BoardCellLengthY, 1);

        if (_model.StepCount % 2 == 0)
        {
            _prefabHistory.Add(GetChess(position, black));
        }

        if (_model.StepCount % 2 == 1)
        {
            _prefabHistory.Add(GetChess(position, white));
        }
    }


    /// <summary>
    /// 胜利的动作
    /// </summary>
    private void Win()
    {
        winnerText.text = (_model.StepCount % 2 == 0 ? "black" : "white") + " Wins!";
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