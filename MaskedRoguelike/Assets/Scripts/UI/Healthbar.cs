using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Slider slider;

    public void SetHealth(float value)
    {
        slider.value = value;
    }

    public void SetMaxHealth(float value)
    {
        slider.maxValue = value;
    }
}
