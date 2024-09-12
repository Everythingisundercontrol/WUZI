using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstanceTest.CallListenerADown();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            InstanceTest.CallListenerDDown();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InstanceTest.CallListenerEscDown();
        }

        if (Input.GetMouseButtonDown(0))
        {
            InstanceTest.CallListenerClickLeft();
        }
    }
}
