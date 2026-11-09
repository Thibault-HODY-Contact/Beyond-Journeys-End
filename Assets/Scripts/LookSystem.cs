using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LookSystem : InputAxisControllerBase<LookSystem.Reader>
{
    [Header("Input Source Override")]
    public PlayerInput PlayerInput;

    void Awake()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // When the PlayerInput receives an input, send it to all the controllers
        if (PlayerInput == null)
            TryGetComponent(out PlayerInput);
        if (PlayerInput == null)
            Debug.LogError("Cannot find PlayerInput component");
        else
        {
            PlayerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            PlayerInput.onActionTriggered += (value) =>
            {
                for (var i = 0; i < Controllers.Count; i++)
                    Controllers[i].Input.ProcessInput(value.action);
            };
        }
    }

    // We process user input on the Update clock
    void Update()
    {
        if (Application.isPlaying)
            UpdateControllers();
    }

    // Controllers will be instances of this class.
    [Serializable]
    public class Reader : IInputAxisReader
    {
        public InputActionReference Input;
        InputAction m_Action;
        Vector2 m_Value; // the cached value of the input
        float MouseRatio = 0.0005f;
        public float m_Scale = 10;

        public void ProcessInput(InputAction action)
        {
            // If it's my action then cache the new value
            if (Input != null && Input.action.id == action.id)
            {
                m_Action = action;
                if (action.expectedControlType == "Vector2")
                {
                    m_Value = action.ReadValue<Vector2>();
                    //m_Value.x = Mathf.Clamp(m_Value.x, -1, 1)* m_Scale;
                    //m_Value.y = Mathf.Clamp(m_Value.y, -1, 1) * m_Scale;


                }
                else
                {
                    m_Value.x = m_Value.y = action.ReadValue<float>();
                }
            }
        }

        // IInputAxisReader interface: Called by the framework to read the input value
        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
        {
            if (m_Action == null) return 0;
            float x=0;
             float y=0;

            if (m_Action.activeControl?.device is Mouse)
            {
                x = m_Action.ReadValue<Vector2>().x / Time.deltaTime * MouseRatio * m_Scale;
                 y = m_Action.ReadValue<Vector2>().y / Time.deltaTime * MouseRatio * m_Scale;

            }

            if (m_Action.activeControl?.device is Gamepad)
            {
                x = m_Action.ReadValue<Vector2>().x * m_Scale;
                y = m_Action.ReadValue<Vector2>().y * m_Scale;
                Debug.Log("manette");
            }

            return (hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? y : x);
        }
    }
}