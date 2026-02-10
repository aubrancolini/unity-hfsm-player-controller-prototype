using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerAirborne : State
    {
        readonly PlayerContext Ctx;
        public readonly PlayerJump PlayerJump;
        public readonly PlayerFall PlayerFall;
        public readonly PlayerAirJump PlayerAirJump;

        public PlayerAirborne(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;
            PlayerJump = new PlayerJump(m, this, Ctx);
            PlayerFall = new PlayerFall(m, this, Ctx);
            PlayerAirJump = new PlayerAirJump(m, this, Ctx);
        }

        protected override State GetInitialState() => PlayerFall;

        protected override void OnUpdate(float deltaTime)
        {
            // Interpolation boost  jumpHorizontalModifier
            float currentHorizontalModifier = Mathf.Lerp(
                Ctx.CurrentHorizontalSpeedMultiplier,  //Current value
                Ctx.jumpConfig.jumpHorizontalMultiplierLimit, // End value
                Ctx.jumpConfig.jumpHorizontalMultiplierSpeed * deltaTime); // Interpolation speed
            Ctx.CurrentHorizontalSpeedMultiplier = currentHorizontalModifier;
        }
    }
}