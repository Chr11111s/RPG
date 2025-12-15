using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    NPCCtrl[] _allNPCs;
    public static NPCManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _allNPCs = GetComponentsInChildren<NPCCtrl>();
    }

    public void Initial()
    {
        for (int i = 0; i < _allNPCs.Length; i++)
        {
            _allNPCs[i].Initial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
