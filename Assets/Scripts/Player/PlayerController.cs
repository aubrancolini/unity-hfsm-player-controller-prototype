using System;
using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {

        #region Runtime and configuration values
        [Header("Runtime values")]
        public PlayerContext PlayerContext; // Player runtime and configuration values shared between state machine states
        public PlayerContext Ctx => PlayerContext; //Ctx as an alias to leave PLayer Context title in the inspector

        [Header("Configuration: Scriptable Objects")]
        [SerializeField] private PlayerJumpConfig jumpConfig;
        [SerializeField] private PlayerCoreConfig coreConfig;
        #endregion

        #region Components
        private PlayerInputHandler input;
        private CharacterController characterController;   // manages movement
        #endregion

        #region State Machine
        StateMachine playerMachine;
        State playerRoot;
        #endregion

        #region Grounding
        [Header("Grounding")]
        public LayerMask GroundMask;
        public Transform GroundCheck;
        [SerializeField] private Vector3 groundBoxHalfExtents = new Vector3(0.4f, 0.1f, 0.4f);
        #endregion


        //Methods
        #region Methods

        /*Manages character facing. Facing direction is the same of Movement direction*/
        private void UpdateFacing(float direction)
        {

            if (direction > 0.01f && !Ctx.FacingRight)
            {
                FaceRight();
            }
            else if (direction < -0.01f && Ctx.FacingRight)
            {
                FaceLeft();
            }
        }

        private void FaceRight()
        {
            Ctx.FacingRight = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void FaceLeft()
        {
            Ctx.FacingRight = false;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        #endregion

        void Awake()
        {
            //Player Context
            PlayerContext = new PlayerContext(); // Ctx is playerContext alias
            Ctx.jumpConfig = jumpConfig;
            Ctx.coreConfig = coreConfig;
            Ctx.CurrentHorizontalSpeedMultiplier = Ctx.coreConfig.baseHorizontalSpeedMultiplier;
            Ctx.FacingRight = true;
            Ctx.MaxAirJumps = Ctx.jumpConfig.baseMaxAirJumps;
            Ctx.capsuleRenderer = GetComponentInChildren<MeshRenderer>();

            //Player State Machine
            playerRoot = new PlayerRoot(null, Ctx);
            var builder = new StateMachineBuilder(playerRoot);
            playerMachine = builder.Build();

            //Player components, manual dependecy injection
            input = GetComponent<PlayerInputHandler>(); //Manages inputs
            characterController = GetComponent<CharacterController>(); //Manages movement, collisions, etc
        }

        void Start()
        {

        }

        void Update()
        {
            //Global Timers
            Ctx.CoyoteTimer -= Time.deltaTime;
            Ctx.JumpBufferTimer = Mathf.Max(0f, Ctx.JumpBufferTimer - Time.deltaTime);

            //Horizontal direction and facing
            Ctx.IntendedHorizontalDirection = input.MoveInput.x;
            UpdateFacing(Ctx.IntendedHorizontalDirection);

            //Jump
            Ctx.JumpPressed = input.JumpPressed;
            Ctx.JumpHeld = input.JumpHeld;
            if (Ctx.JumpPressed)
            {
                Ctx.JumpBufferTimer = jumpConfig.jumpBufferTime;
            }

            //Applies movement to the character
            characterController.Move(Ctx.Velocity * Time.deltaTime);

            //Runs the TransitionSequencer
            playerMachine.Tick(Time.deltaTime);
        }

        void FixedUpdate()
        {
            //Returns true if character is on ground
            Ctx.IsGrounded = Physics.CheckBox(
                                GroundCheck.position,
                                groundBoxHalfExtents,
                                Quaternion.identity,
                                GroundMask,
                                QueryTriggerInteraction.Ignore);
        }
    }

    [Serializable]
    public class PlayerContext
    {
        //Runtime

        [Header("Core movement")]
        public Vector3 Velocity;
        public bool IsGrounded;
        public bool FacingRight;

        [Header("Horizontal Speed and Interpolation")]
        public float IntendedHorizontalDirection;
        public float CurrentHorizontalSpeedMultiplier;     // Interpolation result, 1 on ground

        [Header("Jump")]
        public bool JumpPressed;
        public bool JumpHeld;
        public float CurrentJumpHoldTime;
        public float CoyoteTimer;
        public float JumpBufferTimer;
        public int AirJumpsUsed;
        public int MaxAirJumps;

        //Configuration scriptable objects
        [NonSerialized] public PlayerCoreConfig coreConfig;
        [NonSerialized] public PlayerJumpConfig jumpConfig;

        [NonSerialized] public Renderer capsuleRenderer;

        /*When pressing jump button, it resets jumpBufferTimer.
        When jumpBufferTimer > 0, player tries to jump calling CanJump()
        So, don't check again jumpPressed and jumpBufferTimer > 0 inside this method.*/
        public JumpType EvaluateJump()
        {
            if (CoyoteTimer > 0f)
                return JumpType.GroundJump;

            if (AirJumpsUsed < MaxAirJumps)
                return JumpType.AirJump;

            return JumpType.None;
        }
    }
}