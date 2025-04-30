using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DraggableLetter draggable = dropped.GetComponent<DraggableLetter>();
        if (draggable == null) return;

        if (transform.childCount == 0)
        {
            Text slotText = GetComponentInChildren<Text>();
            Text letterText = draggable.GetComponentInChildren<Text>();
            slotText.text = letterText.text;

            draggable.gameObject.SetActive(false);
        }
    }
}