using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProps : MonoBehaviour
{
    public float t;
    public bool forward;
    
    private Material material;
    private Color A, B;

    void Start()
    {
        t = 0f;
        forward = true;
        material = GetComponent<Renderer>().material;
        A = new Color(1f, 1f, 1f, 1f);
        B = material.color;
    }

    public void flash_color(){
        Color c = new Color(A.r + (B.r-A.r) * t, A.g + (B.g-A.g) * t, A.b + (B.b-A.b) * t, 1f);
        GetComponent<Renderer>().material.color = c;
    }

    public void apply_material(Material mat){
        GetComponent<Renderer>().material = mat;
    }

    public void restart_material(){
        GetComponent<Renderer>().material = material;
    }

}
