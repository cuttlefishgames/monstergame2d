using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monster
{
    public class ElementChart : Utils.Singleton<ElementChart>
    {
        public static ElementChartSettings ElementChartTable { get => Instance._elementChartSettings; }

        [SerializeField] private ElementChartSettings _elementChartSettings;

        public static List<Elements> GetAdvantages(Elements element)
        {
            var result = ElementChartTable.ElementsAdvantages.Where(e => e.WeakTo.Contains(element)).Select(d => d.Element).ToList();

            return result;
        }

        public static List<Elements> GetDisadvantages(Elements element)
        {
            var result = new List<Elements>();

            var elementData = ElementChartTable.ElementsAdvantages.Where(e => e.Element == element).FirstOrDefault();

            if (elementData != null)
                result = elementData.WeakTo;

            return result;
        }

        public static List<Elements> GetResistances(Elements element)
        {
            var result = new List<Elements>();

            var elementData = ElementChartTable.ElementsAdvantages.Where(e => e.Element == element).FirstOrDefault();

            if (elementData != null)
                result = elementData.Resists;

            return result;
        }

        public static float GetEffectiveness(Elements attacker, Elements defender)
        {
            var result = 1f;

            var defenderAdvantages = ElementChartTable.ElementsAdvantages.Where(d => d.Element == defender).FirstOrDefault();

            if (defenderAdvantages != null && defenderAdvantages.WeakTo.Contains(attacker))
                result = ElementChartTable.AdvantageMultiplier;
            else if (defenderAdvantages != null && defenderAdvantages.Resists.Contains(attacker))
                result = ElementChartTable.DisadvantageMultiplier;

            return result;
        }
    }
}