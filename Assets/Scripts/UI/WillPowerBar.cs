using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WillPowerBar : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] Player player;
    [Header("Settings")]
    [SerializeField] float sliderMoveSpeed = 1;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (slider.value != player.GetWillPower())
        {
            if (player.GetWillPower() > slider.value)
            {
                slider.value += sliderMoveSpeed;
                if (slider.value > player.GetWillPower())
                {
                    slider.value = player.GetWillPower();
                }
            }
            else
            {
                slider.value -= sliderMoveSpeed;
                if (slider.value < player.GetWillPower())
                {
                    slider.value = player.GetWillPower();
                }
            }
        }
    }
}
