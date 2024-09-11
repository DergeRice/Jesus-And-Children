using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopEnemy : MonoBehaviour
{
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ball") == true)
        {
            var otherBall = other.gameObject.GetComponent<Ball>();

            if( otherBall.ballLevel > 30)
            {
                other.transform.DOScale(Vector3.zero,1f);

                // otherBall.GetComponent<Collider2D>().enabled = false;
                otherBall.GetComponent<Rigidbody2D>().gravityScale = 0;
                // otherBall.transform.DOLocalRotate(otherBall.transform.rotation.eulerAngles+(Vector3.back*800),1f,RotateMode.FastBeyond360);

                Destroy(other.gameObject,1f);
            }
    }
}
}
