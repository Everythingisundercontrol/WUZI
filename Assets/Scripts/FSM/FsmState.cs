public interface FsmState
{
    void OnEnter(Manager manager);

    void OnUpdate();

    void OnExit();

}