using Cysharp.Threading.Tasks;
using DG.Tweening;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class EnemyCircle : EnemyCharacter
    {

        [SerializeField]
        private GameObject projectilPrefab;

        public override async UniTask Attack()
        {
            var projectile = projectilPrefab.SpawnObject(transform);

            var dist = Vector3.Distance(transform.position, Target.transform.position);
            var time = dist * 0.05f;

            projectile.transform.DOMove(Target.transform.position, time).SetEase(Ease.Linear);
            projectile.DespawnObject(time).Forget();

            Target.Hit(Status.Damage);
            await UniTask.WaitForSeconds(Status.AttackSpeed - 0.25f, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            characterSprite.DOKill();
            characterSprite.DOScale(3.0f, 5.0f);
            await UniTask.WaitForSeconds(5.0f, cancellationToken: token);

            var players = GameManager.Instance.PlayerController.PlayerCharacters;

            for (int i = 0; i < players.Count; ++i)
                players[i].Hit(Status.Damage * 10.0f);

            characterSprite.localScale = Vector3.one;

            GameManager.Instance.ReleaseSkillCast(CharacterId);
            coolTime = Time.time + CharacterData.CoolTime;
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);
        }
    }
}
