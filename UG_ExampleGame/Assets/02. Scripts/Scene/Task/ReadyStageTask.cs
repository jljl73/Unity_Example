using System.Collections;
using System.Collections.Generic;
using UK.UI;
using UnityEngine;

namespace UK.Scene
{
    public class ReadyStageTask : SceneTaskBase
    {
        public override eSceneTaskType SceneTaskType => eSceneTaskType.ReadyStage;

        public override void Init()
        {
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
