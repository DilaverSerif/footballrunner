using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform chunk, finishChunk;
    
    private List<Transform> chucks = new List<Transform>();
    private List<Transform> spawnedChunks = new List<Transform>();
    private Transform player;
    
    [SerializeField] private int finishCount;
    public float FinishZ => finishCount * 10.17f + 31f;

    public static Action<int> SpawnChunk;
    
    private void OnEnable()
    {
        SpawnChunk += ActiveChunk;
    }

    private void OnDisable()
    {
        SpawnChunk -= ActiveChunk;
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }


    private int chunkCount = 0;
    private void ActiveChunk(int amount = 1)
    {
        if(chunkCount == finishCount) return;
        
        if (chucks.Count == 0)
        {
            for (var i = 0; i < amount; i++)
            {
                var spawnedChunk = Instantiate(chunk);
                spawnedChunk.gameObject.SetActive(false);
                chucks.Add(spawnedChunk);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            var selectedChunk = chucks[0];
            var pos = Vector3.zero;
            pos.z = chunkCount * 10.17f;
            selectedChunk.position = pos;
            selectedChunk.gameObject.SetActive(true);
            chucks.RemoveAt(0);
            spawnedChunks.Add(selectedChunk);
            chunkCount++;
        }

        if (chunkCount == finishCount)
        {
            var pos = Vector3.zero;
            pos.z = chunkCount * 10.17f;
            
            var end = Instantiate(finishChunk);
            end.position = pos;
        }
        
        CheckChunks(amount);
    }

    private void CheckChunks(int amount = 1)
    {
        if(spawnedChunks.Count == 0) return;
        
        for (int i = 0; i < amount; i++)
        {
            if (i > spawnedChunks.Count) break;
            
            var forDot = Vector3.Normalize(spawnedChunks[0].position - player.position);
            var dot = Vector3.Dot(player.forward,forDot);

            if (dot > 0) continue;
            
            var distance = Vector3.Distance(player.position, spawnedChunks[0].position);

            if (distance < 30) continue;
            
            chucks.Add(spawnedChunks[0]);
            spawnedChunks[0].gameObject.SetActive(false);
            spawnedChunks.RemoveAt(0);

        }
        
    }
}