using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemySpace;

public class UnitManager : MonoBehaviour
{
    int id;
    private List<Enemy> enemyPull;
    void Awake()
    {
        CreatePull();
        foreach(Enemy a in enemyPull)
        {
            a.EnemyAwake();
        }
    }

    void FixedUpdate()
    {
        var deltaTime = Time.deltaTime;
        foreach (Enemy a in enemyPull)
        {
            a.EnemyUpdate(deltaTime);
        }
    }

    private void CreatePull()
    {
        id = 1;
        enemyPull = new List<Enemy>();
        Enemy[] startEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy a in startEnemies)
        {
            enemyPull.Add(a);
            a.name = "Проклятый охотник" + id;
            id++;
        }
    }

    private void DestroyUnit(Enemy dead)
    {
        Destroy(dead.gameObject);
    }
}
