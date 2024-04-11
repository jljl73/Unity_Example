using System.Collections;
using System.Collections.Generic;
using UK.Character;
using UK.Data;
using UK.Scene;
using UK.UI;
using UK.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace UK
{
    public interface iEnemyController
    {
        IReadOnlyList<EnemyCharacter> EnemyCharacters { get; }
        EnemyCharacter GetNearEnemy(Vector3 position);
        void Despawn(EnemyCharacter enemyCharacter);
    }


    public class EnemyController : MonoBehaviour, iEnemyController
    {
        [SerializeField]
        private Transform[] slotTrans;
        [SerializeField]
        private StageData stageData;

        private List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();
        public IReadOnlyList<EnemyCharacter> EnemyCharacters => enemyCharacters;

        public void Init()
        {
            int stageLevel = DataCenter.Instance.UserData.Stage.Value;
            Spawn(stageLevel);
        }

        public void Dispose()
        {
            for (int i = 0; i < enemyCharacters.Count; ++i)
            {
                enemyCharacters[i].Dispose();
                enemyCharacters[i].DespawnObject();
            }
            enemyCharacters.Clear();
        }

        public void Spawn(int slotIndex)
        {
            for (int i = 0; i < stageData.Stages[slotIndex].Enemies.Count; ++i)
            {
                var enemyData = stageData.Stages[slotIndex].Enemies[i];
                var newCharacter = enemyData.Prefab.SpawnObject(slotTrans[i]).GetComponent<EnemyCharacter>();

                newCharacter.Init(enemyData);
                enemyCharacters.Add(newCharacter);
            }
        }

        public EnemyCharacter GetNearEnemy(Vector3 position)
        {
            EnemyCharacter targetEnemy = null;
            float minDist = float.MaxValue;

            for(int i = 0; i < enemyCharacters.Count; ++i)
            {
                if (enemyCharacters[i].CurState == State.eStateType.Die) 
                    continue;

                var dist = (enemyCharacters[i].transform.position - position).sqrMagnitude;
                if(dist < minDist)
                {
                    targetEnemy = enemyCharacters[i];
                    minDist = dist;
                }
            }
            return targetEnemy;
        }

        public void Despawn(EnemyCharacter enemyCharacter)
        {
            enemyCharacters.Remove(enemyCharacter);
            enemyCharacter.Dispose();
            enemyCharacter.DespawnObject();

            if(enemyCharacters.Count == 0)
            {
                GameManager.Instance.GameOver(true);
            }
        }
    }
}
