using System.Collections;
using System.Collections.Generic;
using TMPro;
using UK.Data;
using UK.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UK.UI
{
    public class BattleWinPopup : PopupBase
    {
        [SerializeField]
        private TMP_Text stageText;

        [SerializeField]
        private Button exitButton;
        public override ePopupType PopupType => ePopupType.BattleWin;

        public override void Init()
        {
            base.Init();
            exitButton.OnClickAsObservable().Subscribe(_ => OnClickedExit());
        }

        public override void OnShow(PopupData popupData)
        {
            base.OnShow(popupData);
            stageText.text = $"Stage {DataCenter.Instance.UserData.Stage.Value + 1}\n Clear";
        }

        private void OnClickedExit()
        {
            SceneSystem.CurrentScene.PopupSystem.HidePopup();
            SceneSystem.ChangeScene(eSceneType.ReadyScene);
            DataCenter.Instance.UserData.ClearStage();
        }
    }
}
