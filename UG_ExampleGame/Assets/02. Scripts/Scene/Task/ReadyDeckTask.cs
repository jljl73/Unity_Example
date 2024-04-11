using System.Collections;
using System.Collections.Generic;
using UK.Scene;
using UnityEngine;

namespace UK
{
    public class ReadyDeckTask : SceneTaskBase
    {
        public override eSceneTaskType SceneTaskType => eSceneTaskType.ReadyDeck;

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
