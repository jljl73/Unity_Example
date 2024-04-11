using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UnityEngine;

namespace UK.State
{
    public class SkillState : StateBase
    {
        public override eStateType StateType => eStateType.Skill;

        public SkillState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Skill();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
