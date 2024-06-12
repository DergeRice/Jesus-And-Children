using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int ballLevel;
    
    [HideInInspector] public bool mergeAble = true;
    public Sprite ballImg;
    public BallCollider ballCollider;

    [HideInInspector] public Vector3 initScale;

    PolygonCollider2D polygonCollider2D;

    public Ball target;

    public bool isSelected;

    private bool isAvailableSelect;

    private void Awake()
    {
        mergeAble = true;
        
        initScale = transform.localScale;
        if(ballLevel == 11)
        {
            SoundManager.instance.JesusCome();
            GameManager.instance.JesusCome();
        }

    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if(isAvailableSelect == false)
        {
            isSelected = false;
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("SpotLight"))
        {
            isAvailableSelect = true;
            isSelected = true;
            GetComponent<SpriteRenderer>().sortingOrder = 10;
        }else
        {
            isAvailableSelect = false;
            isSelected = false;
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }
    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        isAvailableSelect = false;
        isSelected = false;
        GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Ball") == true)
        {
            var otherBall = other.gameObject.GetComponent<Ball>();

            if(otherBall != null)
            {
                if(otherBall.ballLevel != ballLevel) return;
                
                if(otherBall.mergeAble == false) return;
                otherBall.mergeAble = true;
                MergeAction(otherBall);
            }
        }
    }


    public void MergeAction(Ball otherBall)
    {
        if(mergeAble == false) return;
        var ballPos = (transform.position + otherBall.transform.position)/2;
        
        GameManager.instance.BallMerge(this,otherBall,ballPos,target);
    }


}
