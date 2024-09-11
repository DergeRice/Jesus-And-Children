using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Ball : MonoBehaviour
{
    public int ballLevel;
    public bool mergeAlbeObject;
    public TMP_Text text;
    
    [HideInInspector] public bool mergeAble = true;
    public Sprite ballImg;
    public BallCollider ballCollider;

    [HideInInspector] public Vector3 initScale;

    PolygonCollider2D polygonCollider2D;

    public Ball target;

    public bool isSelected;

    private bool isAvailableSelect;

    private bool clearEnemyPossible;

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

    public void CanClearEnemy()
    {
        clearEnemyPossible = true;
        Invoke(nameof(CanClearEnemyReturn),0.8f);
    }
    public void CanClearEnemyReturn()
    {
        clearEnemyPossible = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Ball") == true)
        {
            var otherBall = other.gameObject.GetComponent<Ball>();

            // if(clearEnemyPossible && otherBall.ballLevel > 30)
            // {
            //     other.transform.DOScale(Vector3.zero,1f);

            //     // otherBall.GetComponent<Collider2D>().enabled = false;
            //     otherBall.GetComponent<Rigidbody2D>().gravityScale = 0;
            //     // otherBall.transform.DOLocalRotate(otherBall.transform.rotation.eulerAngles+(Vector3.back*800),1f,RotateMode.FastBeyond360);

            //     Destroy(other.gameObject,1f);
            // }

            if(otherBall != null)
            {
                if(mergeAlbeObject == false) return;
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
