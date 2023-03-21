using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
    public TextMeshProUGUI myMessage;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().SetAsLastSibling();
    }

   
}
