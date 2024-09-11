using UnityEngine;

namespace DefaultNamespace.FSM
{
    public class PlayState : FsmState
    {
        public void OnEnter()
        {
            Debug.Log("Play");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Manager manager)
        {
            if (Input.GetMouseButtonDown(0))
            {
                manager.LeftMousePlay();
            }
        }

        public void OnExit()
        {
        }
    
    }
}