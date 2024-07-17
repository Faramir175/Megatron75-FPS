using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //public float moveSpeed;
    //public Rigidbody theRB;
    private bool chasing;
    public float distanceToChase = 10f,distanceToLose = 15f, distanceToStop = 2f;

    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;
    public float keepChasingTime=5f;
    private float chaseCoutner;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate,waitBetweenShots, timeToShoot=1f;
    private float fireCount,shotWaitCounter, shootTimeCounter;

    void Start()
    {
        startPoint = transform.position;
        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
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
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
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
            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;

                if(shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }
            }
            else
            { 
                if(PlayerController.instance.gameObject.activeInHierarchy) { 
                    shootTimeCounter -= Time.deltaTime;

                if (shootTimeCounter>0)
                {
                    fireCount -= Time.deltaTime;
                    if (fireCount <= 0)
                    {
                        fireCount = fireRate;

                        firePoint.LookAt(PlayerController.instance.transform.position + new Vector3 (0f,1.2f,0f));

                        //provera ugla igraca i bot-a

                         Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                        float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                        if(Mathf.Abs(angle) < 30f)
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation);
                        }
                        else
                        {
                            shotWaitCounter = waitBetweenShots;
                        }
                    }

                    agent.destination = transform.position;
                }else
                {
                    shotWaitCounter = waitBetweenShots;
                }
                }
            }
        }
    }
}
