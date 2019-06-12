using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemySpace;

public class UnitManager : MonoBehaviour
{
    int id;
    float timer;
    private Dictionary<string, Enemy> enemyPull;

    void Awake()
    {
        CreatePull();
        foreach(Enemy a in enemyPull.Values)
        {
            a.EnemyAwake();
        }
        EnemyDie.DieEvent += DeactivateUnit;
    }

    void FixedUpdate()
    {
        var deltaTime = Time.deltaTime;
        foreach (Enemy a in enemyPull.Values)
        {
            a.EnemyUpdate(deltaTime);
        }
    }

    private void CreatePull()
    {
        id = 1;
        enemyPull = new Dictionary<string, Enemy>();
        Enemy[] startEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy a in startEnemies)
        {
            a.name = "Проклятый охотник" + id;
            enemyPull.Add(a.name, a);
            id++;
        }
    }

    private void DeactivateUnit(string unitName)
    {
        enemyPull[unitName].gameObject.SetActive(false);
    }
}
