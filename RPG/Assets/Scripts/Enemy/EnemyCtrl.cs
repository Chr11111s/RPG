using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStateSortEnum
{
    ·¢´ô,
    Ñ²Âß,
    ×·»÷,
    ¹¥»÷,
    ÊÜÉË,
    Ñ£ÔÎ,
    ËÀÍö
}

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField]
    Transform[] _patrollPoints;

    [SerializeField]
    int _enemyID;

    float _workTime;
    int _patrollIndex;
    const float IdleDuring = 2f;
    const float DetectRange = 10f;
    const float AttackRange = 3f;

    //float DPS = 5f;

    EnemyStateSortEnum _currentState;
    NavMeshAgent _selfAgent;
    Animator _selfAnim;

    public EnemyInfo SelfInfo { get; private set; }

    // Start is called before the first frame update

    /*void Start()
    {
        
    }*/

    public void Initial()
    {
        //½«Start()ÄÚ´úÂëÒÆÖÁ´Ë´¦
        _selfAgent = GetComponent<NavMeshAgent>();
        _selfAnim = GetComponent<Animator>();

        enabled = true;
        SelfInfo = DataManager.Instance.GetNewEnemy(_enemyID);
        _selfAgent.speed = (float)SelfInfo.movespeed;

        WndManager.Instance.GetWnd<BattleWnd>().AddHealthBar(this);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        KeyTest();
    }

    private void KeyTest()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(3);
        }
    }

    void SwitchState(EnemyStateSortEnum state)
    {
        switch (_currentState)
        {
            case EnemyStateSortEnum.·¢´ô:
                break;
            case EnemyStateSortEnum.Ñ²Âß:
                break;
            case EnemyStateSortEnum.×·»÷:
                break;
            case EnemyStateSortEnum.¹¥»÷:
                break;
            case EnemyStateSortEnum.ÊÜÉË:
                break;
            case EnemyStateSortEnum.Ñ£ÔÎ:
                break;
            case EnemyStateSortEnum.ËÀÍö:
                break;
        }


        switch (state)
        {
            case EnemyStateSortEnum.·¢´ô:
                Idle();
                break;
            case EnemyStateSortEnum.Ñ²Âß:
                Patroll();
                break;
            case EnemyStateSortEnum.×·»÷:
                Pursuit();
                break;
            case EnemyStateSortEnum.¹¥»÷:
                Attack();
                break;
            case EnemyStateSortEnum.ÊÜÉË:
                GetHit();
                break;
            case EnemyStateSortEnum.Ñ£ÔÎ:
                Dizzy();
                break;
            case EnemyStateSortEnum.ËÀÍö:
                Die();
                break;
        }

        _currentState = state;

        switch (_currentState)
        {
            case EnemyStateSortEnum.·¢´ô:
                break;
            case EnemyStateSortEnum.Ñ²Âß:
                break;
            case EnemyStateSortEnum.×·»÷:
                break;
            case EnemyStateSortEnum.¹¥»÷:
                break;
            case EnemyStateSortEnum.ÊÜÉË:
                break;
            case EnemyStateSortEnum.Ñ£ÔÎ:
                break;
            case EnemyStateSortEnum.ËÀÍö:
                break;
        }
    }

    private void Die()
    {
        _selfAnim.SetTrigger("Die");
        GetComponent<SphereCollider>().enabled = false; //½«Åö×²Ìå¹ØµôÒÔ·À·´¸´²¥·ÅËÀÍö¶¯»­
        DataManager.Instance.GetTarget(_enemyID, JobSortEnum.»÷É±µÐÈË);
        DataManager.Instance.PlayerInfo.EarnExp(SelfInfo.exp);
    }

    public void ToDie()
    {
        Destroy(gameObject);
    }

    private void Dizzy()
    {
        
    }

    private void GetHit()
    {
        
    }

    private void Attack()
    {
        _selfAgent.SetDestination(transform.position);
        _selfAnim.SetBool("Attack", true);
    }

    private void Pursuit()
    {
        _selfAnim.SetBool("Run", true);
        _selfAgent.SetDestination(PlayerCtrl.Instance.transform.position);
    }

    private void Patroll()
    {
        if (_patrollPoints.Length == 0)
        {
            SwitchState(EnemyStateSortEnum.·¢´ô);
        }
        else
        {
            _selfAnim.SetBool("Run", true);
            _patrollIndex++;

            if (_patrollIndex == _patrollPoints.Length)
            {
                _patrollIndex = 0;
            }

            Transform currentPatrollPoint = _patrollPoints[_patrollIndex];

            _selfAgent.SetDestination(currentPatrollPoint.position);
        }
    }

    private void Idle()
    {
        _selfAgent.SetDestination(transform.position);
        _selfAnim.SetBool("Run", false);
    }

    private void StateMachine()
    {
        switch (_currentState)
        {
            case EnemyStateSortEnum.·¢´ô:
                Idling();
                break;
            case EnemyStateSortEnum.Ñ²Âß:
                Patrolling();
                break;
            case EnemyStateSortEnum.×·»÷:
                Pursuiting();
                break;
            case EnemyStateSortEnum.¹¥»÷:
                Attacking();
                break;
            case EnemyStateSortEnum.ÊÜÉË:
                GetHiting();
                break;
            case EnemyStateSortEnum.Ñ£ÔÎ:
                Dizzying();
                break;
            case EnemyStateSortEnum.ËÀÍö:
                Dyixng();
                break;
        }
    }

    private void Dyixng()
    {
        
    }

    private void Dizzying()
    {
        
    }

    private void GetHiting()
    {
        
    }

    private void Attacking()
    {
        float distance = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position);

        if (distance > AttackRange)
        {
            _selfAnim.SetBool("Attack", false);
            SwitchState(EnemyStateSortEnum.×·»÷);
        }
    }

    private void Pursuiting()
    {
        float distance = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position);

        _selfAgent.SetDestination(PlayerCtrl.Instance.transform.position);

        transform.LookAt(PlayerCtrl.Instance.transform.position);

        if (distance < AttackRange)
        {
            _selfAnim.SetBool("Run", false);
            SwitchState(EnemyStateSortEnum.¹¥»÷);
        }
        else if (distance > DetectRange)
        {
            SwitchState(EnemyStateSortEnum.Ñ²Âß);
        }
    }

    private void Patrolling()
    {
        float distance = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position);

        if (distance <= DetectRange)
        {
            SwitchState(EnemyStateSortEnum.×·»÷);
        }
    }

    private void Idling()
    {
        _workTime += Time.deltaTime;

        if (_workTime >= IdleDuring)
        {
            _workTime = 0;
            SwitchState(EnemyStateSortEnum.Ñ²Âß);
        }

        float distance = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position);

        if (distance <= DetectRange)
        {
            SwitchState(EnemyStateSortEnum.×·»÷);
        }
    }

    public void AttackPlayer()
    {
        PlayerCtrl.Instance.TakeDamage(SelfInfo.dps); //½«DPS¸ÄÎª_selfInfo.dps
    }

    public void TakeDamage(int damage)
    {
        SelfInfo.hp -= damage;

        WndManager.Instance.GetWnd<BattleWnd>().AddDamageTip(transform, damage.ToString());

        if (SelfInfo.hp <= 0)
        {
            SwitchState(EnemyStateSortEnum.ËÀÍö);
        }
    }
}
