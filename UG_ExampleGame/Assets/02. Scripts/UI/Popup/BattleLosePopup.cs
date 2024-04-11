using UK.Data;
using UK.Scene;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UK.UI
{
    public class BattleLosePopup : PopupBase
    {
        [SerializeField]
        private Button exitButton;
        public override ePopupType PopupType => ePopupType.BattleLose;

        public override void Init()
        {
            base.Init();
            exitButton.OnClickAsObservable().Subscribe(_ => OnClickedExit());
        }

        private void OnClickedExit()
        {
            SceneSystem.CurrentScene.PopupSystem.HidePopup();
            SceneSystem.ChangeScene(eSceneType.ReadyScene);
        }
    }
}
