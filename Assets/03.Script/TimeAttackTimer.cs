using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TimeAttackTimer : MonoBehaviour
{
    public TMP_Text timeText;
    public float remainTime = 300f;


    public Image clockIcon; 

    public bool isGameStart = false;
    public void TimeAttackStart()
    {
        //remainTime -= Time.deltaTime;
        isGameStart = true;
    }

    private void Update()
    {
        if(isGameStart == true)  remainTime -= Time.deltaTime;

        clockIcon.fillAmount = (300 - remainTime )/ 300;
        timeText.text = remainTime.ToString("F1");

        if (remainTime <= 5)
        {
            remainTime = 5;
            isGameStart = false;

            FindFirstObjectByType<MaxLine>().timeOver = true;
        }
    }
}
