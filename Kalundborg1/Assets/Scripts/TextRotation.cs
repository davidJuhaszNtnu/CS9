using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro text;
    void Start()
    {
        text.transform.Rotate(0,0,0,Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        //text.transform.Rotate(0,1,0,Space.Self);
    }
}
