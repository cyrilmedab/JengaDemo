namespace JengaDemo
{
    using UnityEngine;

    public class OrbitalControls : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] Transform cameraTarget;

        private void Awake()
        {
            mainCamera = GetComponent<Camera>(); 
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