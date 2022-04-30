using UnityEngine;
using Siphonn.Enemy;

namespace Siphonn.Player
{
    public class GrabbingState : BaseState
    {
        EnemyStateMachine _enemy;
        private float _grabTimer;
        private int _hitCounter;
        private int _maxGrabHits = 3;


        public GrabbingState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState() { }

        public override void UpdateState()
        {
            if (!context.grabbedEnemy)
            {
                if (Time.time > _grabTimer)
                {
                    SwitchState(factory.Wait());
                }
            }
            else
            {
                if (!context.isGrabbing)
                {
                    StartGrab();
                }

                if (context.lightButtonCount > 0)
                {
                    GrabLightAttack();
                }
                else if (context.heaveButtonCount > 0) /// special attack
                {
                    GrabHeavyAttack();
                }
                else if (context.jumpPressed) /// jump to enemys other side
                {
                    GrabJump();
                }
            }
        }

        protected override void ExitState() { }

        private void StartGrab()
        {
            context.isGrabbing = true;
            //context.playerAnim.SetBool("Grabbing", true);

            _enemy = context.grabbedEnemy;
            Vector3 direction = GetDirection();
            float xOffset = direction.x > 0 ? -0.25f : 0.25f;

            Vector3 newPosition = new Vector3(_enemy.transform.position.x + xOffset, context.transform.position.y, _enemy.transform.position.z);
            context.transform.position = newPosition;
        }

        private void GrabLightAttack()
        {
            context.lightButtonCount = 0;

            if (context.horzMovement > 0.5f || context.horzMovement < -0.5f) /// attack in direction
            {
                Debug.Log("Grab - BIG Knock back!");
                float inputDirection = context.horzMovement;
                ThrowEnemy(_enemy, new Vector2(inputDirection, 0.5f), 125);
            }
            else /// nuetral attack
            {
                _hitCounter++;
                if (_hitCounter < _maxGrabHits)
                {
                    //context.playerAnim.SetTrigger("Grab Knee");
                }
                else if (_hitCounter >= _maxGrabHits)
                {
                    //Debug.Log("Grab - Knock back!");
                    ThrowEnemy(_enemy, ThrowDirection(), 100);
                }
            }
        }

        private void GrabHeavyAttack()
        {
            context.heaveButtonCount = 0;
            //Debug.Log("Grab - Special attack");
            //context.playerAnim.SetTrigger("Special");
            ThrowEnemy(_enemy, ThrowDirection(), 150);
        }

        private void GrabJump()
        {
            context.jumpPressed = false;
            Vector3 dir = GetDirection();
            context.transform.position += (dir * 2);
            context.Flip();

            //TODO: animate jump transition
        }

        private Vector3 GetDirection()
        {
            return _enemy.transform.position - context.transform.position;
        }

        private Vector2 ThrowDirection()
        {
            Vector2 direction = GetDirection();
            direction = direction.normalized;
            direction.y = 0.5f;
            return direction;
        }

        private void ThrowEnemy(EnemyStateMachine enemy, Vector2 throwDirection, float force)
        {
            enemy.enemyRb.AddForce(throwDirection * force, ForceMode.Force);
            enemy.hitStunned = true;
            enemy.knockedDown = true;
            EndThrowState();
        }

        /// <summary>
        /// Returns IdleState after clearing grab values
        /// </summary>
        private void EndThrowState()
        {
            _hitCounter = 0;
            context.isGrabbing = false;
            context.grabbedEnemy = null;
            //context.playerAnim.SetBool("Grabbing", false);
            _grabTimer = Time.time + context.grabCooldown;
        }
    }
}
