using UK.UI;
using UnityEngine;

namespace UK.Scene
{
    public enum eSceneTaskType
    {
        Splash_Init     = 0,

        ReadyStage      = 101,
        ReadyDeck       = 102,

        GameInit        = 201,
        GameBattle      = 202,
        GameResult      = 203,
    }

    public abstract class SceneTaskBase : MonoBehaviour
    {
        [SerializeField]
        public UIView   CurrentView;

        public abstract eSceneTaskType SceneTaskType   { get; }

        public virtual void Init()
        {
            CurrentView?.Init();
        }

        public virtual void Dispose()
        {
            CurrentView?.Dispose();
        }

        public virtual void ActiveTask()
        {
            CurrentView?.Show();
        }

        public virtual void DeactiveTask()
        {
            CurrentView?.Hide();
        }
    }
}
