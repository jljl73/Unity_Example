using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Collections.Generic;
using UK.Data;
using System;


namespace UK.UI
{
    public class UIGameView : UIView
    {
        [SerializeField]
        private Transform   skillParent;
        [SerializeField]
        private TMP_Text    timerText;

        [SerializeField]
        private Image       playerHP;
        [SerializeField]
        private Image       enemyHP;

        [SerializeField]
        private Button      autoSkillButton;
        [SerializeField]
        private Image       autoSkillOutLine;
        [SerializeField]
        private Button      speedUpButton;
        [SerializeField]
        private Image       speedUpOutLine;

        private float       playerMaxHPSum;
        private float       enemyMaxHPSum;

        private List<UISkillButton> skillButtons = new List<UISkillButton>();
        private IDisposable timerDisposable;
        private bool isAutoSkill;

        public override void Init()
        {
            base.Show();
            isAutoSkill = false;

            // 스킬 버튼
            int deckCount = DataCenter.Instance.DeckData.Decks.Count;
            for (int i = 0; i < skillParent.childCount; i++)
            {
                int tIndex = i;
                var newButton = skillParent.GetChild(i).GetComponent<UISkillButton>();
                if(i < deckCount)
                    newButton.gameObject.SetActive(true);
                else
                    newButton.gameObject.SetActive(false);

                newButton.Init(tIndex);
                skillButtons.Add(newButton.GetComponent<UISkillButton>());
            }

            // 아군 체력
            var players = GameManager.Instance.PlayerController.PlayerCharacters;
            playerMaxHPSum = 0;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Status.ReactiveHP.Subscribe(_ => UpdatePlayerHP());
                playerMaxHPSum += players[i].Status.MaxHP;
            }

            // 적 체력 
            var enemies = GameManager.Instance.EnemyController.EnemyCharacters;
            enemyMaxHPSum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Status.ReactiveHP.Subscribe(_ => UpdateEnemyHP());
                enemyMaxHPSum += enemies[i].Status.MaxHP;
            }

            // 타이머
            float stdTime = Time.time + 60.0f;
            timerDisposable = Observable.Timer(TimeSpan.FromSeconds(1.0f)).Repeat()
                .Subscribe(_ =>
                {
                    timerText.text = (stdTime - Time.time).ToString("#");
                    if(Time.time >= stdTime)
                    {
                        GameManager.Instance.GameOver(false);
                        timerDisposable.Dispose();
                    }
                }
            );

            // 인게임 기능
            autoSkillButton.OnClickAsObservable().Subscribe(_ => OnClickedAutoSkill());
            speedUpButton.OnClickAsObservable().Subscribe(_ => OnClickedSpeedUp());
        }

        public override void Dispose()
        {
            Time.timeScale = 1.0f;
            timerDisposable.Dispose();
        }

        public override void UpdateView()
        {
        }

        private void UpdatePlayerHP()
        {
            var players     = GameManager.Instance.PlayerController.PlayerCharacters;
            float hpsum     = 0;
            
            for (int i = 0; i < players.Count; i++)
                hpsum       += players[i].Status.HP;

            playerHP.fillAmount = hpsum / playerMaxHPSum;
        }

        private void UpdateEnemyHP()
        {
            var enemies = GameManager.Instance.EnemyController.EnemyCharacters;
            float hpsum = 0;

            for (int i = 0; i < enemies.Count; i++)
                hpsum += enemies[i].Status.HP;

            enemyHP.fillAmount = hpsum / enemyMaxHPSum;
        }

        #region Click Action
        private void OnClickedAutoSkill()
        {
            isAutoSkill                 = !isAutoSkill;
            autoSkillOutLine.enabled    = isAutoSkill;

            for (int i = 0; i < skillButtons.Count; ++i)
                skillButtons[i].SetAutoSkill(isAutoSkill);
        }

        private void OnClickedSpeedUp()
        {
            if (Time.timeScale > 1.0f)
            {
                Time.timeScale          = 1.0f;
                speedUpOutLine.enabled  = false;
            }
            else
            {
                Time.timeScale          = 1.5f;
                speedUpOutLine.enabled  = true;
            }
        }
        #endregion
    }
}
