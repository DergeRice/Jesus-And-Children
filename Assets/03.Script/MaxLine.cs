using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxLine : MonoBehaviour
{
    public float maxTime;

    public float curTime;
    public int floorTime;

    public List<GameObject> triggers = new List<GameObject>();

    public bool isGameOver = false;
    public bool gameOverTimeTrigger = false;

    private void Start()
    {
        curTime = maxTime;
    }
    private void Update()
    {
        if(triggers.Count > 0 || gameOverTimeTrigger == true)
        {
            floorTime = (int)curTime;
            curTime -= Time.deltaTime;
            if(0 > curTime && isGameOver == false)
            {
                GameManager.instance.GameOver();
                gameOverTimeTrigger = false;
                isGameOver = true;
            }
                

            if(maxTime - 1 > curTime)
            {
                CanvasManager.instance.dangerCount.gameObject.SetActive(true);
                CanvasManager.instance.dangerCount.text = ((int)curTime).ToString();
            }

            if(floorTime != 5 &&(int)curTime != floorTime)
            {
                SoundManager.instance.Ticking((int)curTime != floorTime);
            }
        }else
        {
            curTime = maxTime;
            CanvasManager.instance.dangerCount.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball") == true)
        {
            triggers.Add(other.gameObject);
        } 
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.CompareTag("Ball") == true && triggers.Contains(other.gameObject) == false)
        {
            triggers.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ball") == true)
        {
            triggers.Remove(other.gameObject);
        }
    }
}
