using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class PlayerRogue : PlayerCharacter
    {
        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            customAnimator.LoadAnimation("Idle", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Rogue_Idle_1")
            });
        }

        public override async UniTask Attack()
        {
            customAnimator.Play("Idle");
            var dir = (Target.transform.position - transform.position).normalized;
            characterSprite.DOPunchPosition(dir, 0.16f, 1);
            Target.Hit(Status.Damage);
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            customAnimator.Play("Idle");

            var enemies = GameManager.Instance.EnemyController.EnemyCharacters;
            var maxDist = 0.0f;
            EnemyCharacter skillTarget = null;

            for (int i = 0; i < enemies.Count; ++i)
            {
                var dist = Vector3.Distance(transform.position, enemies[i].transform.position);
                if (maxDist < dist)
                {
                    skillTarget = enemies[i];
                    maxDist = dist;
                }
            }

            if(skillTarget != null)
            {
                transform.position = skillTarget.transform.position + (skillTarget.transform.position - transform.position).normalized;
                skillTarget.Hit(Status.Damage * 10);
                Target = skillTarget;
            }

            await UniTask.WaitForSeconds(0.1f, cancellationToken: token);
            GameManager.Instance.ReleaseSkillCast(CharacterId);
            await UniTask.WaitForSeconds(Status.AttackSpeed - 0.1f, cancellationToken: token);
        }
    }
}
