using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject shootpoint,enemyObject;

    public List<string> hardshipTexts;

    public void Shoot()
    {
        // transform.position
        var shootObj =  Instantiate(enemyObject,shootpoint.transform.position,quaternion.identity);
        var forceVector =  shootpoint.transform.position -transform.position ;
        shootObj.GetComponent<Rigidbody2D>().AddForce(forceVector.normalized*15f,ForceMode2D.Impulse);
        shootObj.GetComponent<EnemyBall>().hardshipText.text = hardshipTexts[Random.Range(0,hardshipTexts.Count)];
        shootObj.transform.parent = GameManager.instance.ballParent.transform;
    }
}
