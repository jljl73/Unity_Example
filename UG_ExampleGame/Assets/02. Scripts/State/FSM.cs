using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UnityEngine;

namespace UK.State
{
    public class FSM
    {
        public eStateType StateType { get; protected set; }
        private StateBase CurState = null;
        private Dictionary<eStateType, StateBase> states = new Dictionary<eStateType, StateBase>();
        private CharacterBase character;

        public FSM(CharacterBase character)
        {
            this.character = character;
            StateType = eStateType.None;

            states.Add(eStateType.Spawn,    new SpawnState(character));
            states.Add(eStateType.Chase,    new ChaseState(character));
            states.Add(eStateType.Attack,   new AttackState(character));
            states.Add(eStateType.Skill,    new SkillState(character));
            states.Add(eStateType.Die,      new DieState(character));
            states.Add(eStateType.Wait,     new WaitState(character));
        }

        public void ChangeState(eStateType inNextState)
        {
            if (CurState != null)
                CurState.OnStateExit();
            else if (StateType == eStateType.Die)
                return;

            StateType   = inNextState;
            CurState    = states[inNextState];
            CurState.OnStateEnter();
        }
    }
}
