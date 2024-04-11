using System;
using System.Collections.Generic;
using UnityEngine;

namespace UK.UI
{
    public class PopupData
    {
        public ePopupType   PopupType       { get; private set; }
        public Action       HideCallback    { get; private set; }
        public object[]     PopupDatas      { get; private set; }

        public PopupData(ePopupType popupType, Action hideCallback, params object[] popupDatas)
        {
            PopupType       = popupType;
            HideCallback    = hideCallback;
            PopupDatas      = popupDatas;
        }
    }

    public class PopupSystem : MonoBehaviour
    {
        [SerializeField]
        private Transform                               popupContainerTrans;
        
        private Dictionary<ePopupType, PopupBase>       popups;
        private Queue<PopupData>                        registPopups;
        private PopupBase                               activePopup;

        public void Init()
        {
            popups          = new Dictionary<ePopupType, PopupBase>();
            registPopups    = new Queue<PopupData>();
            activePopup     = null;

            for (int i = popupContainerTrans.childCount - 1; i >= 0; --i)
            {
                var newPopup = popupContainerTrans.GetChild(i).GetComponent<PopupBase>();
                newPopup.Init();
                popups.Add(newPopup.PopupType, newPopup);
            }
        }

        public void Dispose()
        {
            foreach(var popup in popups.Values)
                popup.Dispose();
            popups.Clear();
            registPopups.Clear();
            activePopup = null;
        }


        public void ShowPopup(PopupData popupData)
        {
            if (popups.ContainsKey(popupData.PopupType) == false)
            {
                Debug.LogError($"{popupData.PopupType} does not Exist!");
                return;
            }

            registPopups.Enqueue(popupData);
            if (activePopup == null)
                ShowPopup();
        }

        public void HidePopup()
        {
            if(activePopup == null)
            {
                Debug.LogError($"Active Popup not Exist!");
                return;
            }

            activePopup.Hide();
            activePopup.OnHide();
            ShowPopup();
        }

        private void ShowPopup()
        {
            if (registPopups.Count == 0)
            {
                activePopup = null;
                return;
            }

            var popupData = registPopups.Dequeue();
            activePopup = popups[popupData.PopupType];
            activePopup.OnShow(popupData);
            activePopup.Show();
        }
    }
}