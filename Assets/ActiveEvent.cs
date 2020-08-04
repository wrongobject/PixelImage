using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveEvent : EventTrigger
{
    public GameObject target;   
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        OnClickButton();
    }

    void OnClickButton()
    {
        if (!target) return;
        if (InputLogic.CtrlDown) return;
        target.SetActive(!target.activeSelf);
    }
}
