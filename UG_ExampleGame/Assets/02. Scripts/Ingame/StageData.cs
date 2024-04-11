using System.Collections.Generic;
using UK.Data;
using UnityEngine;

namespace UK
{
    [System.Serializable]
    public struct StageInfo
    {
        public List<CharacterData> Enemies;
    }

    public class StageData : ScriptableObject
    {
        [SerializeField]
        private List<StageInfo> stages = new List<StageInfo>();
        public IReadOnlyList<StageInfo> Stages { get {  return stages; } }
    }
}
