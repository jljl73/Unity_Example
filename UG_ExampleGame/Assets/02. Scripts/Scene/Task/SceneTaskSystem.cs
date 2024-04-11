using System.Collections;
using System.Collections.Generic;
using UK.Scene;
using UnityEngine;

namespace UK
{
    public class SceneTaskSystem : MonoBehaviour
    {
        public SceneTaskBase        CurrentTask { get; private set; }

        private List<SceneTaskBase> tasks       = new List<SceneTaskBase>();

        public void Init()
        {
            tasks.Clear();
            for (int i = 0; i < transform.childCount; ++i)
            {
                var task = transform.GetChild(i).GetComponent<SceneTaskBase>();
                task.Init();
                tasks.Add(task);
            }
            UpdateActiveTask(tasks[0]);
        }

        public void Dispose()
        {
            CurrentTask.Dispose();
            for (int i = 0; i < tasks.Count; ++i)
                tasks[i].Dispose();
            tasks.Clear();
        }

        public void UpdateActiveTask(eSceneTaskType sceneTaskType)
        {
            for (int i = 0; i < tasks.Count; ++i)
            {
                if (tasks[i].SceneTaskType == sceneTaskType)
                {
                    UpdateActiveTask(tasks[i]);
                    return;
                }
            }
        }

        private void UpdateActiveTask(SceneTaskBase sceneTask)
        {
            if (CurrentTask == sceneTask)
                return;
            if (CurrentTask != null)
                CurrentTask.DeactiveTask();

            CurrentTask = sceneTask;
            CurrentTask.ActiveTask();
        }
    }
}
