using TMPro;
using UnityEngine;

public class GPSController : MonoBehaviour
{
    public double my_lat, my_lon, my_time;

    private LocationService gps_service;
    private LocationServiceStatus gps_status;
    private LocationInfo gps_info;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bearingText;

    private void Start()
    {
        gps_service = Input.location;
        if (!gps_service.isEnabledByUser)
        {
            gps_service.Start();
        }

        gps_service.Start(2.0f, 2.0f);

        InvokeRepeating("GetGPSInfo", 0.0f, 2.0f);
        InvokeRepeating("CheckStatus", 0.0f, 2.0f);
    }

    private void Update()
    {
        GetGPSInfo();
        CalBearing(25.04777f, 121.51738f);
    }
    private void GetGPSInfo()
    {
        gps_info = gps_service.lastData;
        my_lat = gps_info.latitude;
        my_lon = gps_info.longitude;
        my_time = gps_info.timestamp;

        locationText.text = $"Lat: {my_lat}, Lon: {my_lon}";
    }

    private void CheckStatus()
    {
        gps_status = gps_service.status;

        if (gps_status == LocationServiceStatus.Failed){
            Debug.Log("定位服務啟動失敗");
        } else if (gps_status == LocationServiceStatus.Running){
            Debug.Log("定位服務正在運行");
        } else if (gps_status == LocationServiceStatus.Stopped){
            Debug.Log("定位服務已停止");
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
        bearingText.text = $"Bearing:\n {bearing}";
    }
}
