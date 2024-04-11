using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UK
{
    public enum eBuffType
    {
        AttackSpeedUp,
    }

    public class Buff
    {
        public eBuffType BuffType { get; private set; }
        public float Value { get; private set; }
        public float Duration { get; private set; }

        public Buff(eBuffType buffType, float value, float duration)
        {
            BuffType = buffType;
            Value = value;
            Duration = duration;
        }
    }


    public class BuffSystem
    {
        private List<Buff> activeBuffs = new List<Buff>();

        public void AddBuff(Buff buff)
        {
            activeBuffs.Add(buff);
            Observable.Timer(TimeSpan.FromSeconds(buff.Duration)).Subscribe(_ => activeBuffs.Remove(buff));
        }

        public float GetValue(eBuffType buffType)
        {
            float valueSum = 0;
            for (int i = 0; i < activeBuffs.Count; ++i)
            {
                if (activeBuffs[i].BuffType == buffType)
                {
                    valueSum += activeBuffs[i].Value;
                }
            }
            return valueSum;
        }
    }
}
