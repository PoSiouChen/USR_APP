using UnityEngine;

public class RotateCameraAroundObject : MonoBehaviour
{
    public float rotationSpeed = 0.5f; // 旋轉速度
    public float smoothTime = 0.1f; // 平滑時間
    public RectTransform controlArea; // 控制旋轉的區域
    public Camera camera; // 旋轉的相機

    private Vector3 targetPosition; // 目標物體的中心位置
    private float currentAngleX;
    private float currentAngleY;
    private float targetAngleX;
    private float targetAngleY;
    private float angleVelocityX;
    private float angleVelocityY;

    private float distanceFromTarget; // 相機到物體的距離

    private bool isTouchingControlArea = false;
    private Vector3 initialCameraPosition; // 初始相機位置
    private Quaternion initialCameraRotation; // 初始相機旋轉

    private const float moveThreshold = 1.0f;
    private Vector2 lastTouchPosition;
    private bool isMoving = false;

    void Start()
    {
        // 設置目標物體的中心位置
        targetPosition = transform.position;

        // 保存初始相機位置和旋轉
        initialCameraPosition = camera.transform.position;
        initialCameraRotation = camera.transform.rotation;

        // 計算相機到物體的距離
        distanceFromTarget = Vector3.Distance(initialCameraPosition, targetPosition);

        Vector3 directionToTarget = targetPosition - initialCameraPosition;
        Quaternion rotation = Quaternion.LookRotation(directionToTarget);

        Vector3 eulerAngles = rotation.eulerAngles;
        currentAngleY = eulerAngles.y;
        currentAngleX = eulerAngles.x;

        UpdateCameraPosition();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        { // 手指
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouchingControlArea = IsTouchingControlArea(touch.position);
                lastTouchPosition = touch.position;
                isMoving = false;
            }

            if (isTouchingControlArea && touch.phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = touch.position - lastTouchPosition;
                
                if (!isMoving && deltaPosition.magnitude > moveThreshold)
                {
                    isMoving = true;
                }

                if (isMoving)
                {
                    float deltaX = deltaPosition.x; // 手指水平移動距離
                    float deltaY = deltaPosition.y; // 手指垂直移動距離
                    targetAngleY += deltaX * rotationSpeed * Time.deltaTime;
                    targetAngleX = Mathf.Clamp(targetAngleX - deltaY * rotationSpeed * Time.deltaTime, 10f, 80f); // 限制X軸旋轉範圍

                    lastTouchPosition = touch.position;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouchingControlArea = false;
                isMoving = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        { // 滑鼠點擊
            isTouchingControlArea = IsTouchingControlArea(Input.mousePosition);
            lastTouchPosition = Input.mousePosition;
            isMoving = false;
        }
        else if (Input.GetMouseButton(0) && isTouchingControlArea)
        { // 滑鼠拖動
            Vector2 deltaPosition = (Vector2)Input.mousePosition - lastTouchPosition;
            
            if (!isMoving && deltaPosition.magnitude > moveThreshold)
            {
                isMoving = true;
            }

            if (isMoving)
            {
                float deltaX = deltaPosition.x; // 滑鼠水平移動距離
                float deltaY = deltaPosition.y; // 滑鼠垂直移動距離
                targetAngleY += deltaX * rotationSpeed * Time.deltaTime;
                targetAngleX = Mathf.Clamp(targetAngleX - deltaY * rotationSpeed * Time.deltaTime, 0f, 50f); // 限制X軸旋轉範圍

                lastTouchPosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        { // 滑鼠放開
            isTouchingControlArea = false;
            isMoving = false;
        }

        // 平滑旋轉
        currentAngleY = Mathf.SmoothDampAngle(currentAngleY, targetAngleY, ref angleVelocityY, smoothTime);
        currentAngleX = Mathf.SmoothDampAngle(currentAngleX, targetAngleX, ref angleVelocityX, smoothTime);

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // 計算相機新位置
        Quaternion rotation = Quaternion.Euler(currentAngleX, currentAngleY, 0f);
        Vector3 direction = new Vector3(0, 0, -distanceFromTarget);

        camera.transform.position = targetPosition + rotation * direction;

        // 相機面向目標物體
        camera.transform.LookAt(targetPosition);
    }

    private bool IsTouchingControlArea(Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(controlArea, screenPosition, camera, out localPoint);
        return controlArea.rect.Contains(localPoint);
    }
}
