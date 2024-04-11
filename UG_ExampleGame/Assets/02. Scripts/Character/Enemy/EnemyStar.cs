using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public class EnemyStar : EnemyCharacter
    {
        public override async UniTask Attack()
        {
            characterSprite.DORotate(new Vector3(0, 0, 720), Status.AttackSpeed, RotateMode.FastBeyond360);
            var players = GameManager.Instance.PlayerController.PlayerCharacters;
            for (int i = 0; i < players.Count; ++i)
            {
                if (UtilFunc.CheckRange(transform.position, players[i].transform.position, Status.AttackRange))
                    players[i].Hit(Status.Damage);
            }
            await UniTask.WaitForSeconds(Status.AttackSpeed, cancellationToken: token);
        }

        public override async UniTask Skill()
        {
            var players = GameManager.Instance.PlayerController.PlayerCharacters;

            for (int i = 0; i < players.Count; ++i)
            {
                var skillTarget = players[i];
                var lerp = Vector3.Lerp(skillTarget.transform.position, transform.position, 0.5f);
                skillTarget.transform.DOMove(lerp, 0.1f);
            }
            
            coolTime = Time.time + CharacterData.CoolTime;
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);
        }
    }
}
