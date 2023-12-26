namespace JengaDemo
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using static UnityEngine.GraphicsBuffer;

    public class CameraController : MonoBehaviour
    {
        /*
        #region Custom Code w/ Zoom

        private CameraControlActions cameraActions;
        private InputAction movement;

        private Camera mainCamera;
        private Transform cameraTransform;

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
        float sensitivity = 1000f;


        // Value set in various functions
        // Used to update the position of the camera base object
        private Vector3 targetPosition;

        private float zoomHeight;

        // USed to track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        [SerializeField] Vector3 targetOffset = Vector3.up;
        [SerializeField] Vector2 orbitSensitivity = Vector2.one;

        float xRot = 0f;
        float yRot = 0f;

        Vector3 targetLookPoint;

        const float TRANSLATE_INTERPOLATE_SPEED = 10.0f;
        const float LOOK_INTERPOLATE_SPEED = 2.0f;
        const float DESIRED_DIST = 10.0f;

        private void Awake()
        {
            cameraActions = new CameraControlActions();
            mainCamera = GetComponentInChildren<Camera>();
            cameraTransform = mainCamera.transform;
        }

        private void OnEnable()
        {
            zoomHeight = cameraTransform.localPosition.y;
            cameraTransform.LookAt(cameraTarget);

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

            xRot += Mathf.DeltaAngle(xRot, xRot - Input.GetAxis("Mouse Y") * sensitivity * Time.unscaledDeltaTime);
            yRot += Mathf.DeltaAngle(yRot, yRot + Input.GetAxis("Mouse X") * sensitivity * Time.unscaledDeltaTime);

            // interpolate camera position
            targetPosition = Vector3.Lerp(targetPosition,
                cameraTarget.transform.TransformPoint(targetOffset) + Quaternion.Euler(xRot, yRot, 0f) * (DESIRED_DIST * Vector3.back),
                TRANSLATE_INTERPOLATE_SPEED * Time.unscaledDeltaTime);
            // interpolate look target point
            targetLookPoint = Vector3.Lerp(targetLookPoint, cameraTarget.transform.TransformPoint(targetOffset),
                LOOK_INTERPOLATE_SPEED * Time.unscaledDeltaTime);

            //Vector3 direction = lastPosition - mainCamera.ScreenToViewportPoint(Input.mousePosition);
            //this.transform.position = cameraTarget.position;

            //this.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            //this.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            //this.transform.Translate(new Vector3(0, 0, -10));

            //
            //float val = context.ReadValue<Vector2>().x;
            //transform.rotation = Quaternion.Euler(0f, val * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
            //
        }

        private void ZoomCamera(InputAction.CallbackContext context)
        {
            float val = -context.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(val) > 0.1f)
            {
                zoomHeight = cameraTransform.localPosition.y + val * stepSize;
                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                        zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition()
        {
            Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
            zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            cameraTransform.LookAt(this.transform);
        }
        #endregion

        */

        [SerializeField] Vector3 targetOffset = Vector3.up;
        [SerializeField] Vector2 orbitSensitivity = Vector2.one;

        
        [SerializeField] JengaStack target;

        float xRot = 0f;
        float yRot = 0f;

        float sensitivity = 1000f;
        Vector3 targetPosition;
        Vector3 targetLookPoint;

        const float TRANSLATE_INTERPOLATE_SPEED = 10.0f;
        const float LOOK_INTERPOLATE_SPEED = 2.0f;
        const float DESIRED_DIST = 10.0f;        

        private void Awake()
        {
            targetPosition = transform.position;
        }

        void Update()
        {
            if (!target)
                return;

            // only use mouse axis while left-click is held
            if (Input.GetMouseButton(2))
            {
                xRot += Mathf.DeltaAngle(xRot, xRot - Input.GetAxis("Mouse Y") * sensitivity * Time.unscaledDeltaTime);
                yRot += Mathf.DeltaAngle(yRot, yRot + Input.GetAxis("Mouse X") * sensitivity * Time.unscaledDeltaTime);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                UpdateTarget(true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                UpdateTarget(false);
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

        public void UpdateTarget(bool isNext)
        {
            if (!AppManager.Instance.hasMadeList)
                AppManager.Instance.StoreLinkedList();

            //targetLookPoint = newTarget.transform.position + ((Vector3.up * 10f) / 2.0f);
            //distance = Vector3.Distance(newTarget.Position, transform.position);
            if (isNext)
                AppManager.Instance.currStack = (AppManager.Instance.currStack.Next != null) ? AppManager.Instance.currStack.Next : AppManager.Instance.stacks.First;
            else
                AppManager.Instance.currStack = (AppManager.Instance.currStack.Previous != null) ? AppManager.Instance.currStack.Previous : AppManager.Instance.stacks.Last;

            target = AppManager.Instance.currStack.Value;
        }
    }
}