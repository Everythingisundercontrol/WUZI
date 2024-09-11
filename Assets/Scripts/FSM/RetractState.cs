using UnityEngine;

namespace DefaultNamespace.FSM
{
    public class RetractState : FsmState
    {
        public void OnEnter()
        {
            Debug.Log("Retract");
        }

        public void OnUpdate(Manager manager)
        {
            if (Input.GetMouseButtonDown(0))
            {
                manager.LeftMouseRetract();
            }
        }

        public void OnExit()
        {
        }
    }
}