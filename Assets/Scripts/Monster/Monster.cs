using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public CreateMonster monsterdata;

    // ״̬ö��
    public enum State { Idle, Patrol, Chase, Attack }
    public State currentState = State.Idle;

    [System.Serializable]
    public class Effect
    {
        public string effectname;
        public float time;

        public void DecreaseTime(float deltaTime)
        {
            time -= deltaTime;
        }
    }

    [Header("��ǰ����")]
    public float health = 100f;
    [Header("��ǰ����")]
    public float damage = 10f;
    [Header("��ǰ�ƶ��ٶ�")]
    public float speed = 10f;
    [Header("��ǰbuff/debuff")]
    public List<Effect> effects = new List<Effect>();

    [Header("����Ƿ��ܵ��˺���Χ")]
    public List<Collider2D> monsterattackrange;
    [Header("���μ��")]
    public float frontRayDist = 0.8f;   // ǰ��ǽ������
    public float groundRayDist = 1f;   // �������¼�����
    public LayerMask groundLayer;        // ����㣨��ǽ�ڣ�

    protected Transform player;
    protected float patrolTimer;     
    protected Rigidbody2D rb;
    protected Animator anim;
    protected float idleTimer;
    protected float attackTimer;
    protected bool facingRight = true;
    protected bool attackFrame = false;
    protected float baseScaleX;
    protected Vector3 startPos;
    protected bool _isDead = false;

    public void Reset()//��ʼ����������
    {
        player=GameDataManager.Instance.player;
        foreach (Collider2D collider in monsterattackrange)
        {
            collider.enabled = false;
        }

        health = monsterdata.health;
        damage = monsterdata.damage;
        speed = monsterdata.speed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = transform.localScale.x >= 0;
        startPos = transform.position;
        player = GameObject.FindWithTag("Player")?.transform;

        facingRight = transform.localScale.x >= 0;
        baseScaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(facingRight ? baseScaleX : -baseScaleX,
                                            transform.localScale.y,
                                            transform.localScale.z);
    }

    public void LoadState()//��ʱ��ȡ״̬
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        UpdateAnimator(distToPlayer);

        switch (currentState)
        {
            case State.Idle:
                IdleState(distToPlayer);
                break;
            case State.Patrol:
                PatrolState(distToPlayer);
                break;
            case State.Chase:
                ChaseState(distToPlayer);
                break;
            case State.Attack:
                AttackState(distToPlayer);
                break;
        }

        if(effects.Contains(effects.Find(e => e.effectname == "Slow")))
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        var toRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            switch (effect.effectname)
            {

            }
            effect.DecreaseTime(Time.deltaTime);
            if (effect.time <= 0)
            {
                toRemove.Add(effect);
            }
        }
        for (int i = toRemove.Count - 1; i >= 0; i--)
        {
            switch (toRemove[i].effectname)
            {

            }

            effects.Remove(toRemove[i]);
        }
    }

    public abstract void Attack(Collider2D other,int id);//������󷽷��Ǵ�������������õ�

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void SetEffect(string name,float time)
    {
        if(effects.Exists(e => e.effectname == name))
        {
            var existingEffect = effects.Find(e => e.effectname == name);
            existingEffect.time = time;
            return;
        }
        effects.Add(new Effect { effectname = name, time = time });
    }

    public abstract void Die();

    #region ״̬������ʵ�ֳ��󷽷�
    protected abstract void IdleState(float dist);//��������

    protected abstract void PatrolState(float dist);//Ѳ�߷���

    protected abstract void ChaseState(float dist);//���з���

    protected abstract void AttackState(float dist);//��������

    protected bool ShouldTurn(float moveDir)
    {
        // 1. ǰ��ǽ��⣨ˮƽ���ߣ�
        Vector2 origin = transform.position;
        Vector2 direction = new Vector2(moveDir, 0);
        RaycastHit2D hitWall = Physics2D.Raycast(origin, direction, frontRayDist, groundLayer);

        // 2. �������¼�⣨б�������ߣ�
        Vector2 groundOrigin = transform.position + Vector3.down * 0.1f + Vector3.right * (facingRight ? 0.5f : -0.5f);
        RaycastHit2D hitGround = Physics2D.Raycast(groundOrigin, Vector2.down, groundRayDist, groundLayer);

        // ��ǽ �� �����޵��� �� Ӧ�õ�ͷ
        return hitWall.collider != null || hitGround.collider == null;
    }

    public void Starttrigger(int id)
    {
        monsterattackrange[id].enabled = true;
    }

    public void Finishtrigger(int id)
    {
        monsterattackrange[id].enabled = false;
    }

    protected void FaceTo(float dirX)
    {
        if (dirX > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(baseScaleX, transform.localScale.y, 1);
        }
        else if (dirX < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-baseScaleX, transform.localScale.y, 1);
        }
    }

    protected void UpdateAnimator(float dist)
    {
        //anim.SetFloat("moveX", Mathf.Abs(rb.velocity.x));
        //anim.SetFloat("distanceToPlayer", dist);
        //anim.SetBool("canAttack", dist <= attackRange);
    }
    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        // ���ӻ���Χ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterdata.detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterdata.attackRange);

        // ǰ��ǽ���
        Gizmos.color = Color.blue;
        Vector2 origin = transform.position;
        Vector2 dir = new Vector2(facingRight ? 1 : -1, 0);
        Gizmos.DrawRay(origin, dir * frontRayDist);

        // �������¼��
        Gizmos.color = Color.cyan;
        Vector2 groundOrigin = transform.position + Vector3.down * 0.1f + Vector3.right * (facingRight ? 0.5f : -0.5f);
        Gizmos.DrawRay(groundOrigin, Vector2.down * groundRayDist);
    }
}
