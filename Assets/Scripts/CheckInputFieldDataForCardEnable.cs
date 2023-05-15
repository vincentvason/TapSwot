using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckInputFieldDataForCardEnable : MonoBehaviour
{
    private Button myButton;
    public TMPro.TMP_InputField title;
    public TMPro.TMP_InputField subtitle;
    public TMPro.TMP_InputField desc;

    private void Start()
    {
        myButton = GetComponent<Button>();
    }

    private void Update()
    {
        string s1 = title.text;
        string s2 = subtitle.text;
        string s3 = desc.text;

        if(s1.Length > 0)
        {
            myButton.interactable = true;
        }
        else
        {
            myButton.interactable = false;
        }
    }


}
