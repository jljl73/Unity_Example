using System.Collections;
using System.Collections.Generic;
using UK.Scene;
using UnityEngine;

namespace UK.Scene
{
    public class GameInitTask : SceneTaskBase
    {
        [SerializeField]
        private GameManager gameManager;

        public override eSceneTaskType SceneTaskType => eSceneTaskType.GameInit;

        public override void Init()
        {
            base.Init();
            gameManager.BattleReady();
        }

        public override void ActiveTask()
        {
            base.ActiveTask();
            SceneSystem.CurrentScene.TaskSystem.UpdateActiveTask(eSceneTaskType.GameBattle);
        }
    }
}
