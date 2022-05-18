using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ElementPickerColorChange : MonoBehaviour
{
    public Color[] Colors;

    [SerializeField] Image SliderImage;
    [SerializeField] Image PickerImage;

    Slider _slider;

    private void Start()
    {
        _slider = gameObject.GetComponent<Slider>();

        _slider.onValueChanged.AddListener(delegate { ChangeImageToColor(); });
    }

    public void ChangeImageToColor()
    {
        Debug.Log((_slider.value * 2) - 2);
        SliderImage.color = Colors[2 * ((int)_slider.value - 2)];
        PickerImage.color = Colors[2 * ((int)_slider.value - 1)];
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(delegate { ChangeImageToColor(); });
    }
}
