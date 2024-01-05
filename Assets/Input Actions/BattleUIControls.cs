//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Input Actions/BattleUIControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @BattleUIControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BattleUIControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BattleUIControls"",
    ""maps"": [
        {
            ""name"": ""BattleUINavigation"",
            ""id"": ""387379bc-4060-42b6-a0cd-776ea6523ff2"",
            ""actions"": [
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""df0d2eb9-e5fa-4546-9e4e-116df4ffb2c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""7d9cc381-0663-4a13-87ed-99799f596d3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Check"",
                    ""type"": ""Button"",
                    ""id"": ""846a3ee0-b630-4aad-a164-62f009c73714"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch"",
                    ""type"": ""Button"",
                    ""id"": ""6e8bbc6b-fb0a-4044-b278-0a3a357fe24a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""70550ddb-1fb9-424f-8db3-f916bf4c5700"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""d3ddafb6-eeda-4028-932d-9e9e65239503"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""dff96ae4-e311-44d9-a881-317a3dd66438"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""3f4464d1-324f-48df-a5c6-cf4b827366a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftPress"",
                    ""type"": ""Button"",
                    ""id"": ""26214aa8-e63f-4f56-a0c0-d37d4c8f1196"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateUp"",
                    ""type"": ""Button"",
                    ""id"": ""77e8720c-b54d-4a32-bec6-8a7fe216b23f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateDown"",
                    ""type"": ""Button"",
                    ""id"": ""643f201a-235e-4133-ae36-e69370172870"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateLeft"",
                    ""type"": ""Button"",
                    ""id"": ""6b8198b4-61e7-4b67-a42a-522afe4cefdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateRight"",
                    ""type"": ""Button"",
                    ""id"": ""c5ffb98b-1e10-4ec9-a6ea-df8597a91479"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PressRight"",
                    ""type"": ""Button"",
                    ""id"": ""434b50a5-ff4e-425f-b77f-9e9caa8ec8d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0001d4c-a169-4c24-b155-040355a3fec2"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3814db42-05b1-40f6-93bf-55ef5b035bef"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b1f0881-9ddd-4fa1-b42c-b514cbad7a63"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Check"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a58e8148-975d-4101-913d-c026d1536cf1"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df058a6a-2fd2-4b56-a5ca-17203dab455d"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75da1953-feb1-4217-94a2-635cc1205acc"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd262947-36a2-47cc-a41e-4a993f8df000"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34aae414-1687-4042-b125-e0fbe6122afb"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11c8bd55-efd4-4a92-88b1-c09f27ea5b8d"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d89b2c44-d922-44ae-b9ba-a7d93e66d436"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""491e0623-4244-48a1-bbc7-5c8fc6361f7a"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc3c5913-cd39-4e05-bb33-87e86eb97c1e"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8387ac7-94f5-4d43-a597-03719e94b8bf"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1101b2f3-33c7-4828-9969-53fef0357fb8"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PressRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BattleUINavigation
        m_BattleUINavigation = asset.FindActionMap("BattleUINavigation", throwIfNotFound: true);
        m_BattleUINavigation_Confirm = m_BattleUINavigation.FindAction("Confirm", throwIfNotFound: true);
        m_BattleUINavigation_Cancel = m_BattleUINavigation.FindAction("Cancel", throwIfNotFound: true);
        m_BattleUINavigation_Check = m_BattleUINavigation.FindAction("Check", throwIfNotFound: true);
        m_BattleUINavigation_Switch = m_BattleUINavigation.FindAction("Switch", throwIfNotFound: true);
        m_BattleUINavigation_MoveUp = m_BattleUINavigation.FindAction("MoveUp", throwIfNotFound: true);
        m_BattleUINavigation_MoveDown = m_BattleUINavigation.FindAction("MoveDown", throwIfNotFound: true);
        m_BattleUINavigation_MoveLeft = m_BattleUINavigation.FindAction("MoveLeft", throwIfNotFound: true);
        m_BattleUINavigation_MoveRight = m_BattleUINavigation.FindAction("MoveRight", throwIfNotFound: true);
        m_BattleUINavigation_LeftPress = m_BattleUINavigation.FindAction("LeftPress", throwIfNotFound: true);
        m_BattleUINavigation_RotateUp = m_BattleUINavigation.FindAction("RotateUp", throwIfNotFound: true);
        m_BattleUINavigation_RotateDown = m_BattleUINavigation.FindAction("RotateDown", throwIfNotFound: true);
        m_BattleUINavigation_RotateLeft = m_BattleUINavigation.FindAction("RotateLeft", throwIfNotFound: true);
        m_BattleUINavigation_RotateRight = m_BattleUINavigation.FindAction("RotateRight", throwIfNotFound: true);
        m_BattleUINavigation_PressRight = m_BattleUINavigation.FindAction("PressRight", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BattleUINavigation
    private readonly InputActionMap m_BattleUINavigation;
    private List<IBattleUINavigationActions> m_BattleUINavigationActionsCallbackInterfaces = new List<IBattleUINavigationActions>();
    private readonly InputAction m_BattleUINavigation_Confirm;
    private readonly InputAction m_BattleUINavigation_Cancel;
    private readonly InputAction m_BattleUINavigation_Check;
    private readonly InputAction m_BattleUINavigation_Switch;
    private readonly InputAction m_BattleUINavigation_MoveUp;
    private readonly InputAction m_BattleUINavigation_MoveDown;
    private readonly InputAction m_BattleUINavigation_MoveLeft;
    private readonly InputAction m_BattleUINavigation_MoveRight;
    private readonly InputAction m_BattleUINavigation_LeftPress;
    private readonly InputAction m_BattleUINavigation_RotateUp;
    private readonly InputAction m_BattleUINavigation_RotateDown;
    private readonly InputAction m_BattleUINavigation_RotateLeft;
    private readonly InputAction m_BattleUINavigation_RotateRight;
    private readonly InputAction m_BattleUINavigation_PressRight;
    public struct BattleUINavigationActions
    {
        private @BattleUIControls m_Wrapper;
        public BattleUINavigationActions(@BattleUIControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Confirm => m_Wrapper.m_BattleUINavigation_Confirm;
        public InputAction @Cancel => m_Wrapper.m_BattleUINavigation_Cancel;
        public InputAction @Check => m_Wrapper.m_BattleUINavigation_Check;
        public InputAction @Switch => m_Wrapper.m_BattleUINavigation_Switch;
        public InputAction @MoveUp => m_Wrapper.m_BattleUINavigation_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_BattleUINavigation_MoveDown;
        public InputAction @MoveLeft => m_Wrapper.m_BattleUINavigation_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_BattleUINavigation_MoveRight;
        public InputAction @LeftPress => m_Wrapper.m_BattleUINavigation_LeftPress;
        public InputAction @RotateUp => m_Wrapper.m_BattleUINavigation_RotateUp;
        public InputAction @RotateDown => m_Wrapper.m_BattleUINavigation_RotateDown;
        public InputAction @RotateLeft => m_Wrapper.m_BattleUINavigation_RotateLeft;
        public InputAction @RotateRight => m_Wrapper.m_BattleUINavigation_RotateRight;
        public InputAction @PressRight => m_Wrapper.m_BattleUINavigation_PressRight;
        public InputActionMap Get() { return m_Wrapper.m_BattleUINavigation; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleUINavigationActions set) { return set.Get(); }
        public void AddCallbacks(IBattleUINavigationActions instance)
        {
            if (instance == null || m_Wrapper.m_BattleUINavigationActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BattleUINavigationActionsCallbackInterfaces.Add(instance);
            @Confirm.started += instance.OnConfirm;
            @Confirm.performed += instance.OnConfirm;
            @Confirm.canceled += instance.OnConfirm;
            @Cancel.started += instance.OnCancel;
            @Cancel.performed += instance.OnCancel;
            @Cancel.canceled += instance.OnCancel;
            @Check.started += instance.OnCheck;
            @Check.performed += instance.OnCheck;
            @Check.canceled += instance.OnCheck;
            @Switch.started += instance.OnSwitch;
            @Switch.performed += instance.OnSwitch;
            @Switch.canceled += instance.OnSwitch;
            @MoveUp.started += instance.OnMoveUp;
            @MoveUp.performed += instance.OnMoveUp;
            @MoveUp.canceled += instance.OnMoveUp;
            @MoveDown.started += instance.OnMoveDown;
            @MoveDown.performed += instance.OnMoveDown;
            @MoveDown.canceled += instance.OnMoveDown;
            @MoveLeft.started += instance.OnMoveLeft;
            @MoveLeft.performed += instance.OnMoveLeft;
            @MoveLeft.canceled += instance.OnMoveLeft;
            @MoveRight.started += instance.OnMoveRight;
            @MoveRight.performed += instance.OnMoveRight;
            @MoveRight.canceled += instance.OnMoveRight;
            @LeftPress.started += instance.OnLeftPress;
            @LeftPress.performed += instance.OnLeftPress;
            @LeftPress.canceled += instance.OnLeftPress;
            @RotateUp.started += instance.OnRotateUp;
            @RotateUp.performed += instance.OnRotateUp;
            @RotateUp.canceled += instance.OnRotateUp;
            @RotateDown.started += instance.OnRotateDown;
            @RotateDown.performed += instance.OnRotateDown;
            @RotateDown.canceled += instance.OnRotateDown;
            @RotateLeft.started += instance.OnRotateLeft;
            @RotateLeft.performed += instance.OnRotateLeft;
            @RotateLeft.canceled += instance.OnRotateLeft;
            @RotateRight.started += instance.OnRotateRight;
            @RotateRight.performed += instance.OnRotateRight;
            @RotateRight.canceled += instance.OnRotateRight;
            @PressRight.started += instance.OnPressRight;
            @PressRight.performed += instance.OnPressRight;
            @PressRight.canceled += instance.OnPressRight;
        }

        private void UnregisterCallbacks(IBattleUINavigationActions instance)
        {
            @Confirm.started -= instance.OnConfirm;
            @Confirm.performed -= instance.OnConfirm;
            @Confirm.canceled -= instance.OnConfirm;
            @Cancel.started -= instance.OnCancel;
            @Cancel.performed -= instance.OnCancel;
            @Cancel.canceled -= instance.OnCancel;
            @Check.started -= instance.OnCheck;
            @Check.performed -= instance.OnCheck;
            @Check.canceled -= instance.OnCheck;
            @Switch.started -= instance.OnSwitch;
            @Switch.performed -= instance.OnSwitch;
            @Switch.canceled -= instance.OnSwitch;
            @MoveUp.started -= instance.OnMoveUp;
            @MoveUp.performed -= instance.OnMoveUp;
            @MoveUp.canceled -= instance.OnMoveUp;
            @MoveDown.started -= instance.OnMoveDown;
            @MoveDown.performed -= instance.OnMoveDown;
            @MoveDown.canceled -= instance.OnMoveDown;
            @MoveLeft.started -= instance.OnMoveLeft;
            @MoveLeft.performed -= instance.OnMoveLeft;
            @MoveLeft.canceled -= instance.OnMoveLeft;
            @MoveRight.started -= instance.OnMoveRight;
            @MoveRight.performed -= instance.OnMoveRight;
            @MoveRight.canceled -= instance.OnMoveRight;
            @LeftPress.started -= instance.OnLeftPress;
            @LeftPress.performed -= instance.OnLeftPress;
            @LeftPress.canceled -= instance.OnLeftPress;
            @RotateUp.started -= instance.OnRotateUp;
            @RotateUp.performed -= instance.OnRotateUp;
            @RotateUp.canceled -= instance.OnRotateUp;
            @RotateDown.started -= instance.OnRotateDown;
            @RotateDown.performed -= instance.OnRotateDown;
            @RotateDown.canceled -= instance.OnRotateDown;
            @RotateLeft.started -= instance.OnRotateLeft;
            @RotateLeft.performed -= instance.OnRotateLeft;
            @RotateLeft.canceled -= instance.OnRotateLeft;
            @RotateRight.started -= instance.OnRotateRight;
            @RotateRight.performed -= instance.OnRotateRight;
            @RotateRight.canceled -= instance.OnRotateRight;
            @PressRight.started -= instance.OnPressRight;
            @PressRight.performed -= instance.OnPressRight;
            @PressRight.canceled -= instance.OnPressRight;
        }

        public void RemoveCallbacks(IBattleUINavigationActions instance)
        {
            if (m_Wrapper.m_BattleUINavigationActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBattleUINavigationActions instance)
        {
            foreach (var item in m_Wrapper.m_BattleUINavigationActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BattleUINavigationActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BattleUINavigationActions @BattleUINavigation => new BattleUINavigationActions(this);
    public interface IBattleUINavigationActions
    {
        void OnConfirm(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnCheck(InputAction.CallbackContext context);
        void OnSwitch(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnLeftPress(InputAction.CallbackContext context);
        void OnRotateUp(InputAction.CallbackContext context);
        void OnRotateDown(InputAction.CallbackContext context);
        void OnRotateLeft(InputAction.CallbackContext context);
        void OnRotateRight(InputAction.CallbackContext context);
        void OnPressRight(InputAction.CallbackContext context);
    }
}