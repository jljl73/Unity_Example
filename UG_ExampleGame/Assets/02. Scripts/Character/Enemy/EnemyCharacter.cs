using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UK.Data;
using UK.State;
using UK.Util;
using UnityEngine;

namespace UK.Character
{
    public abstract class EnemyCharacter : CharacterBase
    {
        protected float coolTime;

        public override void Init(CharacterData characterData)
        {
            base.Init(characterData);
            coolTime = Time.time + CharacterData.CoolTime;
        }

        protected override void LoadAnimation()
        {
            customAnimator.ClearAnimation();
            //customAnimator.LoadAnimation("Idle",)
        }

        protected override void LoadState()
        {
            NextStates.Clear();
            NextStates.Add(eStateType.Spawn, new List<ConditionBase>()
            {
                new ConditionBase(eStateType.Chase,     FindTarget)
            });

            NextStates.Add(eStateType.Chase, new List<ConditionBase>()
            {
                new ConditionBase(eStateType.Skill,     CheckSkillCoolTime),
                new ConditionBase(eStateType.Attack,    CheckRange),
                new ConditionBase(eStateType.Chase,     FindTarget)
            });

            NextStates.Add(eStateType.Attack, new List<ConditionBase>()
            {
                new ConditionBase(eStateType.Skill,     CheckSkillCoolTime),
                new ConditionBase(eStateType.Chase,     IsTargetNull),
                new ConditionBase(eStateType.Attack,    IsTargetExist)
            });

            NextStates.Add(eStateType.Skill, new List<ConditionBase>()
            {
                new ConditionBase(eStateType.Chase,     () => true),
            });

            NextStates.Add(eStateType.Wait, new List<ConditionBase>()
            {
                new ConditionBase(eStateType.Skill,     CheckSkill),
                new ConditionBase(eStateType.Wait,      () => GameManager.Instance.CastingSkill),
                new ConditionBase(eStateType.Chase,     () => true),
            });
        }

        public override async UniTask Chase()
        {
            var dir = (Target.transform.position - transform.position).normalized;
            transform.position += dir * Status.MoveSpeed * Time.deltaTime;
            await UniTask.DelayFrame(1, cancellationToken: token);
        }

        public override async UniTask Spawn()
        {
            transform.localPosition = new Vector3(4.0f, 0);
            transform.DOLocalMove(Vector3.zero, 1.0f).SetEase(Ease.Linear);
            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);
        }

        public override async UniTask Wait()
        {
            await UniTask.WaitForSeconds(0.5f, cancellationToken: token);
        }

        public override async UniTask Die()
        {
            await characterSprite.DORotate(new Vector3(0, 0, 720), 1.0f, RotateMode.FastBeyond360).AsyncWaitForCompletion();
            GameManager.Instance.EnemyController.Despawn(this);
        }

        protected override bool FindTarget()
        {
            if (IsTargetNull())
                Target = GameManager.Instance.PlayerController.GetNearPlayer(transform.position);
            return Target != null;
        }

        private bool CheckSkillCoolTime()
        {
            return Time.time > coolTime;
        }
    }
}
