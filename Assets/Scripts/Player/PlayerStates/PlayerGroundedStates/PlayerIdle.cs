using UnityEngine;
using HSM;

namespace Game.Player
{
    public class PlayerIdle : State
    {
        readonly PlayerContext Ctx;

        public PlayerIdle(StateMachine m, State parent, PlayerContext Ctx) : base(m, parent)
        {
            this.Ctx = Ctx;

            Add(new ColorPhaseActivity(Ctx.capsuleRenderer) // Adding an activity to the state
            {
                enterColor = Color.green, // Initializing ColorPhaseActivity enterColor field
                exitColor = Color.green
            });
        }

        protected override State GetTransition()
        {
            //ChangeState: Idle->Jump
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

            //ChangeState: Idle->Walk
            return Mathf.Abs(Ctx.IntendedHorizontalDirection) > 0.01f ? ((PlayerGrounded)Parent).PlayerWalk : null;
        }

        protected override void OnEnter()
        {
            Ctx.Velocity.x = 0f;
        }
    }
}

/* ((PlayerRoot)Parent).Airborne.
In the file State.cs there is "Parent = parent;" NOTE: it is of type State.
Since Grounded inherits from State, it has a Parent field.
In the file PlayerRoot.cs there is "Grounded = new Grounded(m, this, Ctx);" so for Grounded, Parent = PlayerRoot because of the "this".
However, PlayerRoot is of type PlayerRoot, while Parent is of type State. Can assign it because PlayerRoot inherits from State.
The problem is that even though PlayerRoot inherits from State, State does not have an Airborne field, but only PlayerRoot does.
Therefore, cannot write "Parent.Airborne"; Must cast Parent to PlayerRoot. */