using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance 
{
    public IBuffBehavoprStrategy IBuff {  get; private set; }   
    public SkillData SkillData { get; private set; }
    public float Power { get; private set; }
    public float Duration { get; private set; }
    public float StartTime { get; private set; }

    public BuffInstance(IBuffBehavoprStrategy _IBuff, float _Power, float _Duration, SkillData _SkillData = null)
    {
        IBuff = _IBuff;
        Power = _Power;
        Duration = _Duration;
        SkillData = _SkillData;
        StartTime = Time.time;
    }

    public bool IsExpired => Time.time >= StartTime + Duration;

    public void RefreshDuration(float newDuration)
    {
        // ���� �����ð� + �� ���ӽð����� ũ�� �ִ� ���ӽð� ����
        float remaining = StartTime + Duration - Time.time;
        Duration = Mathf.Max(remaining, newDuration);
        StartTime = Time.time; // ���� ���� �ð�
    }
}
