using System.Collections.Generic;
using UK.Data;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UK.Scene;
using System;

namespace UK.UI
{
    public class UIBattleReadyView : UIView
    {
        [Header("Player")]
        [SerializeField]
        private Button[]        playerChooseButtons;

        [SerializeField]
        private Transform       playerSlotTrans;
        

        [SerializeField]
        private List<Image>     characterImages;
        [SerializeField]
        private List<Sprite>    characterSprites;

        [Header("View")]
        [SerializeField]
        private Button          startButton;
        [SerializeField]
        private Button          exitButton;


        private List<IDisposable> disposables = new List<IDisposable>();
        private int slotCount = 3;

        public override void Init()
        {
            for (int i = 0; i< playerChooseButtons.Length; ++i)
            {
                int characterId = i + 1;
                playerChooseButtons[i].OnClickAsObservable().Subscribe(_ =>
                {
                    OnClickedCharacter(characterId);
                });
            }

            startButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (DataCenter.Instance.DeckData.Decks.Count > 0)
                    SceneSystem.ChangeScene(eSceneType.GameScene);
            });

            exitButton.OnClickAsObservable().Subscribe(_ =>
            {
                SceneSystem.CurrentScene.TaskSystem.UpdateActiveTask(eSceneTaskType.ReadyStage);
            });

            disposables.Add(DataCenter.Instance.DeckData.Decks.ObserveAdd().Subscribe(x => UpdateView()));
            disposables.Add(DataCenter.Instance.DeckData.Decks.ObserveRemove().Subscribe(x => UpdateView()));

            UpdateView();
        }

        public override void Dispose()
        {
            for (int i = 0; i < disposables.Count; i++)
            {
                disposables[i].Dispose();
            }
        }

        public override void UpdateView()
        {
            int i = 0;
            for (; i < DataCenter.Instance.DeckData.Decks.Count; ++i)
            {
                characterImages[i].sprite = characterSprites[DataCenter.Instance.DeckData.Decks[i] - 1];
                characterImages[i].SetNativeSize();
            }
            for (; i < characterImages.Count; ++i)
                characterImages[i].sprite = null;
        }

        public override void Show()
        {
            var stageLevel = DataCenter.Instance.UserData.Stage.Value;
            if (stageLevel < 2)
                slotCount = 1;
            else if(stageLevel < 4)
                slotCount = 2;
            else if(stageLevel < 9)
                slotCount = 3;
            else
                slotCount = 4;

            for (int i = 0; i < playerSlotTrans.childCount; ++i)
            {
                playerSlotTrans.GetChild(i).gameObject.SetActive(i < slotCount);
            }
            base.Show();
        }

        private void OnClickedCharacter(int characterId)
        {
            if (DataCenter.Instance.DeckData.Decks.Contains(characterId))
                DataCenter.Instance.DeckData.Decks.Remove(characterId);
            else if (DataCenter.Instance.DeckData.Decks.Count < slotCount)
                DataCenter.Instance.DeckData.Decks.Add(characterId);
        }
    }
}
