using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class PlayerWarrior : PlayerCharacter
    {
        [SerializeField]
        private GameObject skillRangeObject;

        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            customAnimator.LoadAnimation("Idle", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Warrior_Idle_1")
            });

            customAnimator.LoadAnimation("Skill", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Warrior_Skill_1")
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
            customAnimator.Play("Skill");
            characterSprite.DOPunchPosition(new Vector3(0, 1.0f, 0), 0.3f, 1);
            Heal(Status.MaxHP * 0.4f);

            var enemies = GameManager.Instance.EnemyController.EnemyCharacters;
            for (int i = 0; i < enemies.Count; ++i)
            {
                if (UtilFunc.CheckRange(transform.position, enemies[i].transform.position, 2.0f))
                {
                    var dir = (enemies[i].transform.position - transform.position).normalized * 2.0f;
                    enemies[i].transform.DOMove(enemies[i].transform.position + dir, 0.1f);
                    enemies[i].Hit(Status.Damage * 0.5f);
                }
            }
            skillRangeObject.SetActive(true);
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);
            skillRangeObject.SetActive(false);
            GameManager.Instance.ReleaseSkillCast(CharacterId);
            await UniTask.WaitForSeconds(Status.AttackSpeed - 0.2f, cancellationToken: token);
        }
    }
}
