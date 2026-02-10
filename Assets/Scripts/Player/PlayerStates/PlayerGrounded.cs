using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerGrounded : State
    {
        readonly PlayerContext Ctx;
        public readonly PlayerIdle PlayerIdle; // Child states
        public readonly PlayerWalk PlayerWalk;

        public PlayerGrounded(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;
            PlayerIdle = new PlayerIdle(m, this, Ctx); // Istantiating child states
            PlayerWalk = new PlayerWalk(m, this, Ctx);

        }

        // Set active child when entering Grounded state depending on horizontal movement
        protected override State GetInitialState() => Mathf.Abs(Ctx.IntendedHorizontalDirection) <= 0.01f ? PlayerIdle : PlayerWalk;

        protected override void OnEnter()
        {
            Ctx.CurrentHorizontalSpeedMultiplier = 1f; //Reset HorizontalModifier (Interpolation) when on ground
            Ctx.AirJumpsUsed = 0; //Reset AirJumps used when on ground
            Ctx.CoyoteTimer = Ctx.jumpConfig.coyoteTime; //Reset Coyote OnEnter() or Jump Buffer won't work
            if (Ctx.Velocity.y < 0f) Ctx.Velocity.y = -0.1f; //Reset gravity when on ground
        }

        protected override void OnUpdate(float deltaTime)
        {
            Ctx.CoyoteTimer = Ctx.jumpConfig.coyoteTime; //Keep resetting Coyote Timer while on ground
        }
    }
}

