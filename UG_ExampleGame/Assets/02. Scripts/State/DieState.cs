using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UnityEngine;

namespace UK.State
{
    public class DieState : StateBase
    {
        public override eStateType StateType => eStateType.Die;
        
        public DieState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Die();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
