using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MovementScript : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed = 5f;
    private float distanceToCamera;
    // Start is called before the first frame update
    void Start()
    {
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
    }


    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (var touch in Touch.activeTouches)
        {
            if (touch.finger.index == 0)
            {
                Vector2 touchPosition = touch.screenPosition;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                Vector3 startPosition = ray.origin;
                Vector3 direction = ray.direction;
                float t = (transform.position.y - startPosition.y) / direction.y;
                Vector3 position = new Vector3(startPosition.x + t * direction.x, transform.position.y, startPosition.z + t * direction.z);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * movementSpeed);
            }
        }


    }
}


