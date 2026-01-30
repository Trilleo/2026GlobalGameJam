using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public Collider2D rangeCollider;   // 把你的触发碰撞体拖进来
    public LayerMask enemyLayer;

    public List<Collider2D> GetMonstersInsideNow()
    {
        // 2D 触发盒就是范围
        return Physics2D.OverlapBoxAll(
            rangeCollider.bounds.center,
            rangeCollider.bounds.size,
            0,
            enemyLayer
        ).ToList();
    }
}
