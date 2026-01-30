using TMPro;
using UnityEngine;

public class Thunderball: MonoBehaviour
{
    [Header("伤害")]
    public float damage = 10f;
    [Header("存在时间")]
    public float lifeTime = 3f;
    [Header("麻痹时长")]
    public float shuttime = 0.1f;
    [Header("是否可穿透敌人")]
    public bool ifpenetrate = false;
    [Header("是否可穿透墙体")]
    public bool ifpenetratewall = false;
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start() => Destroy(gameObject, lifeTime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            Hit(other);

        if (other.CompareTag("Ground")&&!ifpenetratewall)
        {
            Destroy(gameObject);
        }
    }

    void Hit(Collider2D enemy)
    {
        enemy.GetComponent<Monster>()?.TakeDamage(damage,shuttime);
        enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (ifpenetrate == false)
        Destroy(gameObject); 
    }
}