using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace UK.Character
{
    public class PlayerPriest : PlayerCharacter
    {
        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            customAnimator.LoadAnimation("Idle", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Priest_Idle_1")
            });

            customAnimator.LoadAnimation("Skill", new List<Sprite>()
            {
                Resources.Load<Sprite>("Sprite/Priest_Skill_1")
            });
        }

        public override async UniTask Attack()
        {
            customAnimator.Play("Idle");

            var players = GameManager.Instance.PlayerController.PlayerCharacters;
            float minHp = float.MaxValue;
            
            PlayerCharacter target = null;
            for(int i = 0; i < players.Count; ++i)
            {
                if (players[i].Status.HP < minHp)
                {
                    target = players[i];
                    minHp = players[i].Status.HP;
                }
            }

            if(target != null)
            {
                target.Heal(Status.Damage);
            }
            
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            customAnimator.Play("Skill");
            var players = GameManager.Instance.PlayerController.PlayerCharacters;

            for (int i = 0; i < players.Count; ++i)
                players[i].Heal(Status.Damage);

            GameManager.Instance.ReleaseSkillCast(CharacterId);
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        protected override bool FindTarget()
        {
            if (IsTargetNull())
                Target = GameManager.Instance.PlayerController.GetNearPlayer(transform.position);
            return Target != null;
        }
    }
}
