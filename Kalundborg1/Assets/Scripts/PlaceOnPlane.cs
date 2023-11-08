using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        //GameObject m_PlacedPrefab;

    UnityEvent placementUpdate;

    [SerializeField]
    GameObject visualObject;
    public GameObject gameController;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI loadingText;
    private bool gotFirstPoint,gotSecondPoint;

    public GameObject env;
    public GameObject imageTracking;
    public GameObject mainCanvasUI;
    public GameObject canvasMain;
    public GameObject listScroll;
    public GameObject arrow;
    public GameObject arSessionOrigin;

    public bool planeDetectionEnabled;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    // public GameObject placedPrefab
    //     {
    //         get { return m_PlacedPrefab; }
    //         set { m_PlacedPrefab = value; }
    //     }

    //     /// <summary>
    //     /// The object instantiated as a result of a successful raycast intersection with a plane.
    //     /// </summary>
    //     public GameObject spawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            gotFirstPoint=false;
            gotSecondPoint=false;
            planeDetectionEnabled=false;
            visualObject.SetActive(false);

            if (placementUpdate == null)
                placementUpdate = new UnityEvent();

                //placementUpdate.AddListener(DisableVisual);
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            if(planeDetectionEnabled){
                if (!TryGetTouchPosition(out Vector2 touchPosition))
                    return;

                // if(!visualObject.activeSelf){
                //     loadingText.gameObject.SetActive(true);
                // }else loadingText.gameObject.SetActive(false);

                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
                    if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
                    {
                        // Raycast hits are sorted by distance, so the first one
                        // will be the closest hit.
                        var hitPose = s_Hits[0].pose;
                        if(!gotFirstPoint && !gotSecondPoint){
                            gameController.GetComponent<Main>().envPosition1=hitPose.position;
                            gotFirstPoint=true;
                            selectText.text="Select the second point";
                            print("first");
                        }else if(gotFirstPoint && !gotSecondPoint){
                            gameController.GetComponent<Main>().envPosition2=hitPose.position;
                            gotSecondPoint=true;
                            gameController.GetComponent<Main>().gotPositionDirection=true;
                            visualObject.SetActive(false);
                            selectText.gameObject.SetActive(false);
                            planeDetectionEnabled=false;
                            //GameObject.Find("AR Session Origin").GetComponent<PlaceOnPlane>().gameObject.SetActive(false);
                            arSessionOrigin.GetComponent<ARPointCloudManager>().pointCloudPrefab=default;
                            gameController.GetComponent<Main>().fromNow=Time.time;
                            gameController.GetComponent<Main>().isActive=true;
                            mainCanvasUI.GetComponent<MainCanvasUI>().envSet=true;
                            mainCanvasUI.GetComponent<MainCanvasUI>().escapeRoomButton.gameObject.SetActive(true);
                            mainCanvasUI.SetActive(false);
                            env.SetActive(true);
                            canvasMain.SetActive(true);
                            imageTracking.SetActive(false);
                            arrow.SetActive(true);
                            print("second");

                        }
                        placementUpdate.Invoke();
                    }
                }
            }
        }

    public void DisableVisual()
    {
        visualObject.SetActive(false);
    }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }