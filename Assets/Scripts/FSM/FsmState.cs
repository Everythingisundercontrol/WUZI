public interface FsmState
{
    void OnEnter();

    void OnUpdate();

    void OnExit();

}