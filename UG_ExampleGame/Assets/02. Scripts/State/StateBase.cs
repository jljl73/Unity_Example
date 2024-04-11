using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using Unity.VisualScripting;
using UnityEngine;

namespace UK.State
{
    public enum eStateType
    {
        None,

        Spawn,
        Chase,
        Attack,
        Skill,
        Die,
        Wait,
    }

    public abstract class StateBase
    {
        public abstract eStateType StateType { get; }

        protected CharacterBase character;

        public StateBase(CharacterBase character)
        {
            this.character = character;
        }

        public abstract UniTask OnStateEnter();
        public abstract UniTask OnStateExit();
    }
}
