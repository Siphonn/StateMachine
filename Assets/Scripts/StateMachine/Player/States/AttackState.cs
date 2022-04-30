using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Player
{
    public class AttackState : BaseState
    {
        private float attackStateEndTime;
        private float currentCooldownTime;
        private int currentCombo;

        public AttackState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState() { }

        public override void UpdateState()
        {
            if (context.attackFinished)
            {
                /// Start cooldown
                if (attackStateEndTime == 0f)
                {
                    attackStateEndTime = Time.time + currentCooldownTime;
                }

                if (Time.time > attackStateEndTime)
                {
                    SwitchState(factory.Wait());
                    return;
                }
            }
            else
            {
                /// Animation parameters set here
                if (context.attacking)
                {
                    context.attacking = false;

                    if (context.heaveButtonCount > 0)
                    {
                        context.heaveButtonCount = 0;
                        //context.playerAnim.SetTrigger("Special");
                        currentCooldownTime = context.specialAttackCooldown;
                    }
                    else if (context.lightButtonCount > 0 && currentCombo < context.maxComboLength)
                    {
                        currentCombo++;
                        context.lightButtonCount = 0;
                        //context.playerAnim.SetBool("Attack", true);
                        currentCooldownTime = context.attackCooldown;
                    }
                }
            }
        }

        protected override void ExitState()
        {
            context.attackFinished = false;
            context.attacking = false;
            context.lightButtonCount = 0;
            context.heaveButtonCount = 0;
            currentCombo = 0;
            attackStateEndTime = 0;
        }
    }
}
