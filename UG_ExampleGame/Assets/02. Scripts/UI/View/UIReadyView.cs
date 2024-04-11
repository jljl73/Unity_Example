using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UK.Scene;
using TMPro;
using UK.Data;
using UnityEngine.TextCore.Text;
using System;


namespace UK.UI
{
    public class UIReadyView : UIView
    {
        [SerializeField]
        private TMP_Text textStage;

        [SerializeField]
        private Button buttonLeft;
        [SerializeField]
        private Button buttonRight;
        [SerializeField]
        private Button buttonStart;

        IDisposable stageDisposable;

        public override void Init()
        {
            stageDisposable = DataCenter.Instance.UserData.Stage.AsObservable().Subscribe(value =>
                {
                    int clearLevel = DataCenter.Instance.UserData.ClearStageLevel.Value;
                    if (clearLevel == value)
                    {
                        textStage.color = Color.yellow;
                        buttonStart.interactable = true;
                    }
                    else if (clearLevel < value)
                    {
                        textStage.color = Color.red;
                        buttonStart.interactable = false;
                    }
                    else
                    {
                        textStage.color = Color.green;
                        buttonStart.interactable = true;
                    }
                    textStage.text = $"Stage {value + 1}";
                });

            buttonStart.OnClickAsObservable().Subscribe(_ =>
                {
                    SceneSystem.CurrentScene.TaskSystem.UpdateActiveTask(eSceneTaskType.ReadyDeck);
                });
            
            buttonLeft.OnClickAsObservable().Subscribe(_ =>
                {
                    if (DataCenter.Instance.UserData.Stage.Value > 0)
                        --DataCenter.Instance.UserData.Stage.Value;
                });
            
            buttonRight.OnClickAsObservable().Subscribe(_ =>
                {
                    if (DataCenter.Instance.UserData.Stage.Value < 12)
                        ++DataCenter.Instance.UserData.Stage.Value;
                });
        }

        public override void Dispose()
        {
            stageDisposable.Dispose();
        }

        public override void UpdateView()
        {
        }
    }
}
