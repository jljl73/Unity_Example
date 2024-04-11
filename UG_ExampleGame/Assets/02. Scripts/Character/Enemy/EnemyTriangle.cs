using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UK.Character
{
    public class EnemyTriangle : EnemyCharacter
    {
        public override async UniTask Attack()
        {
            var dir = (Target.transform.position - transform.position).normalized;
            characterSprite.DOPunchPosition(dir, 0.16f, 1);
            Target.Hit(Status.Damage);
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            var dir = (Target.transform.position - transform.position).normalized;
            characterSprite.DOPunchPosition(dir, 0.16f, 1);
            Target.Hit(Status.Damage * 2.0f);
            coolTime = Time.time + CharacterData.CoolTime;
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);
        }
    }
}
