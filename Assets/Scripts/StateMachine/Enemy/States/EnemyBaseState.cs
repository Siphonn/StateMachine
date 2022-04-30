
namespace Siphonn.Enemy
{
    public abstract class EnemyBaseState : BaseState
    {
        public EnemyStateMachine context;
        public EnemyStateFactory factory;

        /// Constructor
        public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory currentStateFactory) : base(currentContext, currentStateFactory)
        {
            context = currentContext;
            factory = currentStateFactory;
        }

        //public abstract void EnterState();
        //public abstract void UpdateState();
        //protected abstract void ExitState();

        //protected void SwitchState(BaseState newState)
        //{
        //    ExitState();
        //    newState.EnterState();
        //    _context.currentState = newState;
        //}
    }
}
