using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLogic : MonoBehaviour
{
    public delegate void MouseScrollDelegate(float delta);
    public static event MouseScrollDelegate mouseScrollEvent;
    public delegate void MouseDragDelegate(Vector3 delta);
    public static event MouseDragDelegate mouseDragEvent;

    public static bool CtrlDown = false;
    public static bool SelectAreaActive = false;

    private Vector3 lastMousePosition;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            CtrlDown = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            CtrlDown = false;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            mouseScrollEvent?.Invoke(Input.mouseScrollDelta.y);
        }
        if (CtrlDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                lastMousePosition = Input.mousePosition;
                mouseDragEvent?.Invoke(delta);
            }
        }
    }
}
