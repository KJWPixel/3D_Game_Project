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
    IBuffBehavoprStrategy IBuff;

    //��ų�� ��Ÿ�� �ð� 
    public Dictionary<SkillData, float> SkillCoolDownTimers = new Dictionary<SkillData, float>();

    //ȿ�� Ÿ�� => ���� ���� ����
    private Dictionary<SkillEffectType, ISkillBehaviorStrategy> EffectHandlers;


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

        SetupEffectHandlers();
    }

    private void SetupEffectHandlers()
    {
        //�������� ��ų
        EffectHandlers = new Dictionary<SkillEffectType, ISkillBehaviorStrategy>
        {
            {SkillEffectType.RayDamage, new RayDamageSkillStrategy() },
            {SkillEffectType.LineAreaDamage, new LineAreaDamageStrategy() },
            {SkillEffectType.TargetAreaDamage, new TargetAreaDamageStrategy() },
        };



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

        //ȿ�� ����
        if(_Skill.Effects != null)
        {
            foreach(var Effect in _Skill.Effects)
            {
                if (EffectHandlers.TryGetValue(Effect.EffectType, out var Handler))
                {
                    Handler.Execute(PlayerController, PlayerStat, _Skill, _Target);
                }
                else
                {
                    Debug.Log($"SkillManager.cs: [{Effect.EffectType}] ���� �ڵ鷯�� ��ϵ��� ����");
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
