using TMPro;
using UnityEngine;

public class Thunderbomb: MonoBehaviour
{
    [Header("ÉËº¦")]
    public float damage = 10f;
    [Header("´æÔÚÊ±¼ä")]
    public float lifeTime = 3f;
    [Header("Âé±ÔÊ±³¤")]
    public float skillshuttime = 2f;
    [Header("±¬Õ¨·¶Î§ÉËº¦±¶ÂÊ")]
    public float explosionDamageMultiplier = 0.6f;
    [Header("ÊÇ·ñ¿É´©Í¸Ç½Ìå")]
    public bool ifpenetratewall = false;
    [Header("·¶Î§±¬Õ¨")]
    public float explosionRadius = 2f;    // ±¬Õ¨°ë¾¶
    public float knockBack = 15f;         // »÷·ÉËÙ¶È
    public GameObject explosionFX;
    public LayerMask enemyLayer;

    Rigidbody2D rb;
    private bool hasExploded = false;     // ·ÀÖ¹ÖØ¸´±¬Õ¨

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start() => Destroy(gameObject, lifeTime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            Explode(other);

        if (other.CompareTag("Ground") && !ifpenetratewall)
            Explode(null);   // Åöµ½Ç½Ò²Õ¨
    }

    /* ·¶Î§±¬Õ¨ */
    void Explode(Collider2D firstEnemy)
    {
        if (hasExploded) return;
        hasExploded = true;
        /* 1. ÒÔÀ×ÇòÎªÖÐÐÄ£¬É¨ËùÓÐµÐÈË */
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (var enemy in enemies)
        {
            Vector2 dir = (enemy.transform.position - transform.position).normalized;
            if (enemy == firstEnemy)
            {
                enemy.GetComponent<Monster>()?.TakeDamage(damage, skillshuttime, dir * knockBack);
                //enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            else
            {
                enemy.GetComponent<Monster>()?.TakeDamage(damage*explosionDamageMultiplier, skillshuttime, dir * knockBack);
                //enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }

        if (explosionFX)
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
        }


        /* 5. ×Ô»Ù */
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}