using Cysharp.Threading.Tasks;
using UK.Character;

namespace UK.State
{
    public class ChaseState : StateBase
    {
        public override eStateType StateType => eStateType.Chase;
        
        public ChaseState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Chase();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
