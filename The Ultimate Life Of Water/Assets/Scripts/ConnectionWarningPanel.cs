using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectionWarningPanel : MonoBehaviour
{
    public GameObject chooseIndustriesPanel, gameController, connectionWarningPanel;
    public TextMeshProUGUI warning_text;
    
    void Start()
    {
        
    }

    public void close_bttn(){
        chooseIndustriesPanel.GetComponent<ChooseIndustriesPanel>().back();

        gameController.GetComponent<gameController>().mainPanel.SetActive(true);
        chooseIndustriesPanel.SetActive(false);
        connectionWarningPanel.SetActive(false);
        gameController.GetComponent<gameController>().allowed_to_view_info = true;
    }

    public void set_warning(string text){
        warning_text.text = text;
    }
}
