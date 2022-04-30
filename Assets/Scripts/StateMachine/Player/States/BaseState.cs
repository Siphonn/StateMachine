
namespace Siphonn.Player
{
    public abstract class BaseState
    {
        public PlayerStateMachine context;
        public PlayerStateFactory factory;

        /// Constructor
        public BaseState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory)
        {
            context = currentContext;
            factory = currentStateFactory;

        }

        public abstract void EnterState();
        public abstract void UpdateState();
        protected abstract void ExitState();

        protected void SwitchState(BaseState newState)
        {
            ExitState();
            newState.EnterState();
            context.currentState = newState;
        }
    }
}
