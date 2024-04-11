using Cysharp.Threading.Tasks;
using UK.Character;

namespace UK.State
{
    public class AttackState : StateBase
    {
        public override eStateType StateType => eStateType.Attack;
        
        public AttackState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Attack();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
