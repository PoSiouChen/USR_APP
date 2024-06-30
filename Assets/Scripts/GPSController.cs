using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Collections;

public class GPSController : MonoBehaviour
{
    public double my_lat, my_lon, my_time;

    private LocationService gps_service;
    private LocationServiceStatus gps_status;
    private LocationInfo gps_info;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bearingText;

    public Image arrowImage; // UI箭頭圖片

    public double targetLatitude = 25.04777f; // 目標建築物的緯度
    public double targetLongitude = 121.51738f; // 目標建築物的經度

    private IEnumerator Start()
    {
        // 請求位置權限
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                yield return new WaitForSeconds(1); // 等待權限請求完成
            }
        }

        gps_service = Input.location;
        if (!gps_service.isEnabledByUser)
        {
            Debug.Log("GPS not enabled by user");
            yield break;
        }

        // 設置更新間隔為 2 秒，最小距離為 1 米
        gps_service.Start(2.0f, 1.0f);

        // 等待 GPS 服務初始化完成
        int maxWait = 20;
        while (gps_service.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("GPS 初始化超時");
            yield break;
        }

        if (gps_service.status == LocationServiceStatus.Failed)
        {
            Debug.Log("定位服務啟動失敗");
            yield break;
        }
        else if (gps_service.status == LocationServiceStatus.Running)
        {
            InvokeRepeating("UpdateGPSAndBearing", 0.0f, 2.0f);
            InvokeRepeating("CheckStatus", 0.0f, 2.0f);
        }

        Input.compass.enabled = true; // 啟動指南針
    }

    private void Update()
    {
        // 每幀更新箭頭方向
        UpdateArrow();
    }

    private void UpdateGPSAndBearing()
    {
        GetGPSInfo();
        CalBearing(targetLatitude, targetLongitude);
    }

    private void GetGPSInfo()
    {
        gps_info = gps_service.lastData;
        my_lat = gps_info.latitude;
        my_lon = gps_info.longitude;
        my_time = gps_info.timestamp;

        locationText.text = $"Lat: {my_lat},\nLon: {my_lon}";
    }

    private void CheckStatus()
    {
        gps_status = gps_service.status;

        switch (gps_status)
        {
            case LocationServiceStatus.Failed:
                Debug.Log("定位服務啟動失敗");
                break;
            case LocationServiceStatus.Running:
                Debug.Log("定位服務正在運行");
                break;
            case LocationServiceStatus.Stopped:
                Debug.Log("定位服務已停止");
                break;
            case LocationServiceStatus.Initializing:
                Debug.Log("定位服務正在初始化");
                break;
        }
    }

    private void CalBearing(double lat, double lng)
    {
        double phi1 = my_lat * Mathf.PI / 180.0;
        double phi2 = lat * Mathf.PI / 180.0;
        double lam1 = my_lon * Mathf.PI / 180.0;
        double lam2 = lng * Mathf.PI / 180.0;
        double y = Mathf.Sin((float)(lam2 - lam1)) * Mathf.Cos((float)phi2);
        double x = Mathf.Cos((float)phi1) * Mathf.Sin((float)phi2) - Mathf.Sin((float)phi1) * Mathf.Cos((float)phi2) * Mathf.Cos((float)(lam2 - lam1));
        double bearing = (Mathf.Atan2((float)y, (float)x) * 180) / Mathf.PI;

        bearing = (bearing + 360) % 360; // 確保方位角在0到360之間
        bearingText.text = $"Bearing:\n {bearing}";
    }

    private void UpdateArrow()
    {
        // 使用指南針的真方位來旋轉箭頭
        float heading = Input.compass.trueHeading;
        
        // 平滑旋轉
        Quaternion targetRotation = Quaternion.Euler(0, 0, -heading);
        arrowImage.rectTransform.rotation = Quaternion.Lerp(arrowImage.rectTransform.rotation, targetRotation, Time.deltaTime * 5);
    }
}
