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

    MaxLine maxLine;

    private void Start()
    {
        maxLine = FindFirstObjectByType<MaxLine>();
    }
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

        if (remainTime <= 5 && maxLine.gameOverTimeTrigger == false && maxLine.isGameOver == false)
        {
            maxLine.gameOverTimeTrigger = true;
        }

        if(remainTime <= 0)
        {
            remainTime = 0;
            isGameStart = false;
        }
    }
}
