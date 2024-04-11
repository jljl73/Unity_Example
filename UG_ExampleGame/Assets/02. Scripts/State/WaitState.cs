using Cysharp.Threading.Tasks;
using UK.Character;

namespace UK.State
{
    /// <summary>
    /// ����� State���µ� �׳� �Ⱦ�
    /// </summary>
    public class WaitState : StateBase
    {
        public override eStateType StateType => eStateType.Wait;
        
        public WaitState(CharacterBase character) : base(character)
        {
        }

        public override async UniTask OnStateEnter()
        {
            character.CreateToken();
            await character.Wait();
            character.CheckNextState(StateType);
        }

        public override async UniTask OnStateExit()
        {
            character.CancelToken();
        }
    }
}
