using System.Runtime.Serialization;
using UK.Data;
using UniRx;

namespace UK
{
    public class Status
    {
        #region HP
        public ReactiveProperty<float> ReactiveHP   { get; set; }
        public float HP                             { get => ReactiveHP.Value; set => ReactiveHP.Value = value; }
        public float MaxHP                          { get; private set; }
        public bool IsDead                          { get => HP <= 0; }
        #endregion

        private float damage;
        public float Damage                         => damage;

        private float attackSpeed;
        public float AttackSpeed                    { get { return attackSpeed  / (1 + BuffSystem.GetValue(eBuffType.AttackSpeedUp)); } }

        private float attackRange;
        public float AttackRange                    => attackRange;

        private float moveSpeed;
        public float MoveSpeed                      => moveSpeed;                    

        public BuffSystem BuffSystem                { get; private set; }

        public void Init(CharacterData characterData)
        {
            ReactiveHP  = new ReactiveProperty<float>();
            BuffSystem  = new BuffSystem();

            HP          = characterData.MaxHp;
            MaxHP       = characterData.MaxHp;
            damage      = characterData.Damage;
            attackSpeed = characterData.AttackSpeed;
            attackRange = characterData.AttackRange;
            moveSpeed   = characterData.MoveSpeed;
        }

        public void Hit(float damage)
        {
            HP -= damage;
        }

        public void Heal(float value)
        {
            HP += value;
            if (HP > MaxHP)
                HP = MaxHP;
        }

        public void AddBuff(eBuffType buffType, float value, float duration)
        {
            BuffSystem.AddBuff(new Buff(buffType, value, duration));
        }
    }
}