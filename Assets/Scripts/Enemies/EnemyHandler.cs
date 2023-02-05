using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHandler
{
    public static ObjectPool<EnemyBullet> enemyBulletPool;
    public static List<EnemyBullet> activeEnemyBullets;

    public static void Initialize()
    {
        enemyBulletPool = new ObjectPool<EnemyBullet>();
        activeEnemyBullets = new List<EnemyBullet>();
    }

    public static void FixedUpdate()
    {
        for(int i = 0; i < activeEnemyBullets.Count; i++) { 
        
            activeEnemyBullets[i].LogicUpdate();
        }
    }

    public static void ReturnBullet(EnemyBullet _toReturn)
    {
        activeEnemyBullets.Remove(_toReturn);
        enemyBulletPool.ReturnObjectToPool(_toReturn);
    }

    public static EnemyBullet RequestBullet()
    {
        EnemyBullet temp = enemyBulletPool.RequestItem();
        activeEnemyBullets.Add(temp);
        return temp;
    }
}
