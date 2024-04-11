using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UK.UI
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract void Init();

        public abstract void Dispose();

        public abstract void UpdateView();

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
