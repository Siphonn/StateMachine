using UnityEngine;

namespace Siphonn.Enemy
{
    public class AttackState : EnemyBaseState
    {
        private float attackTimer;
        private float cooldownTime = 2f;


        public AttackState(EnemyStateMachine currentContext, EnemyStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState()
        {
            attackTimer = Time.time + cooldownTime;
            //_enemyAnim.SetTrigger("Attack");
        }

        public override void UpdateState()
        {
            if (Time.time > attackTimer || context.hitStunned || context.knockedDown)
            {
                SwitchState(factory.Wait());
                return;
            }
        }

        protected override void ExitState() { }
    }
}