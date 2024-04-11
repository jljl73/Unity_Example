using Cysharp.Threading.Tasks;
using System;
using UK.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UK.UI
{
    public class UISkillButton : MonoBehaviour
    {
        [SerializeField]
        private Image skillImage;
        [SerializeField]
        private Image gaugeImage;
        [SerializeField]
        private Button useSkillButton;
        
        private bool timerActive = true;
        private bool isAutoSkill = false;
        private int characterId;
        private IDisposable timerDisposable = null;

        public void Init(int skillIndex)
        {
            var character = GameManager.Instance.PlayerController.GetPlayer(skillIndex);

            if (character == null)
            {
                useSkillButton.interactable = false;
                return;
            }

            characterId = character.CharacterId;
            GetComponent<Button>().OnClickAsObservable().Subscribe(_ => OnClickedSkill());
            var tableData = TableData.Instance.GetCharacterData(character.CharacterId);
            SetImage(character.CharacterId);

            SetTimer(tableData.CoolTime);
        }

        private void SetImage(int characterId)
        {
            switch(characterId)
            {
                case 1:
                    skillImage.sprite = Resources.Load<Sprite>("Sprite/Warrior_Win_1");
                    break;
                case 2:
                    skillImage.sprite = Resources.Load<Sprite>("Sprite/Wizard_Idle_1");
                    break;
                case 3:
                    skillImage.sprite = Resources.Load<Sprite>("Sprite/Rogue_Idle_1");
                    break;
                case 4:
                    skillImage.sprite = Resources.Load<Sprite>("Sprite/Berserker_Idle_1");
                    break;
                case 5:
                    skillImage.sprite = Resources.Load<Sprite>("Sprite/Priest_Idle_1");
                    break;
            }
            skillImage.SetNativeSize();
        }

        public void SetTimer(float totalTime)
        {
            useSkillButton.interactable = false;
            timerActive = true;

            float coolTime = Time.time + totalTime;

            if(timerDisposable != null)
                timerDisposable.Dispose();

            timerDisposable = gaugeImage.UpdateAsObservable()
                .TakeWhile(_ => timerActive)
                .Select(_ => Mathf.Clamp01((coolTime - Time.time) / totalTime))
                .Subscribe(fillAmount =>
                {
                    gaugeImage.fillAmount = fillAmount;

                    if (fillAmount <= 0)
                    {
                        timerActive = false;
                        useSkillButton.interactable = true;
                        if (isAutoSkill)
                            OnClickedSkill();
                    }
                })
                .AddTo(this);
        }

        public void SetAutoSkill(bool value)
        {
            isAutoSkill = value;
            if (isAutoSkill)
                OnClickedSkill();
        }

        private void OnClickedSkill()
        {
            if (useSkillButton.interactable == false)
                return;

            var players = GameManager.Instance.PlayerController.PlayerCharacters;
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].CharacterId == characterId)
                {
                    var tableData = TableData.Instance.GetCharacterData(characterId);
                    GameManager.Instance.PlayerController.UseSkill(characterId);
                    SetTimer(tableData.CoolTime);
                    return;
                }
            }

            useSkillButton.interactable = false;
        }
    }
}
