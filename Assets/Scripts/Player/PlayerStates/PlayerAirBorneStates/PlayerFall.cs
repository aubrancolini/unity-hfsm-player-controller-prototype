using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerFall : State
    {
        readonly PlayerContext Ctx;
        float fallHorizontalSpeed;

        public PlayerFall(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;

            Add(new ColorPhaseActivity(Ctx.capsuleRenderer)
            {
                enterColor = new Color(75f / 255f, 0f, 130f / 255f), // Initializing ColorPhaseActivity enterColor field
                exitColor = new Color(75f / 255f, 0f, 130f / 255f)
            });
        }

        //Manages state change
        protected override State GetTransition()
        {
            //ChangeState: Fall->Idle/Walk
            if (Ctx.IsGrounded)
            {
                if (Mathf.Abs(Ctx.IntendedHorizontalDirection) <= 0.01f)
                {
                    return ((PlayerRoot)Parent.Parent).PlayerGrounded.PlayerIdle;
                }
                else
                {
                    return ((PlayerRoot)Parent.Parent).PlayerGrounded.PlayerWalk;
                }
            }

            //ChangeState: Fall->Jump/AirJump
            if (Ctx.JumpBufferTimer > 0)  //Jump logic is started by JumpBufferTimer
            {
                var jumpType = Ctx.EvaluateJump();
                switch (jumpType)
                {
                    case JumpType.GroundJump:
                        Ctx.JumpBufferTimer = 0;
                        return ((PlayerRoot)Parent.Parent).Airborne.PlayerJump;

                    case JumpType.AirJump:
                        Ctx.JumpBufferTimer = 0;
                        return ((PlayerAirborne)Parent).PlayerAirJump;
                }
            }
            return null;
        }

        protected override void OnUpdate(float deltaTime)
        {
            //Horizontal velocity during fall
            fallHorizontalSpeed = Ctx.coreConfig.horizontalSpeed * Ctx.CurrentHorizontalSpeedMultiplier;
            Ctx.Velocity.x = Ctx.IntendedHorizontalDirection * fallHorizontalSpeed;

            #region Gravity
            // Enhanced gravity while falling
            Ctx.Velocity.y += Physics.gravity.y * Ctx.jumpConfig.fallGravityMultiplier * deltaTime;

            // Clamps falling speed
            if (Ctx.Velocity.y < Ctx.jumpConfig.maxFallSpeed)
            {
                Ctx.Velocity.y = Ctx.jumpConfig.maxFallSpeed;
            }
            #endregion
        }
    }
}