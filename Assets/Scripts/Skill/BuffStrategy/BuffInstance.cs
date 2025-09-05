using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance 
{
    //버프 실행시 인스턴스 생성하여 인스턴스에 대입
    public IBuffBehavoprStrategy IBuff;
    public SkillData SkillData;
    public float Power;
    public float Duration;
    public float StartTime;

    public BuffInstance(IBuffBehavoprStrategy _IBuff, float _Power, float _Duration, SkillData _SkillData = null)
    {
        //인스턴스가 생성되면 IBuff의 값을 집어넣고 StartTime에 현재 시간을 넣음
        IBuff = _IBuff;
        Power = _Power;
        Duration = _Duration;
        SkillData = _SkillData;
        StartTime = Time.time;
    }

    //bool 현재시간 >= 버프실행시간 + 지속시간 이면 True
    public bool IsExpired => Time.time >= StartTime + Duration;

    public void RefreshDuration(float _NewDuration)
    {
        //Time.time: 시작된 순간부터, Time.deltaTime: 프레임이 시작하고 끝나는 시간
        float Remaining = StartTime + Duration - Time.time;
        Duration = Mathf.Max(Remaining, _NewDuration);
        StartTime = Time.time;
    }
}
