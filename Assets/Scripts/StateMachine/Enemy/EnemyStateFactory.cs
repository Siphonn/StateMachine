using System.Collections.Generic;

namespace Siphonn.Enemy
{
    public class EnemyStateFactory : StateFactory
    {
        private enum States
        {
            wait,
            move,
            attack,
            stunned
        }

        private EnemyStateMachine _context;
        private Dictionary<States, BaseState> _states = new Dictionary<States, BaseState>();

        public EnemyStateFactory(EnemyStateMachine currentContext) : base (currentContext)
        {
            _context = currentContext;
            _states[States.wait] = new WaitState(_context, this);
            _states[States.move] = new MoveState(_context, this);
            _states[States.attack] = new AttackState(_context, this);
            _states[States.stunned] = new StunnedState(_context, this);
        }

        public BaseState Wait() => _states[States.wait];
        public BaseState Move() => _states[States.move];
        public BaseState Attack() => _states[States.attack];
        public BaseState Stunned() => _states[States.stunned];
    }
}


/// Backup
// public IdleState Idle() => new IdleState(_context, this);
//public MoveState Move() => new MoveState(_context, this);
//public AttackState Attack() => new AttackState(_context, this);

/// other way to set states
//_states = new Dictionary<string, BaseState>()
//{
//    { "idle", new IdleState(_context, this) },
//    { "move", new MoveState(_context, this) },
//    { "attack", new AttackState(_context, this) },
//};