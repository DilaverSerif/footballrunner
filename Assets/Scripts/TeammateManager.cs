using Dreamteck.Splines;
using UnityEngine;

public class TeammateManager : SpawnManager
{
    public static TeammateManager instance;
    private EnvironmentManager _environmentManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else Destroy(instance);
        
        
        _environmentManager = GetComponent<EnvironmentManager>();
        _splineComputer = transform.Find("MateLine").GetComponent<SplineComputer>();
    }

    protected override void Finish()
    {
        var mates = FindObjectsOfType<Mate>();

        foreach (var mate in mates)
        {
            mate.gameObject.SetActive(false);
        }
    }
    
    
    
    private void Start()
    {
        _splineComputer.SetPointPosition
            (1,new Vector3(1.25f,0,_environmentManager.FinishZ),SplineComputer.Space.World);
    }

    protected override void Order(int amount = 1)
    {
        var distance = 1 / (float)amount ;
        
        for (int i = 1; i < amount; i++)
        {
            var selectedEnemy = objectPools[0];
            selectedEnemy.GetComponent<SplineFollower>().SetPercent(distance * i);
            selectedEnemy.gameObject.SetActive(true);
            objectPools.RemoveAt(0);
        }
    }
}
