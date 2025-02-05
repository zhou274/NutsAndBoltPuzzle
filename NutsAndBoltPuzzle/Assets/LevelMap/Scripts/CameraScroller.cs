using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform player;

    public float scrollSpeed = 0.5f; // Speed at which the camera moves up
    public float maxDistance = 5f; // Maximum distance the camera can move up

    private float originalYPosition;

    bool isMovingBack = false;
    void Start()
    {
        originalYPosition = virtualCamera.transform.position.y;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.y < 0)
            {
                // Get the movement delta of the touch
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;

                // Move the camera up based on the scroll delta, clamped to maxDistance
                float newYPosition = Mathf.Clamp(virtualCamera.transform.position.y - touchDelta.y * scrollSpeed * Time.deltaTime, originalYPosition, originalYPosition + maxDistance);
                virtualCamera.transform.position = new Vector3(virtualCamera.transform.position.x, newYPosition, virtualCamera.transform.position.z);
            }
        }
        else if (Input.touchCount == 0 && transform.position.y != originalYPosition)
        {
            if(isMovingBack)
            {
                return;
            }
            SetInitialPos();
            isMovingBack = true;
        }
    }

    private void SetInitialPos()
    {
        transform.DOMove(new Vector3(virtualCamera.transform.position.x, originalYPosition, virtualCamera.transform.position.z),0.5f,false).OnComplete(()=> isMovingBack = false);
    }
}
