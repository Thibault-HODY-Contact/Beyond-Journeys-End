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
        Vector2 m_Value; 
        float MouseRatio = 0.0005f;
        public float m_Scale = 10;

        public void ProcessInput(InputAction action)
        {
            if (Input != null && Input.action.id == action.id)
            {
                m_Action = action;
                if (action.expectedControlType == "Vector2")
                {
                    m_Value = action.ReadValue<Vector2>();
                    


                }
                else
                {
                    m_Value.x = m_Value.y = action.ReadValue<float>();
                }
            }
        }

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