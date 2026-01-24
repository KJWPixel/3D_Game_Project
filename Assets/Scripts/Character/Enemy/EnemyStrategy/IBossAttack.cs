using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossAttack
{
    int AttackIndex { get; } // 애니메이션 AttackType(int)
    IEnumerator Execute(EnemyBoss boss, Transform player,  System.Action onComplete);

    public void OnEffectEvent(); // 애니메이션 이벤트 발생 시 호출될 함숮
}
