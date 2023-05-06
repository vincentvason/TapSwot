using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageObjective : MonoBehaviour
{
    public RectTransform stage1objective;
    private bool stage1UIvisible=false;



    public void OnSelectStageOne()
    {
        if (!stage1UIvisible)
        {
            
            stage1objective.DOAnchorPos(new Vector2(750, 390), 0.5f);

            stage1UIvisible = true;
        }
        else
        {
            stage1objective.DOAnchorPos(new Vector2(750, 630), 0.5f);
            stage1UIvisible = false;
        }
    }


}
