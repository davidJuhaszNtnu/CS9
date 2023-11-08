using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class HeaderButtons : MonoBehaviour
{
    public CanvasGroup tab1;
    public CanvasGroup tab2;
    public CanvasGroup tab3;

    public GameObject canvasToClose;
    public ClickObject closeCanvas;

    public GameObject videoPlayer;
    private bool videoPlayerActive;

    void Start()
    {
        videoPlayer.SetActive(false);
        videoPlayerActive = false;
    }

    
    public void close_canvas()
    {
        canvasToClose.SetActive(false);
        closeCanvas.isCanvasOpen = false;
    }

    public void changeTab(CanvasGroup changeTo, bool show)
    {
        if(show)
        {
            changeTo.alpha = 1;
            changeTo.interactable = true;
            changeTo.blocksRaycasts = true;
            if(changeTo != tab3 && videoPlayerActive)
            {
                videoPlayer.SetActive(false);
            }
            return;
        }
        changeTo.alpha = 0;
        changeTo.interactable = false;
        changeTo.blocksRaycasts = false;
    }

    public void tab1Press()
    {
        changeTab(tab1, true);
        changeTab(tab2, false);
        changeTab(tab3, false);
    }
    public void tab2Press()
    {
        changeTab(tab1, false);
        changeTab(tab2, true);
        changeTab(tab3, false);
    }
    public void tab3Press()
    {
        changeTab(tab1, false);
        changeTab(tab2, false);
        changeTab(tab3, true);
        videoPlayerActive = true;
        videoPlayer.SetActive(true);
    }

}
