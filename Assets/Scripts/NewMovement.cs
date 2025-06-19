using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.InputSystem.EnhancedTouch;

public class NewMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    private bool touchingScreen;
    private Vector3 previousPosition;
    private Vector3 previousVelocity;
    private Vector3 acceleration;

    private Vector3 smoothedAcceleration = Vector3.zero;
    private float smoothSpeed = 0.6f;
    [SerializeField]
    private float springConstant = 10f;

    [SerializeField]
    private float damping = 3f;
    private float stopTime;
    private Vector3 stopValue;
    private Material mat;
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        previousVelocity = Vector3.zero;
        previousPosition = transform.position;

        Renderer childRenderer = transform.GetChild(0).GetComponent<Renderer>();
        mat = childRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.finger.index == 0 && touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 touchPosition = touch.screenPosition;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Cylinder"))
                {
                    touchingScreen = true;
                }

            }

            if (touch.finger.index == 0 && touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                touchingScreen = false;
                stopTime = Time.time;
                stopValue = acceleration;
            }


        }
        if (touchingScreen == true)
        {
            Vector2 touchPosition = Touch.activeTouches[0].screenPosition;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            Vector3 startPosition = ray.origin;
            Vector3 direction = ray.direction;
            float t = (transform.position.y - startPosition.y) / direction.y;
            Vector3 targetPosition = new Vector3(startPosition.x + t * direction.x, transform.position.y, startPosition.z + t * direction.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementSpeed);

            Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;

            Vector3 realAcceleration = (velocity - previousVelocity) / Time.deltaTime;
            smoothedAcceleration = Vector3.Lerp(smoothedAcceleration, realAcceleration, Time.deltaTime * smoothSpeed);

            acceleration = new Vector3(smoothedAcceleration.x, 0f, smoothedAcceleration.z);
            previousVelocity = velocity;
            previousPosition = transform.position;

            Quaternion targetTilt = Quaternion.LookRotation(acceleration, Vector3.up);

            float tiltAmount = 45f;
            Vector3 tiltEuler = new Vector3(-acceleration.z * tiltAmount, 0f, acceleration.x * tiltAmount);

            Quaternion finalTilt = Quaternion.Euler(tiltEuler);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalTilt, Time.deltaTime * 5f);

        }
        else
        {
            float frequency = 6f;
            float damping = 1f;

            float t = Time.time - stopTime;
            acceleration.x = stopValue.x * Mathf.Cos(t * frequency) * Mathf.Exp(-t * damping);
            acceleration.z = stopValue.z * Mathf.Cos(t * frequency) * Mathf.Exp(-t * damping);


            float tiltAmount = 45f;
            Vector3 tiltEuler = new Vector3(-acceleration.z * tiltAmount, 0f, acceleration.x * tiltAmount);
            Quaternion finalTilt = Quaternion.Euler(tiltEuler);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalTilt, Time.deltaTime * 5f);
        }

            mat.SetFloat("_MovementX", acceleration.x);
            mat.SetFloat("_MovementZ", acceleration.z);
    }
}