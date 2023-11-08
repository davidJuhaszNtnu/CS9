using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class App : MonoBehaviour
{
    public GameObject mainCanvasUI;
    public GameObject env;
    public GameObject canvasMain;
    public GameObject arrow;
    public GameObject arSessionOrigin;

    void Start()
    {
        mainCanvasUI.SetActive(true);
        env.SetActive(false);
        canvasMain.SetActive(false);
        arrow.SetActive(false);
        arSessionOrigin.GetComponent<ARPointCloudManager>().pointCloudPrefab=default;
    }
}
