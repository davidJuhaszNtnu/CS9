using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UltimateTechWarningPanel : MonoBehaviour
{
    public GameObject mainPanel, ultimateTechWarningPanel;
    // public int index_showed;

    void Start()
    {
        
    }

    public void close_bttn(){
        mainPanel.SetActive(true);
        ultimateTechWarningPanel.SetActive(false);
    }
}
