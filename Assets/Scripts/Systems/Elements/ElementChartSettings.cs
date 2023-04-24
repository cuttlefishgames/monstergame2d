using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    [CreateAssetMenu(fileName = "ElementChartSettings", menuName = "Elements/Element Chart Settings")]
    public class ElementChartSettings : ScriptableObject
    {
        [Serializable]
        public class ElementAdvantagesData
        {
            public Elements Element;
            public List<Elements> WeakTo = new List<Elements>();
            public List<Elements> Resists = new List<Elements>();
        }

        [SerializeField] private List<ElementAdvantagesData> _elementsAdvantages = new List<ElementAdvantagesData>();
        [SerializeField] private float _advantageMultiplier = 1.25f;
        [SerializeField] private float _disadvantageMultiplier = 0.75f;

        public List<ElementAdvantagesData> ElementsAdvantages { get => _elementsAdvantages; }
        public float AdvantageMultiplier { get => _advantageMultiplier; }
        public float DisadvantageMultiplier { get => _disadvantageMultiplier; }

        private void OnValidate()
        {
            if (_elementsAdvantages == null || _elementsAdvantages.Count == 0) 
            {
                _elementsAdvantages = new List<ElementAdvantagesData>();

                foreach (Elements element in Enum.GetValues(typeof(Elements)))
                {
                    _elementsAdvantages.Add(new ElementAdvantagesData()
                    {
                        Element = element,
                        WeakTo = new List<Elements>(),
                        Resists = new List<Elements>()
                    });
                }
            }
        }
    }
}