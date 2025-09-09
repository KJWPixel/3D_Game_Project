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
        IBuff = _IBuff;
        Power = _Power;
        Duration = _Duration;
        SkillData = _SkillData;
        StartTime = Time.time; //생성 시점에 Time.time을 찍어 종료시간 기준
    }

    //bool 시작시간 >= 버프실행시간 + 지속시간 이면 True
    public bool IsExpired => Time.time >= StartTime + Duration;

    public void RefreshDuration(float _NewDuration)
    {
        //Time.time: 시작된 순간부터, Time.deltaTime: 프레임이 시작하고 끝나는 시간
        float Remaining = StartTime + Duration - Time.time;
        Duration = Mathf.Max(Remaining, _NewDuration);
        StartTime = Time.time;
    }
}
