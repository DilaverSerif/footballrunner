using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int chuckSize;
    [SerializeField] private int enemyCount;
    [SerializeField] private int mateCount; 
    
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else Destroy(gameObject);
    }

    private void Start()
    {
        EnvironmentManager.SpawnChunk.Invoke(chuckSize);
        EnemySpawner.instance.SpawnObject.Invoke(enemyCount);
        TeammateManager.instance.SpawnObject.Invoke(mateCount);
    }
    
    
    
}
