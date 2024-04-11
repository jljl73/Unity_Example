using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class PlayerWizard : PlayerCharacter
    {
        [SerializeField]
        private GameObject projectilPrefab;
        [SerializeField]
        private GameObject blizzardPrefab;

        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            customAnimator.LoadAnimation("Idle", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Wizard_Idle_1")
            });

            customAnimator.LoadAnimation("Attack", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Wizard_Attack_1")
            });

            customAnimator.LoadAnimation("Skill", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Wizard_Skill_1")
            });
        }
        public override async UniTask Attack()
        {
            customAnimator.Play("Attack");
            var projectile = projectilPrefab.SpawnObject(transform);

            var dist = Vector3.Distance(transform.position, Target.transform.position);
            var time = dist * 0.05f;

            projectile.transform.DOMove(Target.transform.position, time).SetEase(Ease.Linear);
            projectile.DespawnObject(time).Forget();

            Target.Hit(Status.Damage);
            customAnimator.Play("Idle", 0.25f).Forget();
            await UniTask.WaitForSeconds(Status.AttackSpeed - 0.25f, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            customAnimator.Play("Skill");
            var blizzard    = blizzardPrefab.SpawnObject(transform);
            blizzard.transform.position = Target.transform.position;
            blizzard.DespawnObject(0.4f).Forget();

            var enemies     = GameManager.Instance.EnemyController.EnemyCharacters;
            var range       = Status.AttackRange * 0.5f;
            
            for (int i = 0; i < enemies.Count; ++i)
            {
                if (UtilFunc.CheckRange(blizzard.transform.position, enemies[i].transform.position, range))
                {
                    enemies[i].Hit(Status.Damage * 10.0f);
                }
            }

            GameManager.Instance.ReleaseSkillCast(CharacterId);
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }
    }
}
