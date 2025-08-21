using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/New Skill")]
public class SkillData : ScriptableObject
{
    /*  ��ų������ SkillData
     *  ��ų�� ����(�Ķ����)�� ���, ������� X
     *  ��ų�̸�, ������, ����
     *  ��Ÿ��, �Ҹ��ڿ�, ��Ÿ�, ����, �����ð�
     *  ȿ�� Ÿ��(����, ȸ��, ����, ����� ��)
     *  �ִϸ��̼�/����Ʈ ������ ����
    */
    [Header("�⺻ ����")]
    public string SkillName;
    public Sprite Icon;
    public float Cooldown;
    public float Cost;
    public float Range;
    public float CastTime;
    public GameObject EffectPrefab;
    public SkillType type;
    public float Power;

    [Header("���� ����")]
    public int RequireLevel;
    public int RequireSP;
    public List<SkillData> PrerequisiteSkills;

    [Header("��ų ����")]
    public int MaxLevel = 1; //�ִ� ����
}

public enum SkillType
{
    Damage,
    Heal,
    Buff,
    Debuff
}
