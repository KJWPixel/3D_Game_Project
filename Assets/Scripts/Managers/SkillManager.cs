using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;    

    PlayerStat PlayerStat;
    PlayerSkillBook PlayerSkillBook;
    PlayerController PlayerController;
    PlayerAnimationController Anim;

    ISkillBehaviorStrategy ISkillBehavior;

    //��ų�� ��Ÿ�� �ð� 
    public Dictionary<SkillData, float> SkillCoolDownTimers = new Dictionary<SkillData, float>();


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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerStat = GetComponent<PlayerStat>();      
        Anim = GetComponent<PlayerAnimationController>();
        PlayerSkillBook = GetComponent<PlayerSkillBook>();
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

        //��ų üũ
        if (!PlayerSkillBook.HasSkill(_Skill))
        {
            Debug.Log("����� ���� ��ų�Դϴ�.");
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

        //��ų ��Ÿ��
        SkillCoolDownTimers[_Skill] = Time.time + _Skill.Cooldown;

        //�̵� ��� 
        PlayerController.SetState(PlayerState.Casting);

        //�ִϸ��̼� ��� 
        Anim.PlayerSkillAnimation(_Skill.Effects, true);

        
        //ĳ���� �ð�
        yield return new WaitForSeconds(_Skill.CastTime);

        //ȿ�� ó��
        if(_Skill.Effects != null)
        {
            foreach(var Effect in _Skill.Effects)
            {
                switch (Effect.EffectType)
                {
                    case SkillEffectType.RayDamage:
                        ISkillBehavior = new RayDamageSkillStrategy();
                        ISkillBehavior.Execute(PlayerController, _Skill, _Target);
                        break;
                    case SkillEffectType.LineAreaDamage:
                        ISkillBehavior = new LineAreaDamageStrategy();
                        ISkillBehavior.Execute(PlayerController, _Skill, _Target);
                        break;
                    case SkillEffectType.TargetAreaDamage:
                        ISkillBehavior = new TargetAreaDamageStrategy();
                        ISkillBehavior.Execute(PlayerController, _Skill, _Target);
                        break;
                    case SkillEffectType.Heal:
                        PlayerStat.Heal(Effect.Power);
                        break;
                    case SkillEffectType.AtkBuff:
                        //PlayerStat.ApplyBuff(Effect); Player �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.DefBuff:
                        //PlayerStat.ApplyBuff(Effect); Player �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.CriBuff:
                        //PlayerStat.ApplyBuff(Effect); Player �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.TotalBuff:
                        //PlayerStat.ApplyBuff(Effect); Player �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.Debuff:
                        //if (_Target != null) _Target.GetComponent<Enemy>()?.ApplyDebuff(Effect); Enemy �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.CC:
                        //if (_Target != null) _Target.GetComponent<Enemy>()?.ApplyCC(Effect); Enemy �Լ� ���� �ʿ�
                        break;
                    case SkillEffectType.Resource:
                        //PlayerStat.RestoreResource(effect.Power); �÷��̾� �Լ����� �ʿ� 
                        break;
                    case SkillEffectType.Movement:
                        //�̵����� ȿ��
                        break;
                    case SkillEffectType.Teleport:
                        //�ڷ���Ʈ
                        this.transform.position += Vector3.forward * Effect.Distance;
                        break;
                }
            }
        }      
       #region
        //if (_Target != null)
        //{
        //    Enemy Enemy = _Target.GetComponent<Enemy>();
        //    if (Enemy != null) Enemy.TakeDamage(Effect.Power);
        //}
        //else if (Effect.Radius > 0)
        //{
        //    Collider[] hits = Physics.OverlapSphere(PlayerStat.transform.position, Effect.Radius);
        //    foreach (var hit in hits)
        //    {
        //        Enemy Enemy = hit.GetComponent<Enemy>();
        //        if (Enemy != null) Enemy.TakeDamage(Effect.Power);
        //    }
        //}
        #endregion

        ////��ų����Ʈ Prefab ���� 
        //if (_Skill.EffectPrefab != null)
        //{
        //    Vector3 pos = _Target != null ? _Target.position : PlayerStat.transform.position;
        //    GameObject fx = Instantiate(_Skill.EffectPrefab, pos, Quaternion.identity);
        //    Destroy(fx, 2f);
        //}

        //�÷��̾� State����
        PlayerController.SetState(PlayerState.Idle);
        //�ִϸ��̼� ����
        Anim.PlayerSkillAnimation(_Skill.Effects, false);
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
