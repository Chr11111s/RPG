using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Data;

public class PlayerInfo
{
    public string username;
    public int lv;
    public int strength;
    public int agility;
    public int intelligence;
    //public double maxhp;
    //public double maxmp;
    public double originhp;
    public double originmp;
    public int exp;
    public int point;
    //public double hprecoverspeed;
    //public double mprecoverspeed;
    public double originhprecoverspeed;
    public double originmprecoverspeed;
    //public double movespeed;
    public double originmovespeed;
    public float currenthp; //不写成double,因为是实时变化的所以不在json里写，在游戏里写就行
    public float currentmp; //不写成double,因为是实时变化的所以不在json里写，在游戏里写就行
    public int currents;
    public int currenta;
    public int currenti;
    public int skillpoint;
    public List<int> skillids;

    public void LearnSkill(int skillid)
    {
        skillids.Add(skillid);
    }


    public float MAXHP
    {
        get
        {
            return (float)originhp + currents * 10;
        }
    }

    public float MAXMP
    {
        get
        {
            return (float)originmp + currenti * 20;
        }
    }

    public int LvUpExp
    {
        get
        {
            return lv * 100;
        }
    }

    public float HPRecoverSpeed
    {
        get
        {
            return (float)originhprecoverspeed + currents * 0.5f * Time.deltaTime;
        }
    }

    public float MPRecoverSpeed
    {
        get
        {
            return (float)originmprecoverspeed + currenti * 0.5f * Time.deltaTime;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return (float)originmovespeed + currenta * 5;
        }
    }

    public void EarnExp(int newExp)
    {
        exp += newExp;
    }

    public void Recover()
    {
        currenthp += HPRecoverSpeed;
        if (currenthp > MAXHP)
        {
            currenthp = MAXHP;
        }

        currentmp += MPRecoverSpeed;
        if (currentmp > MAXMP)
        {
            currentmp = MAXMP;
        }

    }

    public void LvUp()
    {
        exp -= LvUpExp;
        if (exp < 0)
        {
            exp = 0;
        }
        lv++;
        point += 5;
        skillpoint++;
    }

    public void Reset()
    {
        currenthp = MAXHP;
        currentmp = MAXMP;
        currents = strength;
        currenta = agility;
        currenti = intelligence;
    }

    public void UseItem(ItemInfo itemInfo)
    {
        currenthp += itemInfo.hp;
        if (currenthp > MAXHP)
        {
            currenthp = MAXHP;
        }
        currentmp += itemInfo.mp;
        if (currenthp > MAXMP)
        {
            currentmp = MAXMP;
        }
        strength += itemInfo.strength;
        agility += itemInfo.agility;
        intelligence += itemInfo.intelligence;
    }

    public void UnUseItem(ItemInfo itemInfo)
    {

        strength -= itemInfo.strength;
        agility -= itemInfo.agility;
        intelligence -= itemInfo.intelligence;
        if (currenthp > MAXHP)
        {
            currenthp = MAXHP;
        }
        if (currenthp > MAXMP)
        {
            currentmp = MAXMP;
        }
    }
}

public enum SkillCastSortEnum
{
    瞬间施法,
    持续性施法
}

public enum SkillDamageSortEnum
{
    一次性伤害,
    持续性伤害
}

public enum SkillMoveSortEnum
{
    直线向前,
    环绕角色,
    跟随角色,
    原地不动
}

public enum SkillPointSortEnum
{
    指向性技能,
    AOE,
    Buff
}

public class SkillInfo
{
    public int skillid;
    public string skillname;
    public string skilldesc;
    public string skillspname;
    public int skillcastsortindex;
    public int skillmovesortindex;
    public int skillpointsortindex;
    public double during;
    public double cd;
    public double movespeed;
    public int skilldamagesortindex;
    public double skillradius;
    public double skilldamage;
    float _cdWorkTime;

    public bool IsCD
    {
        get
        {
            return _cdWorkTime < CD;
        }
    }

    public void Initial()
    {
        _cdWorkTime = CD;
    }

    public void CoolDown()
    {
        _cdWorkTime += Time.deltaTime;
    }

    public void UseSkill()
    {
        _cdWorkTime = 0;
    }

    public float CDPercent
    {
        get
        {
            if (_cdWorkTime < CD)
            {
                return _cdWorkTime / CD;
            }
            else
            {
                return 1;
            }
        }
    }

    public float SkillRadius
    {
        get
        {
            return (float)skillradius;
        }
    }

    public float SkillDamage
    {
        get
        {
            return (float)skilldamage;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return (float)movespeed;
        }
    }

    public float During
    {
        get
        {
            return (float)during;
        }
    }

    public float CD
    {
        get
        {
            return (float)cd;
        }
    }

    public Sprite SkillSp
    {
        get
        {
            return Resources.Load<Sprite>("Icon/Skill/" + skillspname);
        }
    }

    public SkillCastSortEnum SkillCastSort
    {
        get
        {
            return (SkillCastSortEnum)skillcastsortindex;
        }
    }

    public SkillMoveSortEnum SkillMoveSort
    {
        get
        {
            return (SkillMoveSortEnum)skillmovesortindex;
        }
    }

    public SkillPointSortEnum SkillPointSort
    {
        get
        {
            return (SkillPointSortEnum)skillpointsortindex;
        }
    }

    public SkillDamageSortEnum SkillDamageSort
    {
        get
        {
            return (SkillDamageSortEnum)skilldamagesortindex;
        }
    }
}

public class EnemyInfo
{
    public int id;
    public string enemyname;
    public int movespeed;
    public int hp;
    public int maxhp;
    public int dps;
    public int exp;

    public EnemyInfo()
    {

    }

    public EnemyInfo(EnemyInfo enemyInfo)
    {
        id = enemyInfo.id;
        enemyname = enemyInfo.enemyname;
        movespeed = enemyInfo.movespeed;
        hp = enemyInfo.hp;
        dps = enemyInfo.dps;
        maxhp= enemyInfo.maxhp;
        exp = enemyInfo.exp;
    }
}

public class ItemInfo
{
    public string itemname;
    public int itemid;
    public int itemsortindex;
    public string itemdesc;
    public string itemspname;
    public int itemcount;
    public int hp;
    public int mp;
    public int strength;
    public int agility;
    public int intelligence;

    public ItemInfo()
    {

    }

    public ItemInfo(ItemInfo itemInfo)
    {
        this.itemname = itemInfo.itemname;
        this.itemid = itemInfo.itemid;
        this.itemsortindex = itemInfo.itemsortindex;
        this.itemdesc = itemInfo.itemdesc;
        this.itemspname = itemInfo.itemspname;
        hp = itemInfo.hp;
        mp = itemInfo.mp;
        strength = itemInfo.strength;
        agility = itemInfo.agility;
        intelligence = itemInfo.intelligence;
        itemcount = 1;
    }

    public Sprite ItemSp
    {
        get
        {
            return Resources.Load<Sprite>("Icon/" + itemspname);
        }
    }

    public void Use()
    {
        itemcount--;
    }

    public ItemSortEnum ItemSort
    {
        get
        {
            return (ItemSortEnum)itemsortindex;
        }
    }
}



public enum ItemSortEnum
{
    消耗品,
    头,
    手套,
    衣服,
    鞋子,
    耳环,
    戒指
}

public enum EquipSortEnum
{
    头,
    左手手套,
    右手手套,
    衣服,
    左鞋子,
    右鞋子,
    左耳环,
    右耳环,
    左手戒指,
    右手戒指
}

public class NPCInfo
{
    public int id;
    public string name;
    public List<int> jobids;
    public int jobindex;
    public string[] common;

    public NPCInfo()
    {

    }

    public NPCInfo(NPCInfo npcInfo)
    {
        id = npcInfo.id;
        name = npcInfo.name;
        jobids = npcInfo.jobids;
        jobindex = npcInfo.jobindex;
        common = npcInfo.common;
    }

    public int CurrentJobID
    {
        get
        {
            if (jobids.Count == 0)
            {
                return -1;
            }
            else
            {
                if (jobindex < jobids.Count)
                {
                    return jobids[jobindex];
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}

public enum JobSortEnum
{
    搜集物品,
    与NPC谈话,
    击杀敌人
}

public enum JobStateEnum
{
    未领取,
    已领取,
    已完成
}

public class JobInfo
{
    public int id;
    public string name;
    public string desc;
    public int jobsortindex;
    public int jobstateindex;
    public int finish;
    public List<int> targetids;
    public List<int> targetcount;
    public List<int> finishcount;
    public int exp;

    public JobInfo() //必须存在无参，json转需要
    {

    }

    public JobInfo(JobInfo jobInfo)
    {
        id = jobInfo.id;
        name = jobInfo.name;
        desc = jobInfo.desc;
        jobsortindex = jobInfo.jobsortindex;
        jobstateindex = jobInfo.jobstateindex;
        finish = jobInfo.finish;
        targetids = jobInfo.targetids;
        targetcount = jobInfo.targetcount;
        finishcount = jobInfo.finishcount;
        exp = jobInfo.exp;
    }

    public bool IsFinish
    {
        get
        {
            bool finish = true;

            for (int i = 0; i < targetcount.Count; i++)
            {
                if (targetcount[i] != finishcount[i])
                {
                    finish = false;
                    break;
                }
            }
            return finish;
        }
    }

    public void GetTarget(int targetID)
    {
        int index = targetids.IndexOf(targetID);
        if (index > -1)
        {
            finishcount[index]++;
        }
    }

    public JobStateEnum JobState
    {
        get
        {
            return (JobStateEnum)jobstateindex;
        }
    }

    public JobSortEnum JobSortEnum
    {
        get
        {
            return (JobSortEnum)jobsortindex;
        }
    }

    public void SwitchState(JobStateEnum state)
    {
        jobstateindex = (int)state;
    }
}

public class DataManager : Singleton<DataManager>
{
    List<ItemInfo> _itemInfoList;
    List<SkillInfo> _skillInfoList;
    List<EnemyInfo> _enemyInfoList;
    List<NPCInfo> _npcInfoList;
    List<JobInfo> _jobInfoList;

    public PlayerInfo PlayerInfo { get; private set; }

    List<JobInfo> _currentJobList;
    public void Initial()
    {
        string filePath = Application.streamingAssetsPath + "/item.json";
        string jsonData=File.ReadAllText(filePath);
        _itemInfoList = JsonConvert.DeserializeObject<List<ItemInfo>>(jsonData);

        filePath = Application.streamingAssetsPath + "/skill.json";
        jsonData = File.ReadAllText(filePath);
        _skillInfoList = JsonConvert.DeserializeObject<List<SkillInfo>>(jsonData);

        filePath = Application.streamingAssetsPath + "/player.json";
        jsonData = File.ReadAllText(filePath);
        PlayerInfo = JsonConvert.DeserializeObject<PlayerInfo>(jsonData);

        filePath = Application.streamingAssetsPath + "/enemy.json";
        jsonData = File.ReadAllText(filePath);
        _enemyInfoList = JsonConvert.DeserializeObject<List<EnemyInfo>>(jsonData);

        filePath = Application.streamingAssetsPath + "/npc.json";
        jsonData = File.ReadAllText(filePath);
        _npcInfoList = JsonConvert.DeserializeObject<List<NPCInfo>>(jsonData);

        filePath = Application.streamingAssetsPath + "/job.json";
        jsonData = File.ReadAllText(filePath);
        _jobInfoList = JsonConvert.DeserializeObject<List<JobInfo>>(jsonData);

        _currentJobList = new List<JobInfo>();
    }

    public EnemyInfo GetNewEnemy(int enemyID)
    {
        EnemyInfo originEnemy = _enemyInfoList.Find(tmp => tmp.id == enemyID);
        EnemyInfo cloneEnemy = new EnemyInfo(originEnemy);
        return cloneEnemy;
    }

    public NPCInfo GetNewNPC(int npcID)
    {
        NPCInfo originNPC = _npcInfoList.Find(tmp => tmp.id == npcID);
        NPCInfo cloneNPC = new NPCInfo(originNPC);
        return cloneNPC;
    }

    public JobInfo GetNewJob(int jobID)
    {
        JobInfo originJob = _jobInfoList.Find(tmp => tmp.id == jobID);
        JobInfo cloneJob = new JobInfo(originJob);
        _currentJobList.Add(cloneJob);
        return cloneJob;
    }

    public void GetTarget(int targetID, JobSortEnum jobSort)
    {
        JobInfo jobInfo = _currentJobList.Find(tmp =>
            tmp.targetids.Contains(targetID) && tmp.JobSortEnum == jobSort && tmp.JobState == JobStateEnum.已领取);
        //Debug.Log($"GetTarget call: id={targetID}, sort={jobSort}");
            
        if (jobInfo != null)
        {
            //Debug.Log($"Hit job {jobInfo.id}, before={string.Join(",", jobInfo.finishcount)}");
            jobInfo.GetTarget(targetID);
        }
    }

    public ItemInfo GetItem(int itemID)
    {
        return _itemInfoList.Find(tmp => tmp.itemid == itemID);
    }

    public ItemInfo GetNewItem(int itemID)
    {
        ItemInfo originItem = GetItem(itemID);
        ItemInfo newItem = new ItemInfo(originItem);
        return newItem;
    }

    public SkillInfo GetSkill(int skillID)
    {
        return _skillInfoList.Find(tmp => tmp.skillid == skillID);
    }
}
