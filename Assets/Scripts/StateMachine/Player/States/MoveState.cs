using UnityEngine;
using Siphonn.Enemy;

namespace Siphonn.Player
{
    public class MoveState : BaseState
    {
        public MoveState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState() { }

        public override void UpdateState()
        {
            ///FLIP SPRITE
            if (context.horzMovement > 0 && !context.facingRight || context.horzMovement < 0 && context.facingRight)
            {
                context.Flip();
            }

            /// MOVE
            float speedValue = (context.dashing) ? context.dashSpeed  : 1;
            context.playerAnim.speed = speedValue; // Set animation to default speed
            context.transform.position += new Vector3(
                context.horzMovement * (context.moveSpeed * speedValue) * Time.deltaTime,
                0,
                context.vertMovement * (context.moveSpeed * speedValue) * Time.deltaTime);

            CheckForStateSwitch();
        }

        protected override void ExitState()
        {
            context.playerAnim.speed = 1;
        }

        private void CheckForStateSwitch()
        {
            if (context.move == 0)
            {
                SwitchState(factory.Wait());
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

            /// Check for enemy to grab
            Collider[] enColliders = Physics.OverlapSphere(context.grabPoint.position, context.grabRadius, LayerMask.GetMask("Enemy"));
            if (enColliders.Length > 0)
            {
                context.grabbedEnemy = enColliders[0].GetComponent<EnemyStateMachine>();
                if (context.grabbedEnemy != null)
                {
                    // TODO: may be better to use the facing direction or dot product the get the enemy directly in front.
                    float enemyDist = Vector3.Distance(context.transform.position, context.grabbedEnemy.transform.position);
                    if (enemyDist < 0.25f && !context.grabbedEnemy.knockedDown && !context.grabbedEnemy.hitStunned)
                    {
                        context.grabbedEnemy.isGrabbed = true;
                        SwitchState(factory.Grabbing());
                        return;
                    }
                }
            }
        }
    }
}
