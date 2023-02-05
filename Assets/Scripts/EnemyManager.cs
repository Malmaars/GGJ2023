using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] EnemyPrefab;
    List<GameObject> enemyPool;

    List<GameObject> activeEnemies;
    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new List<GameObject>();
        activeEnemies = new List<GameObject>();

        SpawnEnemy(new Vector2(1.5f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject enemy in activeEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (!enemyScript.alive)
            {
                enemyScript.alive = true;

                enemyPool.Add(enemy);
                activeEnemies.Remove(enemy);
                enemy.SetActive(false);
                SpawnEnemy(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)));
                break;
            }
        }
    }

    public void SpawnEnemy(Vector2 _loc)
    {
        GameObject temp;
        if(enemyPool.Count > 0)
        {
            temp = enemyPool[0];
            enemyPool.Remove(temp);
            temp.SetActive(true);
            temp.GetComponent<Enemy>().OnEnableObject();
        }
        else
        {
            temp = Instantiate(EnemyPrefab[Random.Range(0,EnemyPrefab.Length)]);
            temp.SetActive(true);
            temp.GetComponent<Enemy>().OnEnableObject();
        }

        activeEnemies.Add(temp);
        temp.transform.position = _loc;
    }
}
