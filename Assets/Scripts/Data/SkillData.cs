using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/New Skill")]
public class SkillData : ScriptableObject
{
    #region
    /*  ��ų������ SkillData
     *  ��ų�� ����(�Ķ����)�� ���, ������� X
     *  ��ų�̸�, ������, ����
     *  ��Ÿ��, �Ҹ��ڿ�, ��Ÿ�, ����, �����ð�
     *  ȿ�� Ÿ��(����, ȸ��, ����, ����� ��)
     *  �ִϸ��̼�/����Ʈ ������ ����
    */
    #endregion

    [Header("�⺻ ����")]
    public string SkillName;
    public Sprite Icon;
    public float Cost;
    public float Range;
    public float CastTime;
    public float Cooldown;
    public GameObject EffectPrefab;

    [Header("ȿ���� (���� �� ����)")]
    public List<SkillEffect> Effects = new List<SkillEffect>();
    //List �߰��ؼ� Type�� 2�� �̻�

    [Header("���� ����")]
    public int RequireLevel;
    public int RequireSP;
    public List<SkillData> PrerequisiteSkills;

    [Header("��ų ����")]
    public int MaxLevel = 1; //�ִ� ����

    [Header("�ִϸ��̼�")]
    public string AnimationName;
}

[System.Serializable]
public class SkillEffect
{
    public SkillEffectType EffectType;
    public float Power;    //������, ȸ�� ��ġ
    public float Duration; //����, �����, CC�� ���ӽð�
    public float Radius;   //���� ���� �ݰ�
    public float Distance; //�Ÿ�
}

public enum SkillEffectType
{
    Damage,
    Heal,
    AtkBuff,
    DefBuff,
    CriBuff,
    TotalBuff,
    Debuff,
    CC,
    Resource,
    Movement,
    Teleport,
}
