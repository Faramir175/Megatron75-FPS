using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float moveSpeed,lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 1;

    public bool damageEnemy;
    public bool damagePlayer;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;

        lifeTime-=Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy) 
        { 
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage); 
        }
        if (other.gameObject.tag == "Player" && damagePlayer) 
        {
            PlayerHealthController.instance.damagePlayer(damage);
        }
        Destroy(gameObject);
        Instantiate(impactEffect,transform.position+(transform.forward*(-moveSpeed*Time.deltaTime)),transform.rotation);
    }
}
