
namespace Siphonn
{
    public abstract class BaseState
    {
        public StateMachine context;
        public StateFactory factory;

        /// Constructor
        public BaseState(StateMachine currentContext, StateFactory currentStateFactory)
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
