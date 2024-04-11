using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UK.Character
{
    public class EnemySquare : EnemyCharacter
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
            characterSprite.DOKill();
            characterSprite.DORotate(new Vector3(0, 0, 360), 1.0f, RotateMode.FastBeyond360);
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);

            var players = GameManager.Instance.PlayerController.PlayerCharacters;

            for (int i = 0; i < players.Count; ++i)
                players[i].Target = this;

            GameManager.Instance.ReleaseSkillCast(CharacterId);
            coolTime = Time.time + CharacterData.CoolTime;
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);
        }
    }
}
