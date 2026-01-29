using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SkeletonKnight : Monster
{
    private bool isEnraged = false;
    private float enrageMultiplier = 1.5f;

    void Start()
    {
        // 先确保找到玩家
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("找不到Player！");
                return;
            }
        }

        Reset();
    }

    void Update()
    {
        LoadState();

        if (!isEnraged && health <= monsterdata.health * 0.5f)
        {
            Enrage();
        }
    }

    public override void Attack(Collider2D other, int id)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BasicControl>()?.TakeDamage(damage);
        }
    }

    public override void Die()
    {
        _isDead = true;
        Destroy(gameObject);
    }

    void Enrage()
    {
        isEnraged = true;
        speed *= enrageMultiplier;
        Debug.Log("骷髅骑士进入狂暴状态！速度提升！");
    }

    #region 行为状态实现
    protected override void IdleState(float dist)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        if (dist < monsterdata.detectRange)
        {
            currentState = State.Chase;
            return;
        }

        idleTimer += Time.deltaTime;
        if (idleTimer >= monsterdata.idleTime)
        {
            idleTimer = 0;
            currentState = State.Patrol;
            facingRight = Random.value > 0.5f;
            float moveDir = facingRight ? 1 : -1;
            transform.localScale = new Vector3(moveDir * baseScaleX, transform.localScale.y, 1);
        }
    }

    protected override void PatrolState(float dist)
    {
        if (dist < monsterdata.detectRange)
        {
            currentState = State.Chase;
            return;
        }

        patrolTimer += Time.deltaTime;
        float moveDir = facingRight ? 1 : -1;

        // 修复：必须设置速度！
        float currentSpeed = isEnraged ? speed * 1.2f : speed;
        rb.velocity = new Vector2(moveDir * currentSpeed, rb.velocity.y);

        if (ShouldTurn(moveDir))
        {
            patrolTimer = 0;
            FaceTo(-moveDir);
            currentState = State.Idle;
            return;
        }

        if (patrolTimer >= monsterdata.patrolDuration)
        {
            patrolTimer = 0;
            FaceTo(-moveDir);
            currentState = State.Idle;
            return;
        }

        if (Random.value < 0.0001f) currentState = State.Idle;
    }

    protected override void ChaseState(float dist)
    {
        if (dist <= monsterdata.attackRange)
        {
            rb.velocity = Vector2.zero;
            currentState = State.Attack;
            return;
        }

        if (dist > monsterdata.detectRange * 1.5f)
        {
            currentState = State.Idle;
            return;
        }

        float dirX = player.position.x > transform.position.x ? 1 : -1;

        if (ShouldTurn(dirX))
        {
            rb.velocity = Vector2.zero;
            FaceTo(-dirX);
            return;
        }

        // 修复：必须设置速度！
        float chaseSpeed = isEnraged ? speed * 1.8f : speed * 1.5f;
        rb.velocity = new Vector2(dirX * chaseSpeed, rb.velocity.y);
        FaceTo(dirX);
    }

    protected override void AttackState(float dist)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= monsterdata.attackCooldown)
        {
            attackTimer = 0;

            float dirX = player.position.x > transform.position.x ? 1 : -1;
            FaceTo(dirX);

            anim.SetTrigger("Attack");

            // 攻击后的移动逻辑
            if (dist <= monsterdata.attackRange)
            {
                rb.velocity = Vector2.zero;
                currentState = State.Attack;
            }
            else
            {
                float chaseSpeed = isEnraged ? speed * 1.8f : speed * 1.5f;
                rb.velocity = new Vector2(dirX * chaseSpeed, rb.velocity.y);
                currentState = State.Chase;
            }
        }
        else
        {
            float dirX = player.position.x > transform.position.x ? 1 : -1;
            FaceTo(dirX);
        }
    }
    #endregion
}