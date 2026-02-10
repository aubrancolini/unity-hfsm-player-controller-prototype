using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpConfig", menuName = "Scriptable Objects/PlayerJumpConfig")]
public class PlayerJumpConfig : ScriptableObject
{
    [Header("Jump")]
    [Tooltip("Higher values -> Higher Jump.")]
    public float jumpForce = 16f;
    [Tooltip("Modifies jump force for air jumps.")]
    public float airJumpForceMultiplier = 0.8f;
    [Tooltip("Higher values -> Can hold jump longer -> Higher jumps.")]
    public float maxJumpHoldTime = 0.15f;
    [Tooltip("Vertical velocity multiplier when releasing jump button allowing micro jump.")]
    public float jumpCutVelocityMult = 0.5f;
    [Tooltip("Grace period after on ground becomes false during which normal jump is allowed.")]
    public float coyoteTime = 0.12f;
    [Tooltip("Period during which jump imput is remembered.")]
    public float jumpBufferTime = 0.12f;
    [Tooltip("Number of air jumps at new game.")]
    public int baseMaxAirJumps = 1;
    [Tooltip("Horizontal speed multiplier Limit value during jump. Values < 0, horizontal deceleration during jump.")]
    public float jumpHorizontalMultiplierLimit = 0.8f; // Interpolation end value
    [Tooltip("Magnitude of the horizontal deceleration / aceleration during jump.")]
    public float jumpHorizontalMultiplierSpeed = 5f; // Horizontal interpolation speed    

    [Header("Gravity")]
    [Tooltip("Gravity multiplier after jump hold time expires or after jump cut.")]
    public float endJumpGravityMultiplier = 4f;
    [Tooltip("Gravity multiplier during fall state.")]
    public float fallGravityMultiplier = 8f;
    [Tooltip("Highest vertical velocity value during fall state.")]
    public float maxFallSpeed = -40f;
}
