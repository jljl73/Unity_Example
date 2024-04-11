using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UK.Util;
using UnityEngine;

namespace UK.Effect
{
    public class DamageTextEffect : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textDamage;

        public void CreateEffect(float damage, Color color)
        {
            textDamage.text = ((int)damage).ToString();
            textDamage.color = color;

            DoMove().Forget();
        }

        async UniTask DoMove()
        {
            await transform.DOMoveY(transform.position.y + 0.5f, 0.4f).AsyncWaitForCompletion();
            await UniTask.WaitForSeconds(0.3f);
            gameObject.DespawnObject();
        }
    }
}
