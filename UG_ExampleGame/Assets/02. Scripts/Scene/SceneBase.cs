using UK.UI;
using UnityEngine;

namespace UK.Scene
{
    public enum eSceneType
    {
        InitScene,
        ReadyScene,
        GameScene,
    }

    public abstract class SceneBase : MonoBehaviour
    {
        [SerializeField]
        private     PopupSystem     popupSystem;
        public      PopupSystem     PopupSystem     => popupSystem;

        [SerializeField]
        private     SceneTaskSystem taskSystem;
        public      SceneTaskSystem TaskSystem      => taskSystem;

        public abstract eSceneType  SceneType   { get; }
        private bool moveScene = false;

        private void Awake()
        {
            moveScene = SceneSystem.MoveFirstScene(SceneType);

            if (moveScene == false)
                SceneSystem.SetScene(this);
        }

        public void SceneInit()
        {
            Init();
            popupSystem?.Init();
            taskSystem?.Init();
        }


        public void SceneDispose()
        {
            if (moveScene) return;

            Dispose();
            popupSystem?.Dispose();
            taskSystem?.Dispose();
        }

        public abstract void Init();
        public abstract void Dispose();
    }
}
