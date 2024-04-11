using System.Collections;
using System.Collections.Generic;
using UK.Data;
using UnityEngine;

namespace UK.Scene
{
    public class InitScene : SceneBase
    {
        public override eSceneType SceneType => eSceneType.InitScene;
        public override void Init()
        {
            DataCenter.Instance.Dispose();
            DataCenter.Instance.Init();

            SceneSystem.ChangeScene(eSceneType.ReadyScene);
        }

        public override void Dispose()
        {
        }

    }
}
