using System.Collections.Generic;

namespace Siphonn
{
    public class StateFactory
    {
        //private enum States
        //{
        //    wait,
        //    move,
        //    attack,
        //    stunned
        //}

        private StateMachine _context;
        //private Dictionary<States, BaseState> _states = new Dictionary<States, BaseState>();

        public StateFactory(StateMachine currentContext)
        {
            _context = currentContext;

            /// EXAMPLE
            //_states[States.wait] = new WaitState(_context, this);
            //_states[States.move] = new MoveState(_context, this);
            //_states[States.attack] = new AttackState(_context, this);
            //_states[States.stunned] = new StunnedState(_context, this);
        }

        /// EXAMPLE
        //public BaseState Wait() => _states[States.wait];
        //public BaseState Move() => _states[States.move];
        //public BaseState Attack() => _states[States.attack];
        //public BaseState Stunned() => _states[States.stunned];
    }
}
