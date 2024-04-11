using Cysharp.Threading.Tasks;
using UK.Character;

namespace UK.State
{
    /// <summary>
    /// 연출용 State였는데 그냥 안씀
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
