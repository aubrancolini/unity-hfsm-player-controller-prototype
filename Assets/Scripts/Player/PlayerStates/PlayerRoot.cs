using HSM;

//This class is the root node of the state machine states hierarchy.
namespace Game.Player
{
    public class PlayerRoot : State
    {
        public readonly PlayerGrounded PlayerGrounded;
        public readonly PlayerAirborne Airborne;
        readonly PlayerContext Ctx;

        public PlayerRoot(StateMachine m, PlayerContext Ctx) : base(m, null)
        {
            this.Ctx = Ctx;
            PlayerGrounded = new PlayerGrounded(m, this, Ctx); // Istantiating child states  
            Airborne = new PlayerAirborne(m, this, Ctx);
        }

        protected override State GetInitialState() => Airborne; // Set active child when entering Grounded state
    }
}