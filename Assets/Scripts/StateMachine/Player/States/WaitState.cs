using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Player
{
    public class WaitState : BaseState
    {
        private float _poseTimer;
        private bool _isPosing;


        public WaitState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState()
        {
            _poseTimer = Time.time + context.idlePoseTime;
        }

        public override void UpdateState()
        {
            if (!_isPosing && Time.time > _poseTimer)
            {
                _isPosing = true;
                //context.playerAnim.SetBool("isPosing", _isPosing);
            }

            CheckForStateSwitch();
        }

        protected override void ExitState()
        {
            _isPosing = false;
            //context.playerAnim.SetBool("isPosing", _isPosing);
        }

        private void CheckForStateSwitch()
        {
            if (context.move > 0 && !context.hitStunned)
            {
                SwitchState(factory.Move());
                return;
            }
            else if (context.lightButtonCount > 0 || context.heaveButtonCount > 0)
            {
                context.attacking = true; /// Setting 'attacking' to true will start the attack animation in the 'AttackState'
                SwitchState(factory.Attack());
                return;
            }
            else if (context.jumpPressed)
            {
                SwitchState(factory.Jump());
                return;
            }
            else if (context.hitStunned || context.knockedDown)
            {
                SwitchState(factory.Stunned());
                return;
            }
        }
    }
}
