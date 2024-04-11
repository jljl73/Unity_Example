using System.Collections;
using System.Collections.Generic;
using UK.Effect;
using UK.Util;
using UnityEngine;

namespace UK
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField]
        private GameObject damageTextEffectPrefab;
        
        public void CreateDamageText(Vector3 position, float damage, bool isEnemy)
        {
            var effect = damageTextEffectPrefab.SpawnObject().GetComponent<DamageTextEffect>();
            var randomCircle = Random.insideUnitCircle * 0.4f;

            effect.transform.position = position + new Vector3(randomCircle.x, randomCircle.y, 0);
            effect.CreateEffect(damage, isEnemy ? Color.white : Color.red);
        }

        public void CreateHealText(Vector3 position, float damage)
        {
            var effect = damageTextEffectPrefab.SpawnObject().GetComponent<DamageTextEffect>();
            var randomCircle = Random.insideUnitCircle * 0.4f;

            effect.transform.position = position + new Vector3(randomCircle.x, randomCircle.y, 0);
            effect.CreateEffect(damage, new Color(0, 1, 0));
        }
    }
}
