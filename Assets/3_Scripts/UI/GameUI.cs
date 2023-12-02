using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    BallShooter ballShooter;
    [SerializeField]
    Transform cameraInputPivot;
    [SerializeField]
    float rotationFactor;

    [SerializeField]
    float rotationTime;

    float rotationSpeed;
    bool dragging = false;
    float targetAngle = 0;
    float currAngle = 0;

    Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        dragging = false;
    }

    private void Update()
    {
        float delta = targetAngle - currAngle;
        if (Mathf.Abs(delta) > Mathf.Epsilon) {
            rotationSpeed = (delta / (rotationTime * Time.timeScale)) * Time.deltaTime;
            currAngle += rotationSpeed;
            cameraInputPivot.Rotate(Vector3.up, rotationSpeed);
        }
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        currAngle = cameraInputPivot.localEulerAngles.y;
        targetAngle = currAngle;
    }

    public void OnPointerDrag(BaseEventData eventData)
    {
        dragging = true;
        PointerEventData pointerEventData = eventData as PointerEventData;
        targetAngle += pointerEventData.delta.x * rotationFactor;
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (!dragging) {
            PointerEventData pointerEventData = eventData as PointerEventData;
            Ray ray = mainCamera.ScreenPointToRay(pointerEventData.position);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.15f, out hit, 100f, 1, QueryTriggerInteraction.Ignore)) {
                TowerTile tile = hit.collider.GetComponent<TowerTile>();
                if (tile && tile.Active)
                    ballShooter.ShootTarget(hit.point, tile);
            }
        } else {
            dragging = false;
        }
    }
}
