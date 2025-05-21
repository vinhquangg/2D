using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    [HideInInspector] public SlotClass boundSlot;
    public void UpdateSlot(SlotClass slot)
    {
        if (slot.item != null)
        {
            icon.sprite = slot.item.itemIcon;
            icon.enabled = true;

            amountText.text = slot.amount.ToString();


            icon.type = Image.Type.Filled;
            icon.fillMethod = Image.FillMethod.Vertical;
            icon.fillOrigin = (int)Image.OriginVertical.Bottom;
            icon.fillClockwise = false;
        }
        else
        {
            icon.enabled = false;
            amountText.text = "";
        }
    }


    public void StartCooldown(float duration)
    {
        StartCoroutine(HandleCooldown(duration));
    }

    private IEnumerator HandleCooldown(float duration)
    {
        if (icon == null) yield break;

        icon.fillAmount = 1f;
        icon.fillClockwise = false;
        icon.enabled = true;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float remaining = Mathf.Clamp(duration - timer, 0, duration);
            icon.fillAmount = remaining / duration;
            yield return null;
        }
        icon.fillAmount = 1f; 
    }
}
