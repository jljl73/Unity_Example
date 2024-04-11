using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class PlayerBerserker : PlayerCharacter
    {
        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            customAnimator.LoadAnimation("Idle", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Berserker_Idle_1")
            });

            customAnimator.LoadAnimation("Skill", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Berserker_Skill_1")
            });
        }

        public override async UniTask Attack()
        {
            customAnimator.Play("Idle");
            var dir = (Target.transform.position - transform.position).normalized;
            characterSprite.DOPunchPosition(dir, 0.16f, 1);
            Target.Hit(Status.Damage);
            
            await UniTask.WaitForSeconds(0.16f, cancellationToken: token);
            characterSprite.DOPunchPosition(dir, 0.16f, 1);
            Target.Hit(Status.Damage);
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            customAnimator.Play("Skill");
            characterSprite.DOPunchPosition(new Vector3(0, 1.0f, 0), 0.3f, 1);
            Hit(Status.HP * 0.4f);
            Status.AddBuff(eBuffType.AttackSpeedUp, 1.0f, 8.0f);

            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);
            GameManager.Instance.ReleaseSkillCast(CharacterId);
            await UniTask.WaitForSeconds(Status.AttackSpeed - 0.2f, cancellationToken: token);
        }

    }
}
