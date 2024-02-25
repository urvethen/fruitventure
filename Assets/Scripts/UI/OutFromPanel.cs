
using UnityEngine;
using UnityEngine.EventSystems;

public class OutFromPanel: MonoBehaviour, IPointerExitHandler
{

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.localScale.x >= 0.6f)
        UIManager.Instance.HidePanel();
    }
}
