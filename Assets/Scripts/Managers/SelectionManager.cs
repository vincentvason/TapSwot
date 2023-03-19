using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private string menuSelection=string.Empty;

    public static SelectionManager instance;    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    public void SetPlayerSelection(string s)
    {
        menuSelection = s;  
    }
    public string GetPlayerSelection()
    {
        return menuSelection;
    }
}
