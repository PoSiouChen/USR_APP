using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleText : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public RectTransform arrowImage;

    public void OnButtonClick()
    {
        if (targetText != null)
        {
            targetText.gameObject.SetActive(!targetText.gameObject.activeSelf);
        }

        if (arrowImage != null)
        {
            arrowImage.localScale = new Vector3(
                arrowImage.localScale.x,
                arrowImage.localScale.y * -1, // 上下翻转
                arrowImage.localScale.z
            );
        }
    }
}

