using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    //-----------------infographics----------------------------
    //refinery
    public AudioSource text31, text32, text33;
    public void text31_bttn(){
        text31.Play();
    }
    public void text32_bttn(){
        text32.Play();
    }
    public void text33_bttn(){
        text33.Play();
    }
    //utility
    public AudioSource text34;
    public void text34_bttn(){
        text34.Play();
    }
    //chemical
    public AudioSource text35, text36, text37;
    public void text35_bttn(){
        text35.Play();
    }
    public void text36_bttn(){
        text36.Play();
    }
    public void text37_bttn(){
        text37.Play();
    }
    //food
    public AudioSource text38, text39, text40;
    public void text38_bttn(){
        text38.Play();
    }
    public void text39_bttn(){
        text39.Play();
    }
    public void text40_bttn(){
        text40.Play();
    }
    //farm
    public AudioSource text41, text42, text43;
    public void text41_bttn(){
        text41.Play();
    }
    public void text42_bttn(){
        text42.Play();
    }
    public void text43_bttn(){
        text43.Play();
    }
    //city
    public AudioSource text44;
    public void text44_bttn(){
        text44.Play();
    }
    //parameter game
    public AudioSource text46;
    public void text46_bttn(){
        text46.Play();
    }
    //warning panel
    public AudioSource text45;
    public void text45_bttn(){
        text45.Play();
    }
    //welcome panel
    public AudioSource text1;
    public void text1_bttn(){
        text1.Play();
    }

    //----------------------------------escape room-----------------------------
    //add new industry panel
    public GameObject centerToCompare;
    public AudioSource text2, text3, text4;
    public void text2_3_4_bttn(){
        if(centerToCompare.GetComponent<SnapToCenter>().indexMin == 0){
            text2.Play();
            text3.Stop();
            text4.Stop();
        }
        if(centerToCompare.GetComponent<SnapToCenter>().indexMin == 1){
            text3.Play();
            text2.Stop();
            text4.Stop();
        }
        if(centerToCompare.GetComponent<SnapToCenter>().indexMin == 2){
            text4.Play();
            text2.Stop();
            text3.Stop();
        }
    }
    //calibration panel
    public AudioSource text5;
    public void text5_bttn(){
        text5.Play();
    }
    //connection warning panel
    public GameObject connectionWarningPanel;
    public AudioSource text6, text7, text8;
    public void text6_7_8_bttn(){
        if(connectionWarningPanel.GetComponent<ConnectionWarningPanel>().warning_text.text == "There is not enough waste water to exchange")
            text7.Play();
        if(connectionWarningPanel.GetComponent<ConnectionWarningPanel>().warning_text.text == "This connection can't be modified. Try to remove or modify some of the outgoing connections of the receiver first.")
            text6.Play();
        if(connectionWarningPanel.GetComponent<ConnectionWarningPanel>().warning_text.text == "There is already a connection between these industries")
            text8.Play();
    }
    //infocards panel
    public GameObject gameController;
    public AudioSource text9, text10, text11, text12, text2_2, text3_2, text4_2, text26_2;
    public void text9_10_11_12_bttn(){
        if(gameController.GetComponent<gameController>().selected_index == 0)
            text26_2.Play();
        if(gameController.GetComponent<gameController>().selected_index == 1)
            text9.Play();
        if(gameController.GetComponent<gameController>().selected_index == 2)
            text10.Play();
        if(gameController.GetComponent<gameController>().selected_index == 3)
            text11.Play();
        if(gameController.GetComponent<gameController>().selected_index == 4)
            text12.Play();
        if(gameController.GetComponent<gameController>().selected_index == 5)
            text2_2.Play();
        if(gameController.GetComponent<gameController>().selected_index == 6)
            text3_2.Play();
        if(gameController.GetComponent<gameController>().selected_index == 7)
            text4_2.Play();
    }
    //new connection animation panel
    public AudioSource text13;
    public void text13_bttn(){
        text13.Play();
    }
    //player won panel
    public AudioSource text14;
    public void text14_bttn(){
        text14.Play();
    }
    //question main panel
    public AudioSource text15;
    public void text15_bttn(){
        text15.Play();
    }
    //question infocard panel
    public AudioSource text16;
    public void text16_bttn(){
        text16.Play();
    }
    //question add new industry panel
    public AudioSource text17;
    public void text17_bttn(){
        text17.Play();
    }
    //question placement new industry panel
    public AudioSource text20;
    public void text20_bttn(){
        text20.Play();
    }
    //question choose industry panel
    public AudioSource text21;
    public void text21_bttn(){
        text21.Play();
    }
    //question connection panel
    public AudioSource text22;
    public void text22_bttn(){
        text22.Play();
    }
    //question new connection panel
    public AudioSource text23;
    public void text23_bttn(){
        text23.Play();
    }

    //-------------------------tutorial--------------------
    //add new industry panel
    public AudioSource text18;
    public void text18_bttn(){
        text18.Play();
    }
    //make connection panel
    public AudioSource text19;
    public void text19_bttn(){
        text19.Play();
    }
    //welcome panel
    public AudioSource text24;
    public void text24_bttn(){
        text24.Play();
    }
    //welcome panel 2
    public AudioSource text25;
    public void text25_bttn(){
        text25.Play();
    }
    //distribution industry panel
    public AudioSource text26;
    public void text26_bttn(){
        text26.Play();
    }
    //industry panel
    public AudioSource text27;
    public void text27_bttn(){
        text27.Play();
    }
    //industry panel 2
    public AudioSource text28;
    public void text28_bttn(){
        text28.Play();
    }
    //how to play panel
    public AudioSource text15_2;
    public void text15_2_bttn(){
        text15_2.Play();
    }
    //make connection panel
    public AudioSource text29;
    public void text29_bttn(){
        text29.Play();
    }
    //connection animation panel
    public AudioSource text30;
    public void text30_bttn(){
        text30.Play();
    }

}
