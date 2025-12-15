using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCtrl : MonoBehaviour
{
    public int _skillID;

    float _moveSpeed = 20f;

    public SkillInfo SkillInfo { get; private set; }

    Transform _ready;
    Transform _effect;
    Transform _followEffect;

    Transform _target;

    float _workTime;
    public bool Casting { get; private set; }

    /*float _cdWorkTime;
    public bool IsCD { get; private set; }*/

    int _enemyLayer;

    float _damageWorkTime;

    const float DamageDuring = 1f;

    public void StartToCast()
    {
        switch (SkillInfo.SkillPointSort)
        {
            case SkillPointSortEnum.指向性技能:
                {
                    WndManager.Instance.GetWnd<MouseWnd>().SwitchToSelect();
                }
                break;
            case SkillPointSortEnum.AOE:
                {
                    _ready.SetParent(null);
                    _ready.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void CancelCast()
    {
        switch (SkillInfo.SkillPointSort)
        {
            case SkillPointSortEnum.指向性技能:
                {
                    WndManager.Instance.GetWnd<MouseWnd>().SwitchToNormal();
                }
                break;
            case SkillPointSortEnum.AOE:
                {
                    _ready.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void Cast()
    {
        _damageWorkTime = DamageDuring; //一释放就能立即造成伤害
        _workTime = 0;
        _effect.gameObject.SetActive(true);
        //_cdWorkTime = 0;
        SkillInfo.UseSkill();
        Casting = true;
        //IsCD = true;
    }

    public void Cast(Transform target)
    {
        Cast();
        _target = target;
        _effect.transform.position = transform.position;
        _effect.SetParent(null); //effect在引擎内是Player子节点，防止攻击特效脱离Player后不锁定enemy还跟着Player视角转弯
        _effect.transform.LookAt(target); //防止攻击特效脱离Player后不锁定enemy还跟着Player视角转弯
    }

    internal void Cast(Vector3 pos)
    {
        Cast();
        _ready.gameObject.SetActive(false);
        _effect.SetParent(null);
        _effect.transform.position = pos;
    }

    public void EndCast()
    {
        _effect.gameObject.SetActive(false);
        Casting = false;

        if (SkillInfo.SkillCastSort == SkillCastSortEnum.持续性施法)
        {
            PlayerCtrl.Instance.EndSpell();
        }
    }

    public void SetReadyPos(Vector3 pos)
    {
        _ready.transform.position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");

        SkillInfo = DataManager.Instance.GetSkill(_skillID);

        SkillInfo.Initial();

        _ready = transform.Find("Ready");
        _effect = transform.Find("Effect");
        _followEffect = transform.Find("FollowEffect");
    }

    // Update is called once per frame
    void Update()
    {
        if (Casting)
        {
            Moving();
            Damaging();
        }

        /*_cdWorkTime += Time.deltaTime;
        if(_cdWorkTime >= SkillInfo.CD)
        {
            IsCD = false;
        }替换为下面这一行即可*/
        SkillInfo.CoolDown();
    }

    private void Damaging()
    {
        _damageWorkTime += Time.deltaTime;

        switch (SkillInfo.SkillDamageSort)
        {
            case SkillDamageSortEnum.一次性伤害:
                {
                    Collider[] enemies = Physics.OverlapSphere(_effect.transform.position, SkillInfo.SkillRadius, _enemyLayer);
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        EnemyCtrl enemyCtrl = enemies[i].GetComponent<EnemyCtrl>();
                        if (enemyCtrl != null)
                        {
                            enemyCtrl.TakeDamage((int)SkillInfo.SkillDamage);
                            EndCast();
                        }
                    }
                }
                break;
            case SkillDamageSortEnum.持续性伤害:
                {
                    Collider[] enemies = Physics.OverlapSphere(_effect.transform.position, SkillInfo.SkillRadius, _enemyLayer);
                    if (_damageWorkTime >= DamageDuring)
                    {
                        _damageWorkTime = 0;
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            EnemyCtrl enemyCtrl = enemies[i].GetComponent<EnemyCtrl>();
                            if (enemyCtrl != null)
                            {
                                enemyCtrl.TakeDamage((int)SkillInfo.SkillDamage);
                            }                           
                                //没有EndCast()
                        }
                    }
                }
                break;
        }
    }

    void Moving()
    {
        switch (SkillInfo.SkillMoveSort)
        {
            case SkillMoveSortEnum.直线向前:
                _effect.transform.position += _effect.transform.forward * Time.deltaTime * SkillInfo.MoveSpeed;
                break;
            case SkillMoveSortEnum.环绕角色:
                break;
            case SkillMoveSortEnum.跟随角色:
                break;
            case SkillMoveSortEnum.原地不动:
                break;
        }

        _workTime += Time.deltaTime;
        if (_workTime >= SkillInfo.During)
        {
            _workTime = 0;
            EndCast();
        }
    }
}
