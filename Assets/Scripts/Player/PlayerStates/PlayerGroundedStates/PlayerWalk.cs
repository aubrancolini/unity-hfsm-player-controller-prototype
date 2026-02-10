using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerWalk : State
    {
        readonly PlayerContext Ctx;

        public PlayerWalk(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;

            Add(new ColorPhaseActivity(Ctx.capsuleRenderer) // Adding an activity to the state
            {
                enterColor = new Color(0f / 255f, 100f / 255f, 0f / 255f), // Initializing ColorPhaseActivity enterColor field
                exitColor = new Color(0f / 255f, 100f / 255f, 0f / 255f)
            });
        }

        protected override State GetTransition()
        {
            //ChangeState: Walk->Fall
            if (!Ctx.IsGrounded) return ((PlayerRoot)((PlayerGrounded)Parent).Parent).Airborne.PlayerFall;

            //ChangeState: Walk->Jump
            if (Ctx.JumpBufferTimer > 0) //True only if player pressed jump recently
            {
                var jumpType = Ctx.EvaluateJump();
                switch (jumpType)
                {
                    case JumpType.GroundJump:
                        Ctx.JumpBufferTimer = 0;
                        return ((PlayerRoot)Parent.Parent).Airborne.PlayerJump;
                }
            }

            //ChangeState: Walk->Idle
            return Mathf.Abs(Ctx.IntendedHorizontalDirection) <= 0.01f ? ((PlayerGrounded)Parent).PlayerIdle : null;

        }

        protected override void OnUpdate(float deltaTime)
        {
            Ctx.Velocity.x = Ctx.IntendedHorizontalDirection * Ctx.coreConfig.horizontalSpeed;
        }
    }
}