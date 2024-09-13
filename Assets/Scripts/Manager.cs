using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
    public bool IfStop;

    public GameObject boardTopRight;
    public GameObject boardBottomLeft;

    public Vector2 vectorBoardTopRight;
    public Vector2 vectorBoardBottomLeft;

    private BaseFSM _baseFsm;

    private Model _model;
    private View _view;

    private List<GameObject> _prefabHistory;

    private static Manager _manager;
    
    private void Start()
    {
        _manager = this;

        IfStop = false;

        _prefabHistory = new List<GameObject>();

        InitListener();
        InitModel();
        InitView();
        InitFsm();
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
        _view.ReturnChessPrefab(_prefabHistory[_prefabHistory.Count - 1]);
        _prefabHistory.RemoveAt(_prefabHistory.Count - 1);
    }

    /// <summary>
    /// 左键下棋
    /// </summary>
    public void LeftMousePlay(Vector3 mousePosition)
    {
        if (!Camera.main)
        {
            return;
        }

        var clickScreenPointVector3 = Camera.main.ScreenToWorldPoint(mousePosition);
        var clickScreenPointVector2 = new Vector2(clickScreenPointVector3.x, clickScreenPointVector3.y);

        var addChessPoint =
            _model.AddChessPoint(clickScreenPointVector2, vectorBoardBottomLeft, vectorBoardTopRight);

        if (addChessPoint.x < 0 || addChessPoint.y < 0)
        {
            return;
        }

        _model.ChessHistory.Add(addChessPoint);
        AddChess(addChessPoint);
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
    /// 初始化view
    /// </summary>
    private void InitView()
    {
        _view = GameObject.Find("ViewManager").GetComponent<View>();
        if (_view == null)
        {
            Debug.Log("View null");
            return;
        }

        _view.ChessPrefabPool = new List<GameObject>();
        _view.GameOverPanel.SetActive(false); //游戏开始时不显示
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    private void InitFsm()
    {
        _baseFsm = new BaseFSM();
        var playState = new PlayState();
        var retractState = new RetractState();
        playState.OnInit(this);
        retractState.OnInit(this);
        var dictionary = new Dictionary<FsmStateEnum, FsmState>
        {
            {FsmStateEnum.Play, playState},
            {FsmStateEnum.Retract, retractState}
        };
        _baseFsm.SetFsm(dictionary);
        _baseFsm.ChangeFsmState(FsmStateEnum.Play, _manager);
    }

    /// <summary>
    /// 键盘按下esc，切换gameOverPanel关闭或开启
    /// </summary>
    private void OnKeyboardEscDown()
    {
        if (_view.GameOverPanel.activeSelf)
        {
            _view.Continue();
            IfStop = false;
            return;
        }

        _view.Stop();
        IfStop = true;
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
    /// 结局检查
    /// </summary>
    /// <param name="addChessPoint"></param>
    private void EndCheck(Vector2 addChessPoint)
    {
        var winOrLoss = _model.WinCheck((int) addChessPoint.x, (int) addChessPoint.y);

        if (winOrLoss)
        {
            _view.Win(_model.StepCount % 2 == 0);
        }

        if (_model.StepCount == 224)
        {
            _view.Draw();
        }
    }

    /// <summary>
    /// 添加棋子
    /// </summary>
    private void AddChess(Vector2 chess)
    {
        var position = new Vector3(vectorBoardBottomLeft.x + chess.x * _model.Borad.BoardCellLengthX,
            vectorBoardBottomLeft.y + chess.y * _model.Borad.BoardCellLengthY, 1);

        if (_model.StepCount % 2 == 0)
        {
            _prefabHistory.Add(_view.GetChess(position, _view.black));
        }

        if (_model.StepCount % 2 == 1)
        {
            _prefabHistory.Add(_view.GetChess(position, _view.white));
        }
    }
}