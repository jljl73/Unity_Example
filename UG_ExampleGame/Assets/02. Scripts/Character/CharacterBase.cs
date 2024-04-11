using UK.State;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UK.Data;
using UnityEngine.UI;
using UniRx;
using UK.Util;
using DG.Tweening;

namespace UK.Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField]
        protected Transform     characterSprite;
        [SerializeField]
        private Image           hpImage;
        [SerializeField]
        protected CustomAnimator customAnimator;

        public int              CharacterId     { get; private set; }
        private FSM             FSM             { get; set; }
        public Status           Status          { get; private set; }
        public CharacterBase    Target          { get; set; }
        public CharacterData    CharacterData   { get; private set; }

        protected Dictionary<eStateType, List<ConditionBase>> NextStates = new Dictionary<eStateType, List<ConditionBase>>();
        
        private CancellationTokenSource cancelToken;
        protected CancellationToken token => cancelToken.Token;
        
        public eStateType CurState
        {
            get
            {
                if (FSM == null)
                    return eStateType.Die;
                else
                    return FSM.StateType;
            }
        }

        public virtual void Init(CharacterData characterData)
        {
            CharacterId     = characterData.CharacerId;

            Target          = null;
            FSM             = new FSM(this);
            Status          = new Status();
            CharacterData   = characterData;
            Status.Init(CharacterData);
            Status.ReactiveHP.Subscribe(hp => { hpImage.fillAmount = hp / Status.MaxHP; }).AddTo(this);

            LoadState();
            LoadAnimation();
        }

        public void GameStart()
        {
            FSM.ChangeState(eStateType.Spawn);
        }

        public void Dispose()
        {
            CancelToken();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        protected abstract void LoadState();

        protected abstract void LoadAnimation();

        public void Hit(float value)
        {
            if (CurState == eStateType.Die)
                return;

            Status.Hit(value);
            GameManager.Instance.EffectController.CreateDamageText(transform.position + new Vector3(0, 0.5f, 0), value, this is EnemyCharacter);

            if (Status.IsDead)
            {
                FSM.ChangeState(eStateType.Die);
            }
        }

        public void Heal(float value)
        {
            Status.Heal(value);
            GameManager.Instance.EffectController.CreateHealText(transform.position + new Vector3(0, 0.5f, 0), value);
        }

        public void CheckNextState(eStateType stateType)
        {
            if (NextStates.ContainsKey(stateType) == false)
                return;

            var conditions = NextStates[stateType];
            for(int i = 0; i < conditions.Count; ++i)
            {
                if (conditions[i].CheckTrue())
                {
                    FSM.ChangeState(conditions[i].NextState);
                    return;
                }
            }
        }

        public bool IsTargetNull()
        {
            return Target == null || Target.CurState == eStateType.Die;
        }

        public bool IsTargetExist()
        {
            return Target != null && Target.CurState != eStateType.Die;
        }

        public void CreateToken()
        {
            if (cancelToken != null)
                cancelToken.Cancel();
            cancelToken = new CancellationTokenSource();
        }

        public void CancelToken()
        {
            if (cancelToken != null)
                cancelToken.Cancel();
            cancelToken = null;

            // TODO : 초기화하는 함수 따로 만들어서 StateExit때 실행
            characterSprite.DOKill();
            characterSprite.localPosition     = Vector3.zero;
            characterSprite.localScale        = Vector3.one;
            characterSprite.rotation          = Quaternion.identity;
        }

        public abstract UniTask Spawn();

        public abstract UniTask Chase();

        public abstract UniTask Attack();

        public abstract UniTask Skill();

        public abstract UniTask Die();

        public abstract UniTask Wait();


        #region Character Util

        protected bool CheckSkill()
        {
            return GameManager.Instance.CastingCharacterId == CharacterId;
        }

        protected bool CheckRange()
        {
            if (IsTargetNull())
                return false;
            return UtilFunc.CheckRange(transform.position, Target.transform.position, Status.AttackRange);
        }

        protected abstract bool FindTarget();
        #endregion
    }
}
