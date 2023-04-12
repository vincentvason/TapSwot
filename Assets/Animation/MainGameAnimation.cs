using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainGameAnimation : MonoBehaviour
{
    [SerializeField] SceneTransition menuTransition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(menuTransition.SceneTransitionEnd());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
