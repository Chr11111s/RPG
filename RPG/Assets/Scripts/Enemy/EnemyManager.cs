using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    EnemyCtrl[] _allEnemies;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _allEnemies = GetComponentsInChildren<EnemyCtrl>();
    }

    public void Initial()
    {
        for (int i = 0; i < _allEnemies.Length; i++)
        {
            _allEnemies[i].Initial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
