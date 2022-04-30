using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Player
{
    public class PlayerStateFactory
    {
        private enum States
        {
            wait,
            move,
            attack,
            grabbing,
            stunned,
            jump
        }

        private PlayerStateMachine _context;
        private Dictionary<States, BaseState> _states = new Dictionary<States, BaseState>();

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;
            _states[States.wait] = new WaitState(_context, this);
            _states[States.move] = new MoveState(_context, this);
            _states[States.attack] = new AttackState(_context, this);
            _states[States.grabbing] = new GrabbingState(_context, this);
            _states[States.stunned] = new StunnedState(_context, this);
            _states[States.jump] = new JumpState(_context, this);
        }

        public BaseState Wait() => _states[States.wait];
        public BaseState Move() => _states[States.move];
        public BaseState Attack() => _states[States.attack];
        public BaseState Grabbing() => _states[States.grabbing];
        public BaseState Stunned() => _states[States.stunned];
        public BaseState Jump() => _states[States.jump];
    }
}
