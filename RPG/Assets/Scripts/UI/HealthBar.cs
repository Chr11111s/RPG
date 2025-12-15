using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    EnemyCtrl _enemy;
    Slider _slider;

    public void Initial(EnemyCtrl target)
    {
        _slider = GetComponent<Slider>();
        _enemy = target;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_enemy.transform.position);
        transform.position = screenPos + Vector3.up * 50;

        _slider.value = ((float)_enemy.SelfInfo.hp) / (float)_enemy.SelfInfo.maxhp;
    }
}
