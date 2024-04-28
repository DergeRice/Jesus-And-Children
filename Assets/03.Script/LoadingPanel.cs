using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public Transform loadingIcon;

    // Update is called once per frame
    void Update()
    {
        loadingIcon.Rotate(0f, 0f, -1f);
    } 
}
