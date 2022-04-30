using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Player
{
    public class StunnedState : BaseState
    {
        private float _stunTimer;


        public StunnedState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState()
        {
            _stunTimer = 0;
        }

        public override void UpdateState()
        {
            if (context.hitStunned)
            {
                if (_stunTimer == 0)
                {
                    _stunTimer = Time.time + context.hitRecovery;
                    //context.playerAnim.SetBool("Hit", true);
                }
                else if (Time.time > _stunTimer)
                {
                    context.hitStunned = false;
                    _stunTimer = 0;
                    //context.playerAnim.SetBool("Hit", false);
                }
            }
            else if (context.knockedDown) //TODO: different recovery time or reset from animation event function
            {
                if (_stunTimer == 0)
                {
                    _stunTimer = Time.time + context.knockdownRecovery;
                    //context.playerAnim.SetBool("Knocked Down", true);
                }
                else if (Time.time > _stunTimer)
                {
                    context.knockedDown = false;
                    _stunTimer = 0;
                    //context.playerAnim.SetBool("Knocked Down", false);
                }
            }
            else
            {
                SwitchState(factory.Wait());
                return;
            }
        }

        protected override void ExitState() { }
    }
}
