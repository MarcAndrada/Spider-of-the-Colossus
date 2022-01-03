using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeToGo;
    [SerializeField]
    private int[] placeToStop;
    [SerializeField]
    private float timeToWaitStoped;
    [SerializeField]
    private AudioSource focusedAudioSource;
    [SerializeField]
    private AudioClip focusSound;
    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private int itinerator;
    private bool checkingZone;
    private float timeStoped = 0;
    private bool[] stoppedHere;
    private bool whatchingPlayer = false;

    private NavMeshAgent agent;
    private Vector3 nextPointToGo;
    private TriggerSensor sensorCone;
    private GameObject Player;
    private SpiderStateController spiderCont;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sensorCone = GetComponentInChildren<TriggerSensor>();
        Player = GameObject.FindGameObjectWithTag("Player");
        spiderCont = Player.GetComponent<SpiderStateController>();
        itinerator = 0;
        stoppedHere = new bool[placeToStop.Length];
        for (int i = 0; i < placeToStop.Length; i++)
        {
            stoppedHere[i] = false;
        }
        //Debug.Log("EMPIEZA EL PARTIDO");
        nextPointToGo = placeToGo[0].transform.position;
        agent.SetDestination(nextPointToGo);
    }

    // Update is called once per frame
    void Update()
    {
        SetNewPosMove();
        CheckIfSeeEnemy();
        CheckIfCanMove();
        FreezeTimer();
        //Debug.Log("ol gud");


    }


    private void SetNewPosMove() {

        //Establecer la nueva posicion a la que ir
        //Y en caso de que no se mueva contar los segundos que estara quieto
        //Revisamos si la distancia al punto es lo suficientemente corta para poder ir hacia el otro


        if (!agent.hasPath)
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
            else if (!checkingZone)
            {
                //Aumentamos el accumulador 
                
                itinerator++;
                nextPointToGo = placeToGo[itinerator].transform.position;
                agent.SetDestination(nextPointToGo);

                if (debug)
                {
                    Debug.Log("Ahora toca el " + itinerator + placeToGo[itinerator].transform.position);
                }

            }

            for (int i = 0; i < placeToStop.Length; i++)
            {
                if (itinerator == placeToStop[i] && !stoppedHere[i])
                {
                    checkingZone = true;
                    stoppedHere[i] = true;
                }
            }

        }

        nextPointToGo = placeToGo[itinerator].transform.position;


    }

    private void FreezeTimer() {
        if (checkingZone)
        {
            timeStoped += Time.deltaTime;
        }

        //Timer para saber cuando ha de volver a mover
        if (timeStoped >= timeToWaitStoped)
        {
            checkingZone = false;
            timeStoped = 0;
            agent.isStopped = false;
        }
    }

    private void CheckIfCanMove() {
        if (!checkingZone && !whatchingPlayer)
        {
            agent.isStopped = false;
            agent.SetDestination(nextPointToGo);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void CheckIfSeeEnemy(){


        if (sensorCone.GetDetectedByComponent<SpiderStateController>().Contains(spiderCont) && !spiderCont.IsInvisible())
        {
            spiderCont.IsSeen();
            //Debug.Log("Me Ve UWU");
            whatchingPlayer = true;
            if (!focusedAudioSource.isPlaying)
            {
                focusedAudioSource.PlayOneShot(focusSound);
            }


        }
        else if (sensorCone.GetDetectedByComponent<SpiderStateController>().Contains(spiderCont) && spiderCont.IsInvisible())
        {
            //Debug.Log("Me Ve Pero Soy INVISIBLE UWU");
            whatchingPlayer = false;
            focusedAudioSource.Stop();
        }
        else
        {
            //Debug.Log("Esto no chufla");

        }
    }

    public void SpiderOutOfVision()
    {
        spiderCont.IsntSeen();
        //Debug.Log("Ya no me ve :)");
        whatchingPlayer = false;
        focusedAudioSource.Stop();


    }

}
