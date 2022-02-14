using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Primitives;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : SpawnManager
{
    public static EnemySpawner instance;
    private EnvironmentManager _environmentManager;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else Destroy(instance);
        
        
        _environmentManager = GetComponent<EnvironmentManager>();
        _splineComputer = transform.Find("EnemyLine").GetComponent<SplineComputer>();
    }

    private void Start()
    {
        _splineComputer.SetPointPosition
            (0,new Vector3(-1.25f,0,_environmentManager.FinishZ),SplineComputer.Space.World);
        
        _splineComputer.SetPointPosition
            (1,new Vector3(-1.25f,0,0),SplineComputer.Space.World);
    }

    protected override void Finish()
    {
        var enemys = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemys)
        {
            enemy.GetComponent<SplineFollower>().wrapMode = SplineFollower.Wrap.Default;
        }
    }

    

    protected override void Order(int amount = 1)
    {
        float percent = 1 / (float)amount;
        
        for (int i = 0; i < amount; i++)
        {
            var nowPercent = percent * i;
            
            if (Random.value > 0.7f)
            {
                if(i == amount) return;
                nowPercent += percent;
                i++;
            }
            
            var selectedEnemy = objectPools[0];
            selectedEnemy.GetComponent<SplineFollower>().SetPercent(nowPercent);
            selectedEnemy.gameObject.SetActive(true);
            objectPools.RemoveAt(0);
        }
    }
}
