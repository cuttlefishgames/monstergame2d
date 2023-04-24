using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Monster.UI
{
    public class DamageNumberComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Canvas _canvas;
        private int _damage;
        
        public bool Instanced { get => _instanced; }
        private bool _instanced;
        private Coroutine _returnToPool;
        private Vector3 _oriringalPosition;

        public void Enable()
        {
            gameObject.SetActive(true);
            _canvas.worldCamera = Camera.main;
        }

        public void Clear()
        {
            EndCoroutines();
            _textMeshPro.transform.localScale = Vector3.one;
            _textMeshPro.transform.position = _oriringalPosition;
            _textMeshPro.text = string.Empty;
            _damage = 0;
            _instanced = false;
        }

        public void Disable()
        {
            EndCoroutines();
            gameObject.SetActive(false);
        }

        public void EndCoroutines()
        {
            if (_returnToPool != null)
            {
                StopCoroutine(_returnToPool);
            }
            _canvas.transform.DOComplete();
            _textMeshPro.transform.DOComplete();
        }

        public void SetInstanced(bool instanced)
        {
            _instanced = instanced;
        }

        public void SetDamage(int damage, bool miss, bool critical, bool animate = true)
        {
            _damage = damage;
            _textMeshPro.text = _damage.ToString();

            if (miss)
            {
                _textMeshPro.color = Color.gray;
            }
            else if (critical)
            {
                _textMeshPro.color = Color.yellow;
            }
            else
            {
                _textMeshPro.color = Color.white;
            }

            if (animate)
            {
                EndCoroutines();
                _textMeshPro.transform.DOPunchScale(Vector3.one * 1.5f, 0.3f, 3).SetEase(Ease.Linear);
                _canvas.transform.DOMoveY(_canvas.transform.position.y + 2, 3f).SetEase(Ease.Linear);
                if(_returnToPool != null)
                {
                    StopCoroutine(_returnToPool);
                }
                _returnToPool = StartCoroutine(ReturnToPool(3f));
            }
        }

        IEnumerator ReturnToPool(float delay)
        {
            yield return new WaitForSeconds(delay);

            DamageNumbersManager.ReturnDamageNumberToPool(this);
        }

        public void ConcatDamage(int damage, bool miss, bool critical, bool animate = true)
        {
            _damage += damage;
            SetDamage(_damage, miss, critical, animate);
        }

        public void SetWorldPosition(Transform point)
        {
            _oriringalPosition = point.position;
            _canvas.transform.position = _oriringalPosition;
        }
    }
}