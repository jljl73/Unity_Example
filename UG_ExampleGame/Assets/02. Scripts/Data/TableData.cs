using UnityEngine;

namespace UK.Data
{
    public class TableData : ScriptableObject
    {
        static TableData instance;
        public static TableData Instance
        {
            get
            {
                if(instance == null)
                    instance = Resources.Load<TableData>("Table/TableData");

                return instance;
            }
        }

        [SerializeField]
        private CharacterData[] friendlyData;
        public CharacterData[] FriendlyData =>friendlyData;

        [SerializeField]
        private CharacterData[] enemyData;
        public CharacterData[] EnemyData =>enemyData;

        public CharacterData GetCharacterData(int characterId)
        {
            for(int i = 0; i < friendlyData.Length; i++)
            {
                if (friendlyData[i].CharacerId == characterId)
                    return friendlyData[i];
            }

            for (int i = 0; i < enemyData.Length; i++)
            {
                if (enemyData[i].CharacerId == characterId)
                    return enemyData[i];
            }
            return null;
        }
    }
}
