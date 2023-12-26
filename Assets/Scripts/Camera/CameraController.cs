namespace JengaDemo
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CameraController : MonoBehaviour
    {
        private CameraControlActions cameraActions;
        private InputAction movement;

        private Camera mainCamera;
        private Transform mainTransform;

        [SerializeField] Transform cameraTarget;

        // Vertical Motion - zooming
        [SerializeField]
        private float stepSize = 2f;
        [SerializeField]
        private float zoomDampening = 7.5f;
        [SerializeField]
        private float minHeight = 5f;
        [SerializeField]
        private float maxHeight = 50f;
        [SerializeField]
        private float zoomSpeed = 2f;

        // Rotation
        [SerializeField]
        private float maxRotationSpeed = 1f;

        // Value set in various functions
        // Used to update the position of the camera base object
        private Vector3 targetPosition;

        private float zoomHeight;

        // USed to track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        // Tracks where the dragging action started
        Vector3 startDrag;

        private void Awake()
        {
            cameraActions = new CameraControlActions();
            mainCamera = GetComponentInChildren<Camera>();
            mainTransform = mainCamera.transform;
        }

        private void OnEnable()
        {
            zoomHeight = mainTransform.localPosition.y;
            mainTransform.LookAt(cameraTarget);

            lastPosition = this.transform.position;

            cameraActions.Camera.RotateCamera.performed += RotateCamera;
            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Enable();
        }

        private void OnDisable()
        {
            cameraActions.Camera.RotateCamera.performed -= RotateCamera;
            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Disable();
        }

        private void Update()
        {
            UpdateCameraPosition();
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
            horizontalVelocity.y = 0;
            lastPosition = this.transform.position;
        }

        private void RotateCamera(InputAction.CallbackContext context)
        {
            if (!Mouse.current.middleButton.isPressed)
                return;

            Vector3 direction = lastPosition - mainCamera.ScreenToViewportPoint(Input.mousePosition);
            mainTransform.position = cameraTarget.position;

            /*
            float val = context.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, val * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
            */
        }

        private void ZoomCamera(InputAction.CallbackContext context)
        {
            float val = -context.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(val) > 0.1f)
            {
                zoomHeight = mainTransform.localPosition.y + val * stepSize;
                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                        zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition()
        {
            Vector3 zoomTarget = new Vector3(mainTransform.localPosition.x, zoomHeight, mainTransform.localPosition.z);
            zoomTarget -= zoomSpeed * (zoomHeight - mainTransform.localPosition.y) * Vector3.forward;

            mainTransform.localPosition = Vector3.Lerp(mainTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            mainTransform.LookAt(cameraTarget);
        }
        /*
        [SerializeField] Vector3 targetOffset = Vector3.up;
        [SerializeField] Vector2 orbitSensitivity = Vector2.one;

        JengaStack target;

        float xRot = 0f;
        float yRot = 0f;

        float sensitivity = 1000f;
        Vector3 targetPosition;
        Vector3 targetLookPoint;

        const float TRANSLATE_INTERPOLATE_SPEED = 10.0f;
        const float LOOK_INTERPOLATE_SPEED = 2.0f;
        const float DESIRED_DIST = 10.0f;

        void Awake()
        {
            targetPosition = transform.position;
        }

        void OnDestroy()
        {

        }

        public void UpdateTarget(JengaStack newTarget)
        {
            targetLookPoint = newTarget.Position;
            //distance = Vector3.Distance(newTarget.Position, transform.position);
            target = newTarget;
        }

        void Update()
        {
            if (!target)
                return;

            // only use mouse axis while left-click is held
            if (Input.GetMouseButton(0))
            {
                xRot += Mathf.DeltaAngle(xRot, xRot - Input.GetAxis("Mouse Y") * sensitivity * Time.unscaledDeltaTime);
                yRot += Mathf.DeltaAngle(yRot, yRot + Input.GetAxis("Mouse X") * sensitivity * Time.unscaledDeltaTime);
            }

            // interpolate camera position
            targetPosition = Vector3.Lerp(targetPosition,
                target.transform.TransformPoint(targetOffset) + Quaternion.Euler(xRot, yRot, 0f) * (DESIRED_DIST * Vector3.back),
                TRANSLATE_INTERPOLATE_SPEED * Time.unscaledDeltaTime);
            // interpolate look target point
            targetLookPoint = Vector3.Lerp(targetLookPoint, target.transform.TransformPoint(targetOffset),
                LOOK_INTERPOLATE_SPEED * Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            if (!target)
                return;

            if (xRot > 89f)
            {
                xRot = 89f;
            }
            else if (xRot < -89f)
            {
                xRot = -89f;
            }

            transform.position = targetPosition;
            transform.LookAt(targetLookPoint, Vector3.up);
        }
        */
    }
}