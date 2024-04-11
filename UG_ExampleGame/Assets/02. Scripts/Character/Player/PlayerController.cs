using System.Collections.Generic;
using UK.Util;
using UnityEngine;
using UK.Data;

namespace UK.Character
{
    public interface iPlayerController
    {
        IReadOnlyList<PlayerCharacter> PlayerCharacters { get; }
        PlayerCharacter GetPlayer(int slotIndex);
        PlayerCharacter GetNearPlayer(Vector3 position);
        void Despawn(PlayerCharacter friendlyCharacter);
        void UseSkill(int skillIndex);
    }

    public class PlayerController : MonoBehaviour, iPlayerController
    {
        [SerializeField]
        private Transform[] slotTrans;
        [SerializeField]
        private CharacterData[] playerDatas;
        
        private List<PlayerCharacter>         playerCharacters  = new List<PlayerCharacter>();
        public IReadOnlyList<PlayerCharacter> PlayerCharacters  => playerCharacters;

        public void Init()
        {
            for (int i = 0; i < DataCenter.Instance.DeckData.Decks.Count; ++i)
                Spawn(DataCenter.Instance.DeckData.Decks[i], i);
        }

        public void Dispose()
        {
            for (int i = 0; i < playerCharacters.Count; ++i)
            {
                playerCharacters[i].Dispose();
                playerCharacters[i].DespawnObject();
            }
            playerCharacters.Clear();
        }

        public void Spawn(int deck, int slotIndex)
        {
            var data    = playerDatas[deck - 1];
            var prefab  = data.Prefab;

            var newCharacter = prefab.SpawnObject(slotTrans[slotIndex]).GetComponent<PlayerCharacter>();
            newCharacter.Init(data);

            playerCharacters.Add(newCharacter);
        }

        public PlayerCharacter GetPlayer(int slotIndex)
        {
            if (playerCharacters.Count <= slotIndex)
                return null;
            return playerCharacters[slotIndex];
        }

        public PlayerCharacter GetNearPlayer(Vector3 position)
        {
            PlayerCharacter targetFriendly = null;
            float minDist = float.MaxValue;

            for (int i = 0; i < playerCharacters.Count; ++i)
            {
                var dist = (playerCharacters[i].transform.position - position).sqrMagnitude;
                if (dist < minDist)
                {
                    targetFriendly = playerCharacters[i];
                    minDist = dist;
                }
            }
            return targetFriendly;
        }

        public void Despawn(PlayerCharacter friendlyCharacter)
        {
            playerCharacters.Remove(friendlyCharacter);
            friendlyCharacter.Dispose();
            friendlyCharacter.DespawnObject();

            if (playerCharacters.Count == 0)
            {
                GameManager.Instance.GameOver(false);
            }
        }

        public void UseSkill(int characterId)
        {
            GameManager.Instance.RegistSkillCast(characterId);
        }
    }
}
