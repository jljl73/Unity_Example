using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UnityEngine;

namespace UK.State
{
    public class SpawnState : StateBase
    {
        public override eStateType StateType => eStateType.Spawn;

        public SpawnState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Spawn();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
