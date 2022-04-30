using UnityEngine;

namespace Siphonn.Enemy
{
    public class WaitState : EnemyBaseState
    {
        private float waitTimer;


        public WaitState(EnemyStateMachine currentContext, EnemyStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState()
        {
            waitTimer = Time.time + context.stateTransitionTime;
        }

        public override void UpdateState()
        {
            if (context.target == null) { return; }

            if (context.isGrabbed || context.hitStunned || context.knockedDown)
            {
                SwitchState(factory.Stunned());
                return;
            }

            if (Time.time > waitTimer)
            {
                float distance = Vector3.Distance(context.transform.position, context.target.position);
                if (distance > context.attackDistance)
                {
                    //Debug.Log("Enemy: Idle -> Move");
                    SwitchState(factory.Move());
                    return;
                }
                else
                {
                    //Debug.Log("Enemy: Idle -> Attack");
                    /// Flip to face the targeted players
                    Vector3 direction = context.target.transform.position - context.transform.position;
                    if (direction.x > 0 && !context.facingRight || direction.x < 0 && context.facingRight)
                    {
                        context.Flip();
                    }
                    SwitchState(factory.Attack());
                    return;
                }
            }
        }

        protected override void ExitState() { }
    }
}