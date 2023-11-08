using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ClickObject : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager arRaycastManager;

    public GameObject[] info_canvas;

    public bool isCanvasOpen = false;

    private GameObject activeCanvas;

    public GameObject mainCanvasUI;
    private float currentSize;
    float minSize, maxSize;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        currentSize=1f;
        minSize=0.1f;
        maxSize=8.0f;
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    foreach(GameObject factoryCanvas in info_canvas)
                    {
                        if(hitObject.collider.tag == factoryCanvas.tag)
                        {
                            mainCanvasUI.SetActive(false);
                            factoryCanvas.transform.position = hitObject.collider.transform.position + hitObject.collider.transform.TransformDirection(new Vector3(-0.2f, 0, 0.2f));
                            factoryCanvas.transform.rotation = Quaternion.LookRotation(factoryCanvas.transform.position- arCamera.transform.position);
                            factoryCanvas.SetActive(true);
                            isCanvasOpen = true;
                            activeCanvas = factoryCanvas;
                        }
    
                        if(hitObject.collider.name == factoryCanvas.name)
                        {
                            Debug.Log("YOU HAVE TOUCHED A CANVASSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");

                        }

                    }
                }
            } 
        }
        if(activeCanvas != null)
        {
            activeCanvas.transform.rotation = Quaternion.LookRotation(activeCanvas.transform.position- arCamera.transform.position);
        }

        foreach(GameObject factoryCanvas in info_canvas){
            if(factoryCanvas.activeSelf){
                PanZoom(factoryCanvas);
            }
        }
        
    }

    void PanZoom(GameObject factory){
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch (0);
            Touch touchOne = Input.GetTouch (1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            //currentSize -= deltaMagnitudeDiff*0.001f;
            
            // if (currentSize >= maxSize)
            //     currentSize = maxSize;
            // else if (currentSize <= minSize)
            //     currentSize = minSize;
            Vector3 direction=factory.transform.position-arCamera.transform.position;
            factory.transform.position+=(direction.normalized)*deltaMagnitudeDiff*0.001f;
            //factory.transform.localScale=Vector3.one*currentSize;
        }
        
        if(Input.touchCount == 1){
            var screenPoint = RectTransformUtility.WorldToScreenPoint(arCamera, factory.transform.position);
            // Add the deltaPosition
			screenPoint += Input.GetTouch(0).deltaPosition * 1f;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(factory.transform.GetChild(0).GetComponent<RectTransform>(), screenPoint, arCamera, out worldPoint) == true){
				factory.transform.position = worldPoint;
			}
        }
    }
}
