using UnityEngine;

namespace Siphonn.Enemy
{
    public class StunnedState : EnemyBaseState
    {
        private float stunnedTimer;


        public StunnedState(EnemyStateMachine currentContext, EnemyStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState()
        {
            stunnedTimer = 0;
            /// TODO: set recovery timer depending on stun type
            /// could also set animation bool here
        }

        public override void UpdateState()
        {
            if (context.hitStunned)
            {
                if (stunnedTimer == 0 || context.hitWhileStunned)
                {
                    context.hitWhileStunned = false;
                    //_context.enemyAnim.SetBool("IsHit", true);
                    stunnedTimer = Time.time + context.hitRecovery;
                }
                else if (Time.time > stunnedTimer)
                {
                    context.hitStunned = false;
                    //_context.enemyAnim.SetBool("IsHit", false);
                }
            }
            else if (context.knockedDown)
            {
                if (stunnedTimer == 0)
                {
                    stunnedTimer = Time.time + context.knockdownRecovery;
                    //_context.enemyAnim.SetBool("Fall", true);
                }
                else if (Time.time > stunnedTimer)
                {
                    context.knockedDown = false;
                    //_context.enemyAnim.SetBool("Fall", false);
                }
            }
            else if (context.isGrabbed)
            {

            }
            else
            {
                /// Return to Wait
                SwitchState(factory.Wait());
                return;
            }
        }

        protected override void ExitState()
        {
            context.hitStunned = false;
            context.knockedDown = false;
            context.isGrabbed = false;
            Debug.Log("Exit Stunned state");
        }
    }
}
