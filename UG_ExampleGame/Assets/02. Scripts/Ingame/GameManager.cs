using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UK.Scene;
using UK.UI;
using UnityEngine;

namespace UK
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField]
        private PlayerController  playerController;
        public iPlayerController  PlayerController  => playerController;

        [SerializeField]
        private EnemyController     enemyController;
        public iEnemyController     EnemyController     => enemyController;

        public EffectController EffectController;

        public bool CastingSkill => (skillCastOrder.Count > 0);
        public int CastingCharacterId
        {
            get
            {
                if (skillCastOrder.Count == 0)
                    return 0;
                else
                    return skillCastOrder[0];
            }
        }
        private List<int> skillCastOrder = new List<int>();

        protected override void Init()
        {
        }

        public void Dispose()
        {
            playerController.Dispose();
            enemyController.Dispose();
        }

        public void BattleReady()
        {
            playerController.Init();
            enemyController.Init();
        }

        public void GameStart()
        {
            for (int i = 0; i < playerController.PlayerCharacters.Count; i++)
                playerController.PlayerCharacters[i].GameStart();

            for (int i = 0; i < enemyController.EnemyCharacters.Count; i++)
                enemyController.EnemyCharacters[i].GameStart();
        }

        public void GameOver(bool isWin)
        {
            if (SceneSystem.CurrentScene.TaskSystem.CurrentTask.SceneTaskType != eSceneTaskType.GameBattle)
                return;

            var popupData = new PopupData(isWin ? ePopupType.BattleWin : ePopupType.BattleLose, null);
            SceneSystem.CurrentScene.PopupSystem.ShowPopup(popupData);
            SceneSystem.CurrentScene.TaskSystem.UpdateActiveTask(eSceneTaskType.GameResult);
        }

        public void RegistSkillCast(int characterId)
        {
            skillCastOrder.Add(characterId);
        }

        public void ReleaseSkillCast(int characterId)
        {
            skillCastOrder.Remove(characterId);
        }
    }
}
