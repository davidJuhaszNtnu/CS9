using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    //Script for the Exit Logo button to close the panels and the river object
    public GameObject canvasToClose;
    public GameObject river;
    
    public void close_canvas()
    {
        canvasToClose.SetActive(false);
        river.SetActive(false);
    }
}
