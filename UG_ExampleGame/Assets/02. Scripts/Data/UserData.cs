using UniRx;
using UnityEngine;

namespace UK.Data
{
    public class UserData : DataModelBase
    {
        private ReactiveProperty<int> gold = new ReactiveProperty<int>();
        public IReactiveProperty<int> Gold => gold;


        private ReactiveProperty<string> nickName = new ReactiveProperty<string>();
        public IReactiveProperty<string> NickName => nickName;

        private ReactiveProperty<int> stage = new ReactiveProperty<int>();
        public IReactiveProperty<int> Stage => stage;

        private ReactiveProperty<int> clearStageLevel = new ReactiveProperty<int>();
        public IReactiveProperty<int> ClearStageLevel => clearStageLevel;

        public override void Init()
        {
            clearStageLevel.Value = PlayerPrefs.GetInt("ClearStage", 0);
        }

        public override void Dispose()
        {
        }

        public void ClearStage()
        {
            int stageLevel = stage.Value;
            if (clearStageLevel.Value < stageLevel + 1)
                clearStageLevel.Value = stageLevel + 1;
            ++stage.Value;
            PlayerPrefs.SetInt("ClearStage", clearStageLevel.Value);
        }
    }
}
