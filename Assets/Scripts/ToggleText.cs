using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleText : MonoBehaviour
{
    public TextMeshProUGUI targetText;

    public void OnButtonClick()
    {
        if (targetText != null){
            targetText.gameObject.SetActive(!targetText.gameObject.activeSelf);
        } 
    }
}
