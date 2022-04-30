using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Player
{
    public class JumpState : BaseState
    {
        private float _jumpTimer;
        private bool _hasJumped;
        private bool _jumpStateEnded;
        private bool _isAttacking;

        public JumpState(PlayerStateMachine currentContext, PlayerStateFactory currentStateFactory) : base(currentContext, currentStateFactory) { }

        public override void EnterState() { }

        public override void UpdateState()
        {
            if (_jumpStateEnded) /// Jump finished
            {
                if (Time.time > _jumpTimer)
                {
                    //Debug.Log("Jump -> Idle");
                    SwitchState(factory.Wait());
                    return;
                }
            }
            else /// Jump loop
            {
                if (context.onGround)
                {
                    if (context.jumpPressed && !_hasJumped)
                    {
                        _hasJumped = true;
                        context.jumpPressed = false;
                        context.onGround = false;
                        //context.playerAnim.SetBool("Grounded", false);

                        if (context.dashing) /// dashing jump
                        {
                            DashJump();
                        }
                        else /// neutral jump
                        {
                            NeutralJump();
                        }
                    }
                    else if (_hasJumped)
                    {
                        _hasJumped = false;
                        _isAttacking = false;
                        context.dashing = false;
                        //context.playerAnim.SetBool("Grounded", true);
                        //context.playerAnim.SetBool("Attack", false);

                        if (_jumpTimer == 0) /// start jumpstate cooldown timer
                        {
                            _jumpTimer = Time.time + context.jumpCooldown;
                            _jumpStateEnded = true;
                        }
                    }
                }
                else /// attack while in the air
                {
                    JumpAttack();
                }
            }
        }

        protected override void ExitState()
        {
            _jumpTimer = 0;
            _jumpStateEnded = false;
            context.enemyHitCheck = false;
        }

        private void DashJump()
        {
            if (context.horzMovement > 0)
            {
                context.playerRb.AddForce(new Vector2(0.75f, 1f) * (context.jumpForce - 0.5f), ForceMode.Impulse);
            }
            else if (context.horzMovement < 0)
            {
                context.playerRb.AddForce(new Vector2(-0.75f, 1f) * (context.jumpForce - 0.5f), ForceMode.Impulse);
            }
        }

        private void NeutralJump()
        {
            if (context.horzMovement > 0 && !context.facingRight || context.horzMovement < 0 && context.facingRight)
            {
                context.Flip();
            }
            context.playerRb.AddForce(new Vector2(context.horzMovement / 4, 1) * context.jumpForce, ForceMode.Impulse);
        }

        private void JumpAttack()
        {
            RaycastHit hit;
            if (Physics.Raycast(context.transform.position, -context.transform.up, out hit, 5, LayerMask.GetMask("Ground")))
            {
                Debug.DrawLine(context.transform.position, hit.point, Color.red);
                if (context.lightButtonCount > 0)
                {
                    if (!_isAttacking && hit.distance > 0.5f || context.dashing && !_isAttacking && hit.distance > 0.25f)
                    {
                        _isAttacking = true;
                        //context.playerAnim.SetBool("Attack", true);
                    }
                }
                if (_isAttacking)
                {
                    if (context.enemyHitCheck) /// enemyHitCheck set from jump attack animation
                    {
                        Collider[] enCol = Physics.OverlapSphere(context.attackPoint.position, context.attackRadius, LayerMask.GetMask("Enemy"));
                        if (enCol != null)
                        {
                            context.KnockBackEnemies();
                        }
                    }
                }
            }
        }
    }
}
