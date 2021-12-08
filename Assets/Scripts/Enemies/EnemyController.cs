using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeToGo;

    private int itinerator;
    private NavMeshAgent agent;
    private Vector3 nextPointToGo;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        itinerator = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(nextPointToGo, transform.position) <= 2f)
        {
            if (itinerator >= placeToGo.Length - 1)
            {
                itinerator = 0;
            }
            else
            {
                itinerator++;
            }
        }

        nextPointToGo = placeToGo[itinerator].transform.position;

    }

    private void FixedUpdate()
    {
        agent.SetDestination(nextPointToGo);
    }
}
