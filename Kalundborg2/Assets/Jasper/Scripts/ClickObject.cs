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
    
    public GameObject river;
    private Vector3 tracked_image_position;

    public GameObject[] info_canvas;

    public bool isCanvasOpen = false;

    private GameObject activeCanvas;

    public bool FoundWaterSymbiosis = false;

    float minSize, maxSize;

    
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        minSize = 0.001f;
        maxSize = 0.1f;
    }

    void Update()
    {
        
        if(Input.touchCount > 0) //Check if screen is touched
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touchPosition); //Send a Ray from where we touched the screen
                RaycastHit hitObject;
                
                if (Physics.Raycast(ray, out hitObject))  //If statement that checks if our Ray hits an object
                {            
                    foreach(GameObject factoryCanvas in info_canvas)
                    {
                        if(hitObject.collider.tag == factoryCanvas.tag) //Check if object hit is a factory model and if so, instantiate corresponding factory panels
                        {
                            // tracked_image_position = new Vector3(hitObject.collider.transform.position.x, hitObject.collider.transform.position.y, hitObject.collider.transform.position.z);
                            // factoryCanvas.transform.position = tracked_image_position + hitObject.collider.transform.TransformDirection(new Vector3(-0.2f, 1f, 0.2f));
                            tracked_image_position = hitObject.collider.transform.position;
                            Vector3 dir = arCamera.transform.forward;
                            factoryCanvas.transform.position = hitObject.collider.transform.position + Vector3.Normalize(new Vector3(dir.x,0f,dir.z)) * 1f + (new Vector3(0f, 1.5f, 0f));
                            factoryCanvas.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
                            
                            factoryCanvas.SetActive(true);
                            isCanvasOpen = true;
                            activeCanvas = factoryCanvas;
                            
                            if(hitObject.collider.tag == "factory4")
                            {
                                //If statement to set 'FoundWaterSymbiosis' true, indicating that we have found the Water Refinery induestry
                                FoundWaterSymbiosis = true;
                            }
                            
                        }

                    }


                }
            } 
        }

        // if(activeCanvas != null)
        // {
        //     activeCanvas.transform.rotation = Quaternion.LookRotation(activeCanvas.transform.position - arCamera.transform.position);
        // }

        foreach(GameObject factoryCanvas in info_canvas){
            if(factoryCanvas.activeSelf){
                PanZoom(factoryCanvas);
            }
        }
    }

    public void ShowRiver() //Instantiate river model from position of panels
    {
        river.transform.position = tracked_image_position;
        Vector3 dir = arCamera.transform.forward;
        river.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0f,dir.z), Vector3.up);
        river.SetActive(true);
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

            // currentSize -= deltaMagnitudeDiff*0.0001f;
        
            // Vector3 direction = factory.transform.position - arCamera.transform.position;
            // factory.transform.position += (direction.normalized)*deltaMagnitudeDiff*0.001f;
            // Debug.Log(currentSize);
            factory.transform.localScale -= Vector3.one * deltaMagnitudeDiff*0.00001f;
            if (factory.transform.localScale.x >= maxSize || factory.transform.localScale.x <= minSize)
                factory.transform.localScale += Vector3.one * deltaMagnitudeDiff*0.00001f;
        }
    }

}
