using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ClickSwitchSlider : Slider
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        value = value >= (minValue + maxValue) * 0.5f
            ? minValue
            : maxValue;
    }

    public override void OnDrag(PointerEventData eventData)
    {

    }
}