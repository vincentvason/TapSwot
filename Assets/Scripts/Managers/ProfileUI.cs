using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    /**
    * First time - Profile title -> Create your profile.
    *                            -> Only Save and Proceed.
    *
    * Second time - Profile title -> Edit your profile.
    *                             -> Save and Back.
    *
    */

    [SerializeField] private bool isCreated;
    [SerializeField] private TMP_Text ProfileTitle;
    [SerializeField] private GameObject Backbtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCreated == true)
        {
            ProfileTitle.text = "<font-weight=900>EDIT YOUR PROFILE";
            Backbtn.SetActive(true);
        }
        else
        {
            ProfileTitle.text = "<font-weight=900>CREATE YOUR PROFILE";
            Backbtn.SetActive(false);
        }
    }

    public void AlreadyCreate()
    {
        isCreated = true;
    }
}
