using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;    

    PlayerStat PlayerStat;
    PlayerController PlayerController;
    PlayerAnimationController Anim;



    //��ų�� ��Ÿ�� �ð� 
    private Dictionary<SkillData, float> SkillCoolDownTimers = new Dictionary<SkillData, float>();


    /* ��ų ���� SkillManger
     * ��ų�� ����� �� ����Ǵ� ����
     * ��Ÿ�� üũ
     * ���� �Ҹ� üũ
     * Ÿ�� ����
     * �ִϸ��̼� ����
     * ����Ʈ ����
     * ������/ȿ�� ����
    */

    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();      
        Anim = GetComponent<PlayerAnimationController>();
        PlayerController = GetComponent<PlayerController>();
    }

    public void UseSkill(SkillData _Skill, Transform _Target = null)
    {
        if (!CanUse(_Skill)) return;
        StartCoroutine(CastSkill(_Skill, _Target));
    }


    private bool CanUse(SkillData _Skill)
    {
        if (PlayerStat == null)
        {
            return false;
        }

        //MP üũ
        if(PlayerStat.CurrentMp < _Skill.Cost)
        {
            return false;
        }

        //��Ÿ�� üũ
        if(SkillCoolDownTimers.ContainsKey(_Skill))
        {
            if(Time.time < SkillCoolDownTimers[_Skill])
            {
                return false;
            }
        }
        
        return true;
    }

    private IEnumerator CastSkill(SkillData _Skill, Transform _Target)
    {
        if(!PlayerStat.ConsumeMp(_Skill.Cost))
        {
            //MP���� �� false �ڷ�ƾ ����
            yield break;
        }

        //�̵� ��� 
        PlayerController.SetState(PlayerState.Casting);

        //�ִϸ��̼� ��� 
        Anim.PlayerSkillAnimation(_Skill.type, true);
        
        //ĳ���� �ð�
        yield return new WaitForSeconds(_Skill.CastTime);

        //ȿ���� ���� ó�� �κ�(������, ȸ��, ���� ����)
        if(_Skill.type == SkillType.Damage)//��ųŸ���� Damage�� 
        {
            //Skill.Type Damage ó�� 
        }

        if(_Skill.type == SkillType.Heal)//��ųŸ���� Heal�̸� 
        {
            PlayerStat.Heal(_Skill.Power);
        }

        if(_Skill.type == SkillType.Buff)
        {
            //SKill.Type Buff ó��
        }

        //��ų����Ʈ Prefab ���� 
        if(_Skill.EffectPrefab != null)
        {
            if(_Skill.type == SkillType.Heal)
            {
                GameObject EffectInstance = Instantiate(_Skill.EffectPrefab, PlayerStat.transform.position, Quaternion.identity);
                Destroy(EffectInstance, 1f);
            }
            else if(_Target != null)
            {
                GameObject EffectInstance = Instantiate(_Skill.EffectPrefab, _Target.transform.position, Quaternion.identity);
            }
        }

        //
        PlayerController.SetState(PlayerState.Idle);
        //�ִϸ��̼� ����
        Anim.PlayerSkillAnimation(_Skill.type, false);

        SkillCoolDownTimers[_Skill] = Time.time + _Skill.Cooldown;
    }


    /* 
     * ��ų �ߵ� �帧
     * 1. �÷��̾� �Է�: Ű����, ���콺 > SkillManager ȣ��
     * 2. ��� ���� ���� Ȯ��: ��Ÿ��, MP, Ÿ�� ��ȿ
     * 3. ����(cast)����: �ִϸ��̼� ����, ����Ʈ �غ�
     * 4. ȿ������: Ÿ�ٿ��� ������, ȸ��, �����̻� �ο�
     * 5. ��Ÿ�� ����
     * 
     * 
     * ��ų Ÿ�Ժ� ó��
     * ���� Ÿ�� ��ų > �� ��� ������ ����
     * ������ų > ���� �� ����� ����
     * ä�θ���ų > ���� �ð� ���� ���� �ߵ�
     * ����/���ۺ� > �ɷ�ġ ����, ���� �ð� �� ����
     * Ÿ�Ժ� ������ SkillType enum�� switch�� �Ǵ� ���/�������̽��� Ȱ���� �и��մϴ�.
     * 
     * ��ų �����͸� ScriptableObject�� ���� > �������� �뷱�� ���� ����
     * ��Ÿ��, �ڿ��Ҹ�� Ŭ���̾�Ʈ + �������� ����
     * �ִϸ��̼� �̺�Ʈ�� Ÿ�̹� ���� ������ ����
     * ���� ��ų�� Physics.OverlapSphere���� �Լ��� ���
     * ȿ��(������,�� ����)�� StatSystem�̳� SatusEffectManager�� ���� ����
     * 
     * 
     * ����
     * �÷��̾ "FireBall" ����
     * SkillManager.UseSkill(FireBall, Target)
     * CanUse üũ (��Ÿ��,MP)
     * �ִϸ��̼� ��� & ĳ���� �ð� ���
     * FireBall Prefab����
     * �浹 �� ������ ��� & Ÿ�� ü�� ���� 
     * ��Ÿ�� ����
    */
}
