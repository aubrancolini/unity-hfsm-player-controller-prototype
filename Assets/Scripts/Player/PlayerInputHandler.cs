using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{ /*This class read player inputs from New Input System.*/

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;   // Value/Vector2
    [SerializeField] private InputActionReference jumpAction;   // Button
    [SerializeField] private InputActionReference sprintAction; // Button (Player/Sprint)
    [SerializeField] private InputActionReference dashAction;   // Button (Player/Dash)


    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }

    void OnEnable()
    {
        moveAction.action?.Enable();
        jumpAction.action?.Enable();
        sprintAction.action?.Enable();
        dashAction.action?.Enable();
    }

    void OnDisable()
    {
        moveAction.action?.Disable();
        jumpAction.action?.Disable();
        sprintAction.action?.Disable();
        dashAction.action?.Disable();
    }

    void Update()
    {   /*Questa funziona comunica continuamente al PlayerController
        quali tasti vengono premuti*/

        //wasd movement
        MoveInput = moveAction.action.ReadValue<Vector2>();
        //Returns true during the frame player presses jump
        JumpPressed = jumpAction.action.WasPressedThisFrame();
        //Returns true while player presses jump
        JumpHeld = jumpAction.action.IsPressed();
        //Returns true while player presses sprint
        SprintHeld = sprintAction.action.IsPressed();
        //Returns true during the frame player presses jump
        DashPressed = dashAction.action.WasPressedThisFrame();
    }
}