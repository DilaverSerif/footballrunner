using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SpawnManager : MonoBehaviour
{
    [SerializeField] protected Transform[] objects;
    protected List<Transform> objectPools = new List<Transform>();
    protected SplineComputer _splineComputer;
    public float ObjectSpeed;
    public Action<int> SpawnObject;
    private void OnEnable()
    {
        SpawnObject += ActiveEnemy;
        Player.FinishEvent += Finish;
    }

    private void OnDisable()
    {
        SpawnObject -= ActiveEnemy;
        Player.FinishEvent -= Finish;
    }

    protected abstract void Finish();
    
    protected abstract void Order(int amount = 1);
    private void ActiveEnemy(int amount)
    {
        if (objectPools.Count == 0)
        {
            for (var i = 0; i < amount; i++)
            {
                var spawnedEnemy = Instantiate(objects[Random.Range(0,objects.Length)]);
                var follower = spawnedEnemy.GetComponent<SplineFollower>();
                follower.spline = _splineComputer;
                follower.followSpeed = ObjectSpeed;
                follower.wrapMode = SplineFollower.Wrap.Loop;
                objectPools.Add(spawnedEnemy);
                spawnedEnemy.gameObject.SetActive(false);
            }
        }
        
        Order(amount);
    }
    
}
