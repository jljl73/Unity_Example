using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UK.State
{
    public class ConditionBase
    {
        public Func<bool> CheckTrue { get; private set; }
        public eStateType NextState { get; private set; }
        public ConditionBase(eStateType nextState, Func<bool> checkTrue)
        {
            CheckTrue = checkTrue; 
            NextState = nextState;
        }
    }
}
