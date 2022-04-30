using UnityEngine;

namespace Siphonn.Enemy
{
    public class MoveState : EnemyBaseState
    {
        private Vector3 direction;
        private Vector3 moveDirection;
        private Vector3 moveToPosition;
        float zOffset = 0.025f;


        public MoveState(EnemyStateMachine currentContext, EnemyStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState() { }

        public override void UpdateState()
        {
            if (context.target == null) { return; }

            if (context.isGrabbed || context.hitStunned || context.knockedDown)
            {
                SwitchState(factory.Stunned());
                return;
            }

            direction = context.target.transform.position - context.transform.position;
            //_enemyAnim.SetFloat("Movement", direction.magnitude);

            if (direction.x > 0 && !context.facingRight || direction.x < 0 && context.facingRight)
            {
                context.Flip();
            }

            moveToPosition = (context.facingRight)
                ? context.target.position + new Vector3(context.attackDistance * -1, 0, 0)
                : context.target.position + new Vector3(context.attackDistance, 0, 0);

            float currentZ = context.transform.position.z;
            float playerZ = context.target.position.z;
            float distanceToTarget = DistanceFrom(context.target.position);

            if (currentZ > playerZ + zOffset || currentZ < playerZ - zOffset)
            {
                MoveTowards(moveToPosition);
            }
            else if (distanceToTarget > context.attackDistance)
            {
                MoveTowards(context.target.position);
            }
            else
            {
                SwitchState(factory.Wait());
                return;
            }
        }

        protected override void ExitState()
        {
            //_enemyAnim.SetFloat("Movement", 0);
        }

        private float DistanceFrom(Vector3 target)
        {
            return Vector3.Distance(context.transform.position, target);
        }

        private void MoveTowards(Vector3 targetPosition)
        {
            moveDirection = (targetPosition - context.transform.position).normalized;
            context.transform.position += moveDirection * context.speed * Time.fixedDeltaTime;
        }
    }
}
