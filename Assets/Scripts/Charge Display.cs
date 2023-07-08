using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ChargeDisplay : MonoBehaviour
    {
        public Slider leftSlider;
        public Slider rightSlider;

        private void Awake()
        {
            ClearSlider();
        }

        public void UpdateSlider(float value)
        {
            if (value < 0)
            {
                leftSlider.value = -1*value;
            }
            else
            {
                rightSlider.value = value;
            }
        }

        public void ClearSlider()
        {
            leftSlider.value = 0;
            rightSlider.value = 0;
        }
    }
}