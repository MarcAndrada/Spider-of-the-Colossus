using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeToGo;
    [SerializeField]
    private int[] placeToStop;
    [SerializeField]
    private float distanceToReach;
    [SerializeField]
    private float timeToWaitStoped;
    [SerializeField]
    private float Distance;

    private int itinerator;
    private bool stopMove;
    private float timeStoped = 0;
    private bool[] stoppedHere;

    private NavMeshAgent agent;
    private Vector3 nextPointToGo;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        itinerator = 0;
        stoppedHere = new bool[placeToStop.Length];
        for (int i = 0; i < placeToStop.Length; i++)
        {
            stoppedHere[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        Distance = Vector3.Distance(nextPointToGo, transform.position);
        SetNewPosMove();
        FreezeTimer();

    }

    private void FixedUpdate()
    {
        CheckIfCanMovee();

    }


    private void SetNewPosMove() {

        //Establecer la nueva posicion a la que ir
        //Y en caso de que no se mueva contar los segundos que estara quieto
        //Revisamos si la distancia al punto es lo suficientemente corta para poder ir hacia el otro
        if (Vector3.Distance(nextPointToGo, transform.position) <= distanceToReach)
        {
            //Revisamos si el punto en el que esta es el ultimo, en ese caso lo reiniciamos todo
            if (itinerator >= placeToGo.Length - 1)
            {
                for (int i = 0; i < stoppedHere.Length; i++)
                {
                    stoppedHere[i] = false;
                }

                itinerator = 0;

            }
            else if (!stopMove)
            {
                //Aumentamos el accumulador 

                itinerator++;

            }

            for (int i = 0; i < placeToStop.Length; i++)
            {
                if (itinerator == placeToStop[i] && !stoppedHere[i])
                {
                    stopMove = true;
                    stoppedHere[i] = true;
                }
            }

        }



        nextPointToGo = placeToGo[itinerator].transform.position;
    }

    private void FreezeTimer() {
        if (stopMove)
        {
            timeStoped += Time.deltaTime;
        }

        //Timer para saber cuando ha de volver a mover
        if (timeStoped >= timeToWaitStoped)
        {
            stopMove = false;
            timeStoped = 0;
            agent.isStopped = false;
        }
    }

    private void CheckIfCanMovee() {
        if (!stopMove)
        {
            agent.isStopped = false;
            agent.SetDestination(nextPointToGo);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
