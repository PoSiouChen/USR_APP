using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GPSEditorSimulator : MonoBehaviour
{
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bearingText;
    public Image arrowImage;

    public double simulatedLatitude = 25.15111f;
    public double simulatedLongitude = 121.78019f;
    public float simulatedHeading = 0.0f;

    private void Start()
    {
        // 初始化模擬數據
        UpdateGPSInfo();
    }

    private void Update()
    {
        // 更新模擬數據
        UpdateGPSInfo();
        UpdateArrow();
    }

    private void UpdateGPSInfo()
    {
        locationText.text = $"Lat: {simulatedLatitude},\nLon: {simulatedLongitude}";
        bearingText.text = $"Bearing:\n {simulatedHeading}";
    }

    private void UpdateArrow()
    {
        arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, -simulatedHeading);
    }
}

