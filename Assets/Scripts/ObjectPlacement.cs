using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    bool objPlaced = false;
    [SerializeField]
    private Renderer renderer;
    private bool isPlacing;
    

    public void EnablePlacementMode(GameObject model)
    {
        isPlacing = true;
        obj = model;
    }
    void Start()
    {
        //arPlaneManager = GetComponent<ARPlaneManager>();
        renderer = obj.GetComponent<Renderer>();
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
        if (isPlacing)
        {
            foreach (var touch in Touch.activeTouches)
            {
                Vector2 touchPosition = touch.screenPosition;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) == true)
                {
                    if (hit.collider.CompareTag("Plane") && objPlaced == false)
                    {
                        Vector3 hitPoint = hit.point;
                        float objectHeight = renderer.bounds.size.y;
                        //hitPoint.y += objectHeight / 2f;
                        hitPoint.y += 0.15f;
                        Instantiate(obj, hitPoint, obj.transform.rotation);

                        objPlaced = true;
                    }

                }
            }
        }
        }

    }
//}
