using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerWonPanel : MonoBehaviour
{
    public GameObject playerWonPanel, mainPanel, appPanel, infographics, gameController, app, environment;
    void Start()
    {
        
    }

    public void close_bttn(){
        // here comes ending the game
        mainPanel.SetActive(true);
        playerWonPanel.SetActive(false);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
    }

    public void replay_bttn(){
        mainPanel.SetActive(true);
        playerWonPanel.SetActive(false);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        gameController.GetComponent<gameController>().restart();

    }

    public void exit_bttn(){
        mainPanel.SetActive(false);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
        app.GetComponent<App>().first_time_playing = false;
        gameController.GetComponent<gameController>().tutorialOn = false;
        // gameController.GetComponent<gameController>().restart();
        appPanel.SetActive(true);
        infographics.SetActive(true);
        app.GetComponent<App>().gotFirst = false;
        app.GetComponent<App>().gotSecond = false;
        app.GetComponent<App>().gotBoth = false;
        // app.GetComponent<App>().gotBoth = true;
        // gameController.SetActive(false);
        if (environment.GetComponent<ARAnchor>() != null)
            Destroy(environment.GetComponent<ARAnchor>());
        environment.SetActive(false);
        playerWonPanel.SetActive(false);
    }
}
