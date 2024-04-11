using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UK.Scene
{
    public class GameBattleTask : SceneTaskBase
    {
        public override eSceneTaskType SceneTaskType => eSceneTaskType.GameBattle;

        [SerializeField]
        private GameManager gameManager;

        public override void ActiveTask()
        {
            base.ActiveTask();
            gameManager.GameStart();
        }

        public override void DeactiveTask()
        {
            base.DeactiveTask();
            gameManager.Dispose();
        }

    }
}
