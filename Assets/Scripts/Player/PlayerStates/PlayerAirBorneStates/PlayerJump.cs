using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerJump : State
    {
        readonly PlayerContext Ctx;
        float jumpHorizontalSpeed;
        bool jumpCutDone; //Flag to manage JumpCut
        bool isJumping; //If true -> normal gravity; if false -> endJumpGravity

        public PlayerJump(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;

            Add(new ColorPhaseActivity(Ctx.capsuleRenderer)
            {
                enterColor = Color.blue, // Initializing ColorPhaseActivity enterColor field   
                exitColor = Color.blue
            });
        }

        protected override State GetTransition()
        {
            //ChangeState: Jump->AirJump. Avaiable only when AirJumpUsed < MaxAirJump
            if (Ctx.JumpBufferTimer > 0)
            {
                var jumpType = Ctx.EvaluateJump();
                switch (jumpType)
                {
                    case JumpType.AirJump:
                        Ctx.JumpBufferTimer = 0;
                        return ((PlayerAirborne)Parent).PlayerAirJump;
                }
            }
            //ChangeState Jump->Fall
            return (Ctx.Velocity.y <= 0f) ? ((PlayerAirborne)Parent).PlayerFall : null;
        }

        protected override void OnEnter()
        {
            Ctx.Velocity.y = Ctx.jumpConfig.jumpForce;
            Ctx.CurrentJumpHoldTime = 0f;//Reset Jump Hold Time because jump is starting now
            Ctx.JumpBufferTimer = 0f;//Set jumpBuffer to 0 (reset it would be jumpBufferTimer = jumpBufferTime)

            jumpCutDone = false; //Flag to manage JumpCut
            isJumping = true; //If true -> normal gravity; if false -> endJumpGravity
        }


        protected override void OnUpdate(float deltaTime)
        {
            //Horizontal movement
            jumpHorizontalSpeed = Ctx.coreConfig.horizontalSpeed * Ctx.CurrentHorizontalSpeedMultiplier;
            Ctx.Velocity.x = Ctx.IntendedHorizontalDirection * jumpHorizontalSpeed;

            #region Jump Cut and Jump Hold Time
            if (Ctx.JumpHeld && isJumping)
            {
                Ctx.CurrentJumpHoldTime += deltaTime;
                if (Ctx.CurrentJumpHoldTime >= Ctx.jumpConfig.maxJumpHoldTime)
                {
                    isJumping = false;//After JumpHoldTime run out, switch to stronger gravity
                }
            }

            if (!Ctx.JumpHeld && !jumpCutDone)
            {
                Ctx.Velocity.y = Ctx.Velocity.y * Ctx.jumpConfig.jumpCutVelocityMult;//Jump Cut
                jumpCutDone = true;//Flag to avoid multiple Jump Cut
                isJumping = false; //After Jump Cut, switch to stronger gravity
            }
            #endregion

            #region Gravity
            //During ascension before jump hold time expires or before jump cut 
            if (Ctx.Velocity.y > 0 && isJumping)
            {
                //Normal Gravity
                Ctx.Velocity.y += Physics.gravity.y * deltaTime;
            }

            //During ascension after jump hold time expires or after jump cut
            else if (Ctx.Velocity.y > 0 && !isJumping)
            {
                //endJumpGravity
                Ctx.Velocity.y += Physics.gravity.y * Ctx.jumpConfig.endJumpGravityMultiplier * deltaTime;
            }
            #endregion
        }
    }
}