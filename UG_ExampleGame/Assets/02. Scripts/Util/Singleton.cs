using UnityEngine;

namespace UK
{
    public class Singleton<T> where T : class, new()
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }

        public virtual void Init()
        {
        }
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    instance.Init();
                }

                if(instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<T>();
                    instance.name = typeof(T).Name;
                    instance.Init();
                }

                return instance;
            }
        }

        protected virtual void Init()
        {
        }
    }
}
