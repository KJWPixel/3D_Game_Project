using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance 
{
    //���� ����� �ν��Ͻ� �����Ͽ� �ν��Ͻ��� ����
    public IBuffBehavoprStrategy IBuff;
    public SkillData SkillData;
    public float Power;
    public float Duration;
    public float StartTime;

    public BuffInstance(IBuffBehavoprStrategy _IBuff, float _Power, float _Duration, SkillData _SkillData = null)
    {
        //�ν��Ͻ��� �����Ǹ� IBuff�� ���� ����ְ� StartTime�� ���� �ð��� ����
        IBuff = _IBuff;
        Power = _Power;
        Duration = _Duration;
        SkillData = _SkillData;
        StartTime = Time.time;
    }

    //bool ����ð� >= ��������ð� + ���ӽð� �̸� True
    public bool IsExpired => Time.time >= StartTime + Duration;

    public void RefreshDuration(float _NewDuration)
    {
        //Time.time: ���۵� ��������, Time.deltaTime: �������� �����ϰ� ������ �ð�
        float Remaining = StartTime + Duration - Time.time;
        Duration = Mathf.Max(Remaining, _NewDuration);
        StartTime = Time.time;
    }
}
