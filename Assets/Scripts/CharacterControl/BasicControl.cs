using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BasicControl : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    private Rigidbody2D _rb;
    private bool _isDead = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    private float _nextAttackTime = 0f;

    void Update()
    {
        if (_isDead) return;

        HandleMovement();

        //HandleJump();

        Attack();

        float h = Input.GetAxis("Horizontal");

        if (h != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(h) * Math.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float h = Input.GetAxis("Horizontal");

            // 使用当前实际移动速度
            _rb.velocity = new Vector2(h * GameDataManager.Instance.moveSpeed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    // Visualization for the Editor to see the ground check circle
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    private void Attack()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Try to perform attack");

                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    Monster monster = enemy.GetComponent<Monster>();

                    if (monster != null)
                    {
                        monster.TakeDamage(GameDataManager.Instance.damage);
                    }
                }

                _nextAttackTime = Time.time + GameDataManager.Instance.attackCooldown;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        GameDataManager.Instance.health -= damage;
    }
}