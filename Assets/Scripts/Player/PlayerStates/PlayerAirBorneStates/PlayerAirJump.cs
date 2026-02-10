using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerAirJump : State
    {
        readonly PlayerContext Ctx;
        float jumpHorizontalSpeed;
        bool jumpCutDone; //Flag to manage JumpCut
        bool isJumping; //If true -> normal gravity; if false -> endJumpGravity

        public PlayerAirJump(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;

            Add(new ColorPhaseActivity(Ctx.capsuleRenderer)
            {
                enterColor = new Color(173f / 255f, 216f / 255f, 230f / 255f), // Initializing ColorPhaseActivity enterColor field
                exitColor = new Color(173f / 255f, 216f / 255f, 230f / 255f)
            });
        }

        //Manages state change
        protected override State GetTransition()
        {
            //ChangeState: AirJump->AirJump. Avaiable only when AirJumpUsed < MaxAirJump
            if (Ctx.JumpBufferTimer > 0)
            {
                //CAN'T! NEED AN AIRJUMP2 state because State Machine blocks From State A to State A
                // if (Ctx.CanJump() == "airJump") return ((Airborne)Parent).AirJump;
            }
            //ChangeState: AirJump->Fall
            return (Ctx.Velocity.y <= 0f) ? ((PlayerAirborne)Parent).PlayerFall : null;
        }

        protected override void OnEnter()
        {
            Ctx.AirJumpsUsed += 1;
            Ctx.Velocity.y = Ctx.jumpConfig.jumpForce * Ctx.jumpConfig.airJumpForceMultiplier; //Air Jump can have a different JumpForce
            Ctx.CurrentJumpHoldTime = 0f;//Reset Jump Hold Time because jump is starting now
            jumpCutDone = false; //Flag to manage JumpCut
            isJumping = true; ////If true -> normal gravity; if false -> endJumpGravity
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
                Ctx.Velocity.y = Ctx.Velocity.y * Ctx.jumpConfig.jumpCutVelocityMult; //Jump Cut
                jumpCutDone = true;//Flag to avoid multiple Jump Cut
                isJumping = false; //After Jump Cut, switch to stronger gravity
            }
            #endregion

            #region Gravity
            //During ascension before jump hold time expires or before jump cut
            if (Ctx.Velocity.y > 0 && isJumping)
            {
                //Normal Gravity
                Ctx.Velocity.y += Physics.gravity.y * Time.deltaTime;
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