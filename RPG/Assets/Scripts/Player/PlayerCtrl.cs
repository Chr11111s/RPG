using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public enum PlayerStateEnum
{
    发呆,
    跑步,
    互动,
    准备施法,
    施法,
    攻击,
    受伤,
    死亡
}

public class PlayerCtrl : MonoBehaviour
{
    Animator _selfAnim;
    PlayerStateEnum _currentState;
    NavMeshAgent _selfAgent;
    Vector3 _desPos;
    float _stopDistance = 0.3f;
    Transform _firePos;
    GameObject _originFireBall;

    List<SkillCtrl> _skillList;
    public static PlayerCtrl Instance;

    int _groundLayer;
    int _unitLayer;

    Vector3 _groundPoint;

    SkillCtrl _currentSkill;

    float _workTime;

    EnemyCtrl _currentEnemy;

    NPCCtrl _currentNPC;

    PlayerInfo _currentPlayerInfo;

    const float _interactDistance = 5f;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _selfAnim = GetComponent<Animator>();
        _selfAgent = GetComponent<NavMeshAgent>();
        _firePos = transform.Find("root/pelvis/Weapon/FirePos");
        //_originFireBall = transform.Find("Ball2").gameObject;
        _groundLayer = LayerMask.GetMask("Ground");
        _unitLayer = LayerMask.GetMask("Unit");

        enabled = false; //确保在开始界面鼠标点击按钮时Player不会跟着动
    }

    public void Initial()
    {
        PlayerCamera.Instance.Initial();
        MapCamera.Instance.Initial();
        _skillList = new List<SkillCtrl>();
        Transform skillRoot = transform.Find("Skills");
        for (int i = 0; i < skillRoot.childCount; i++)
        {
            var tmpSkill = skillRoot.GetChild(i).GetComponent<SkillCtrl>();
            _skillList.Add(tmpSkill);
            tmpSkill.gameObject.SetActive(true);
        }

        _currentPlayerInfo = DataManager.Instance.PlayerInfo;
        _currentPlayerInfo.Reset();

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        MouseDetect();
        MouseInput();
        KeyTest();
        Recover();
        SyncInfo();
    }

    private void Recover()
    {
        _currentPlayerInfo.Recover();
    }

    private void SyncInfo()
    {
        if (_currentPlayerInfo == null)
        {
            return;
        }

        WndManager.Instance.GetWnd<CharacterWnd>().UpdatePlayerInfo(); //删掉形参_currentPlayerInfo
        WndManager.Instance.GetWnd<BattleWnd>().UpdatePlayerInfo();
    }

    private void KeyTest()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchState(PlayerStateEnum.攻击);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseSkill(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UseItem(5);

        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            UseItem(6);

        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            UseItem(7);

        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            UseItem(8);

        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UseItem(9);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ItemInfo item = DataManager.Instance.GetNewItem(0);
            WndManager.Instance.GetWnd<InventoryWnd>().PickUp(item);
            item = DataManager.Instance.GetNewItem(1);
            WndManager.Instance.GetWnd<InventoryWnd>().PickUp(item);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            WndManager.Instance.OpenWnd<InventoryWnd>();
            WndManager.Instance.GetWnd<MouseWnd>().Show();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            WndManager.Instance.OpenWnd<EquipWnd>();
            WndManager.Instance.GetWnd<MouseWnd>().Show();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            WndManager.Instance.OpenWnd<SkillWnd>();
            WndManager.Instance.GetWnd<MouseWnd>().Show();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            WndManager.Instance.OpenWnd<CharacterWnd>();
            WndManager.Instance.GetWnd<MouseWnd>().Show();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            _currentPlayerInfo.LvUp();
            WndManager.Instance.GetWnd<CharacterWnd>().ShowLvUpInfo();
            WndManager.Instance.GetWnd<SkillWnd>().UpdatePlayerInfo();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            WndManager.Instance.OpenWnd<JobWnd>();
            WndManager.Instance.GetWnd<MouseWnd>().Show();
        }
    }

    private void UseItem(int index)
    {
        WndManager.Instance.GetWnd<BattleWnd>().Use(index);
    }

    void UseSkill(int skillIndex)
    {
        if (!_currentPlayerInfo.skillids.Contains(_skillList[skillIndex].SkillInfo.skillid))
        {
            return;
        }

        if (_skillList[skillIndex].SkillInfo.IsCD)
        {
            return;
        }

        _currentSkill = _skillList[skillIndex];

        if (_skillList[skillIndex].SkillInfo.SkillPointSort == SkillPointSortEnum.AOE || _skillList[skillIndex].SkillInfo.SkillPointSort == SkillPointSortEnum.指向性技能)
        {
            SwitchState(PlayerStateEnum.准备施法);
            _skillList[skillIndex].StartToCast();
        }
        else
        {
            //_skillList[skillIndex].Cast();
            SwitchState(PlayerStateEnum.施法);
        }
    }

    private void MouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //防止打开面板鼠标进行点击操作时Player跟随移动
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_currentSkill != null)
            {
                if (_currentState == PlayerStateEnum.准备施法)
                {
                    if (_currentSkill.SkillInfo.SkillPointSort == SkillPointSortEnum.AOE)
                    {
                        transform.LookAt(_groundPoint, Vector3.up);
                        SwitchState(PlayerStateEnum.施法);
                    }
                    else if (_currentSkill.SkillInfo.SkillPointSort == SkillPointSortEnum.指向性技能 && _currentEnemy != null)
                    {
                        transform.LookAt(_currentEnemy.transform);
                        SwitchState(PlayerStateEnum.施法);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(_currentState == PlayerStateEnum.准备施法)
            {
                _currentSkill.CancelCast();
                SwitchState(PlayerStateEnum.发呆);
            }
            else if (_currentState == PlayerStateEnum.施法)
            {
                _currentSkill.EndCast();
            }
            else
            {
                if (_currentNPC != null)
                {
                    float distance = Vector3.Distance(transform.position, _currentNPC.transform.position);
                    if (distance <= _interactDistance)
                    {
                        SwitchState(PlayerStateEnum.互动);
                        return;
                    }
                }
                SwitchState(PlayerStateEnum.跑步);
            }
        }
    }

    private void MouseDetect()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //使用射线检测
        RaycastHit hitInfo;

        bool hasEnemy = false;
        bool hasNPC = false;

        if (Physics.Raycast(mouseRay, out hitInfo))
        {
            _desPos = hitInfo.point;
            EnemyCtrl enemy=hitInfo.transform.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                _currentEnemy = enemy;
                hasEnemy = true;
            }

            NPCCtrl npc = hitInfo.transform.GetComponent<NPCCtrl>();
            if (npc != null)
            {
                _currentNPC = npc;
                hasNPC = true;
            }
        }

        if (!hasNPC && _currentNPC != null)
        {
            _currentNPC = null;
        }

        if (!hasEnemy && _currentEnemy != null) //解决移动时释放技能报错问题
        {
            if (_currentState != PlayerStateEnum.施法)
            {
                _currentEnemy = null;
            }         
        }

        Ray groundRay=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHitInfo;
        if(Physics.Raycast(groundRay, out groundHitInfo, 1000, _groundLayer))
        {
            _groundPoint = groundHitInfo.point;
        }
    }

    private void StateMachine()
    {
        switch (_currentState)
        {
            case PlayerStateEnum.发呆:
                Idling();
                break;
            case PlayerStateEnum.跑步:
                Running();
                break;
            case PlayerStateEnum.互动:
                Interacting();
                break;
            case PlayerStateEnum.准备施法:
                ReadyToAttacking();
                break;
            case PlayerStateEnum.施法:
                Spelling();
                break;
            case PlayerStateEnum.攻击:
                Attacking();
                break;
            case PlayerStateEnum.受伤:
                GettingHit();
                break;
            case PlayerStateEnum.死亡:
                Dying();
                break;
        }
    }

    private void Interacting()
    {
        
    }

    private void Spelling()
    {
        
    }

    private void ReadyToAttacking()
    {
        if (_currentSkill.SkillInfo.SkillPointSort == SkillPointSortEnum.AOE)
        {
            _currentSkill.SetReadyPos(_groundPoint);
        }
    }
    
    void SwitchState(PlayerStateEnum targetState)
    {
        //转换前需要做的事情
        switch (_currentState)
        {
            case PlayerStateEnum.发呆:
                break;
            case PlayerStateEnum.跑步:
                break;
            case PlayerStateEnum.互动:
                break;
            case PlayerStateEnum.准备施法:
                break;
            case PlayerStateEnum.施法:
                break;
            case PlayerStateEnum.攻击:
                break;
            case PlayerStateEnum.受伤:
                break;
            case PlayerStateEnum.死亡:
                break;
        }

        //转换时需要做的事情
        switch (targetState)
        {
            case PlayerStateEnum.发呆:
                Idle();
                break;
            case PlayerStateEnum.跑步:
                Run();
                break;
            case PlayerStateEnum.互动:
                Interact();
                break;
            case PlayerStateEnum.准备施法:
                ReadyToAttack();
                break;
            case PlayerStateEnum.施法:
                Spell();
                break;
            case PlayerStateEnum.攻击:
                Attack();
                break;
            case PlayerStateEnum.受伤:
                GetHit();
                break;
            case PlayerStateEnum.死亡:
                Die();
                break;
        }

        _currentState = targetState;

        //转换以后需要做的事情
        switch (_currentState)
        {
            case PlayerStateEnum.发呆:
                break;
            case PlayerStateEnum.跑步:
                break;
            case PlayerStateEnum.互动:
                break;
            case PlayerStateEnum.准备施法:
                break;
            case PlayerStateEnum.施法:
                break;
            case PlayerStateEnum.攻击:
                break;
            case PlayerStateEnum.受伤:
                break;
            case PlayerStateEnum.死亡:
                break;
        }
    }

    private void Interact()
    {
        _currentNPC.Interact();
    }

    private void Spell()
    {
        if (_currentSkill.SkillInfo.SkillCastSort == SkillCastSortEnum.瞬间施法)
        {
            _selfAnim.SetTrigger("SAttack");
        }
        else
        {
            _selfAnim.SetBool("MAttack", true);
        }
    }

    public void EndSpell()
    {
        _selfAnim.SetBool("MAttack", false);
        SwitchState(PlayerStateEnum.发呆);
    }

    private void ReadyToAttack()
    {
        
    }

    private void Die()
    {
        _selfAnim.SetTrigger("Die");
    }

    public void ToDie()
    {
        Debug.Log("死了");
    }

    private void GetHit()
    {
        
    }

    private void Attack()
    {
        _selfAnim.SetTrigger("Attack");
    }

    private void Run()
    {
        _selfAnim.SetBool("Run", true);
        _selfAgent.SetDestination(_desPos);
    }

    private void Idle()
    {
        _selfAnim.SetBool("Run", false);
    }

    private void Dying()
    {
        
    }

    private void GettingHit()
    {
        
    }

    private void Attacking()
    {
        
    }

    private void Running()
    {
        if (_selfAgent.remainingDistance <= _stopDistance)
        {
            SwitchState(PlayerStateEnum.发呆);
        }
    }

    private void Idling()
    {
        
    }

    //将之前的Spell()方法挪到CastSkill()中，在Spell()中调用动画
    public void CastSkill()
    {
        _workTime = 0;
        switch (_currentSkill.SkillInfo.SkillPointSort)
        {
            case SkillPointSortEnum.指向性技能:
                transform.LookAt(_currentEnemy.transform);
                _currentSkill.Cast(_currentEnemy.transform);
                WndManager.Instance.GetWnd<MouseWnd>().SwitchToNormal();
                break;
            case SkillPointSortEnum.AOE:
                _currentSkill.Cast(_groundPoint);
                break;
            case SkillPointSortEnum.Buff:
                _currentSkill.Cast();
                break;
        }

        if(_currentSkill.SkillInfo.SkillCastSort == SkillCastSortEnum.瞬间施法)
        {
            SwitchState(PlayerStateEnum.发呆);
        }
    }

    public void Fire()
    {
        GameObject cloneBall = Instantiate(_originFireBall);
        cloneBall.transform.position = _firePos.position;
        cloneBall.transform.rotation = transform.rotation;
        cloneBall.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        _currentPlayerInfo.currenthp -= damage;

        WndManager.Instance.GetWnd<BattleWnd>().AddDamageTip(transform, damage.ToString());

        if (_currentPlayerInfo.currenthp <= 0)
        {
            SwitchState(PlayerStateEnum.死亡);
        }
    }
}