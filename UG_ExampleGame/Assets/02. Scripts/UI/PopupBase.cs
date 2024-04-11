using System;
using UnityEngine;

namespace UK.UI
{
    public enum ePopupType
    {
        Option,
        BattleWin,
        BattleLose,
        GameOver,
    }

    public abstract class PopupBase : MonoBehaviour
    {
        public abstract ePopupType PopupType { get; }
        private Action hideCallback = null;

        public virtual void Init()
        {
        }

        public virtual void Dispose()
        {
            hideCallback = null;
        }

        public virtual void OnShow(PopupData popupData)
        {
            hideCallback = popupData.HideCallback;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public virtual void OnHide()
        {
            hideCallback?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateView()
        {
        }
    }
}
