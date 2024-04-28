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

    private void Awake()
    {
        mergeAble = true;
        
        initScale = transform.localScale;
        if(ballLevel == 11) SoundManager.instance.JesusCome();
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
