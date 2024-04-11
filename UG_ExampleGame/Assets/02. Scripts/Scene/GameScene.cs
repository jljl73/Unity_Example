using UnityEngine;

using UniRx;
using UniRx.Triggers;
using UK.Character;

namespace UK.Scene
{
    public class GameScene : SceneBase
    {
        public override eSceneType      SceneType       => eSceneType.GameScene;
        
        public override void Init()
        {
            TestInput();
        }

        public override void Dispose()
        {
        }

        private void TestInput()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ =>
                {
                    //DataCenter.Instance.UserData.Score.Value += 1;
                    //battleController.BattleReady();
                });
        }
    }
}
