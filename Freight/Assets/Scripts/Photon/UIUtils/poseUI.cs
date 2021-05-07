using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class poseUI : MonoBehaviour
{
    public GameObject text;
    public Image image;

    public void OnMouseOver() {
        Debug.Log("MOUSE OVER POSE");
        transform.parent.parent.parent.GetComponent<AudioSource>().Play();
        TextMeshProUGUI TextMeshPros = text.GetComponent<TextMeshProUGUI>();
        image.enabled = true;
        TextMeshPros.color  = new Color32(0, 0, 0, 255);
    }

    public void OnMouseExit() {
         TextMeshProUGUI TextMeshPros = text.GetComponent<TextMeshProUGUI>();
         TextMeshPros.color = new Color32(151, 158, 169, 255);
         image.enabled = false;
    }
    
    public void onClick()
    {
        Debug.Log("turn off pose");
        PoseParser.turnOffPose();
    }
}