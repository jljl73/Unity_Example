using UnityEngine;

namespace UK.Data
{
    [CreateAssetMenu]
    public class CharacterData : ScriptableObject
    {
        [SerializeField]
        private int     characterId;
        public int      CharacerId      => characterId;

        [SerializeField]
        private GameObject  prefab;
        public GameObject   Prefab      => prefab;

        [SerializeField]
        private float   maxHp;
        public float    MaxHp           => maxHp;

        [SerializeField]
        private float   damage;
        public float    Damage          => damage;

        [SerializeField]
        private float   attackSpeed;
        public float    AttackSpeed     => attackSpeed;

        [SerializeField]
        private float   attackRange;
        public float    AttackRange     => attackRange;


        [SerializeField]
        private float   moveSpeed;
        public float    MoveSpeed       => moveSpeed;

        [SerializeField]
        private float   coolTime;
        public float    CoolTime        => coolTime;
    }
}