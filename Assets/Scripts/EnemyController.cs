using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    //public Rigidbody theRB;
    private bool chasing;
    public float distanceToChase = 10f,distanceToLose = 15f, distanceToStop = 2f;

    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;
    public float keepChasingTime=5f;
    private float chaseCoutner;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing) 
        {
            if(Vector3.Distance(transform.position,targetPoint) < distanceToChase) 
            {
                chasing= true;
            }

            if(chaseCoutner>0) 
            { 
                chaseCoutner -= Time.deltaTime;

                if(chaseCoutner <=0) 
                {
                    agent.destination = startPoint;
                }
            }
        }
        else
        {
            //transform.LookAt(targetPoint);
            //theRB.velocity = transform.forward * moveSpeed;

            if(Vector3.Distance(transform.position,targetPoint)>distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }

            if(Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCoutner = keepChasingTime;
            }
        }
    }
}
