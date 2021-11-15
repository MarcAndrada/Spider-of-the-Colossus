using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;



public class SpiderController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float smoothness;
    [SerializeField]
    private int raysNb;
    [SerializeField]
    private float raysEccentricity;
    [SerializeField]
    private float outerRaysOffset;
    [SerializeField]
    private float innerRaysOffset;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float rotSmooth;
    [SerializeField]
    private bool onAir;
    [SerializeField]
    private float raySurfaceCheckDist;
    [SerializeField]
    private GameObject spiderObj;
    [SerializeField]
    private Transform[] legsPos;
    [SerializeField]
    LayerMask scalableMask;


    private float valueY;
    private float valueX;
    [SerializeField]
    private bool canCheckFloor;
    
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Vector3 forward;
    private Vector3 upward;
    private Quaternion lastRot;
    private Vector3[] pn;


    private SpiderProceduralAnimation spiderAnimCont;


 /* 
    Vector3[] GetIcoSphereCoords(int depth)
    {
        Vector3[] res = new Vector3[(int)Mathf.Pow(4, depth) * 12];
        float t = (1f + Mathf.Sqrt(5f)) / 2f;
        res[0] = (new Vector3(t, 1, 0));
        res[1] = (new Vector3(-t, -1, 0));
        res[2] = (new Vector3(-1, 0, t));
        res[3] = (new Vector3(0, -t, 1));
        res[4] = (new Vector3(-t, 1, 0));
        res[5] = (new Vector3(1, 0, t));
        res[6] = (new Vector3(-1, 0, -t));
        res[7] = (new Vector3(0, t, -1));
        res[8] = (new Vector3(t, -1, 0));
        res[9] = (new Vector3(1, 0, -t));
        res[10] = (new Vector3(0, t, 1));
        res[11] =(new Vector3(0, -t, -1));

        return res;
    }
 
    
    Vector3[] GetClosestPointIco(Vector3 point, Vector3 up, float halfRange)
    {
        Vector3[] res = new Vector3[2] { point, up };

        Vector3[] dirs = GetIcoSphereCoords(0);
        raysNb = dirs.Length;

        float amount = 1f;

        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Ray ray = new Ray(point + up*0.15f, dir);
            //Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                amount += 1;
            }
        }
        res[0] /= amount;
        res[1] /= amount;
        return res;
    }
 */

    static Vector3[] GetClosestPoint(Vector3 point, Vector3 forward, Vector3 up, float halfRange, float eccentricity, float offset1, float offset2, int rayAmount)
    {
        Vector3[] res = new Vector3[2] { point, up };
        Vector3 right = Vector3.Cross(up, forward);
        float normalAmount = 1f;
        float positionAmount = 1f;

        Vector3[] dirs = new Vector3[rayAmount];
        float angularStep = 2f * Mathf.PI / (float)rayAmount;
        float currentAngle = angularStep / 2f;
        for(int i = 0; i < rayAmount; ++i)
        {
            dirs[i] = -up + (right * Mathf.Cos(currentAngle) + forward * Mathf.Sin(currentAngle)) * eccentricity;
            currentAngle += angularStep;
        }

        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Vector3 largener = Vector3.ProjectOnPlane(dir, up);
            Ray ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset1 / 100f, dir);
            //Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount += 1;
                positionAmount += 1;
            }
            ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset2 / 100f, dir);
            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount   += 1;
                positionAmount += 1;
            }
        }
        res[0] /= positionAmount;
        res[1] /= normalAmount;
        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3();
        forward = transform.forward;
        upward = transform.up;
        lastRot = transform.rotation;
        onAir = false;
        canCheckFloor = true;
        spiderAnimCont = GetComponentInChildren<SpiderProceduralAnimation>();
    }

    private void Update()
    {
        valueY = Input.GetAxis("Vertical");
        valueX = Input.GetAxis("Horizontal");

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!onAir)
        {


            velocity = (smoothness * velocity + (transform.position - lastPosition)) / (1f + smoothness);
            if (velocity.magnitude < 0.00025f)
            {
                velocity = lastVelocity;
            }
            lastPosition = transform.position;
            lastVelocity = velocity;


            float multiplier = 1f;
            float rotMultiplier = 1f;

            /*if (Input.GetKey(KeyCode.LeftShift))
            {
                multiplier = 2f;
            }
            */
            if (valueY != 0)
            {
                transform.position += transform.forward * valueY * _speed * multiplier * Time.fixedDeltaTime;
                rotMultiplier = 2f;
            }
            else
            {
                rotMultiplier = 1f;
            }

            if (valueX != 0)
            {
                transform.position += Vector3.Cross(transform.up, transform.forward) * valueX * (_speed / 2) * multiplier * Time.fixedDeltaTime;
            }

            if (valueX != 0 || valueY != 0)
            {
                pn = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, 0.1f, 30, -30, 5);

                upward = pn[1];

                Vector3[] pos = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, raysEccentricity, innerRaysOffset, outerRaysOffset, raysNb);
                transform.position = Vector3.Lerp(lastPosition, pos[0], 1f / (0.5f + smoothness));
                if (valueY == -1)
                {
                    forward = -velocity.normalized;
                }
                else
                {
                    forward = velocity.normalized;
                }
                Quaternion rot = Quaternion.LookRotation(forward, upward);
                transform.rotation = Quaternion.Lerp(lastRot, rot, (rotSpeed * rotMultiplier) / (rotSmooth + smoothness));
            }

            lastRot = transform.rotation;

            Debug.DrawRay(spiderObj.transform.position, -spiderObj.transform.up * (raySurfaceCheckDist * 1.5f), Color.green);
            if (!Physics.Raycast(spiderObj.transform.position, -spiderObj.transform.up, raySurfaceCheckDist * 1.5f, scalableMask))
            {
                Debug.DrawRay(legsPos[0].transform.position, -spiderObj.transform.up * (raySurfaceCheckDist), Color.white);
                Debug.DrawRay(legsPos[1].transform.position, -spiderObj.transform.up * (raySurfaceCheckDist), Color.white);
                Debug.DrawRay(legsPos[2].transform.position, -spiderObj.transform.up * (raySurfaceCheckDist), Color.white);
                Debug.DrawRay(legsPos[3].transform.position, -spiderObj.transform.up * (raySurfaceCheckDist), Color.white);

                if (!Physics.Raycast(legsPos[0].transform.position, -spiderObj.transform.up, raySurfaceCheckDist, scalableMask) ||
                    !Physics.Raycast(legsPos[1].transform.position, -spiderObj.transform.up, raySurfaceCheckDist ,scalableMask) ||
                    !Physics.Raycast(legsPos[2].transform.position, -spiderObj.transform.up, raySurfaceCheckDist ,scalableMask) ||
                    !Physics.Raycast(legsPos[3].transform.position, -spiderObj.transform.up, raySurfaceCheckDist ,scalableMask))
                {
                    //setOnAir();
                }
                
            }
        }
        else
        {
            if (canCheckFloor)
            {

                Vector3 rayStartPos = new Vector3(spiderObj.transform.position.x, spiderObj.transform.position.y + 0.1f, spiderObj.transform.position.z);
                if (Physics.Raycast(rayStartPos, -spiderObj.transform.up, raySurfaceCheckDist, scalableMask)){
                    setOnFloor();

                }
                else
                {
                    if (Physics.Raycast(rayStartPos, spiderObj.transform.up,raySurfaceCheckDist, scalableMask) ||
                    Physics.Raycast(rayStartPos, spiderObj.transform.right, raySurfaceCheckDist, scalableMask) ||
                    Physics.Raycast(rayStartPos, -spiderObj.transform.right, raySurfaceCheckDist, scalableMask))
                    {
                        setOnFloor();
                    }
                }

                Debug.DrawRay(rayStartPos, spiderObj.transform.up * raySurfaceCheckDist, Color.red);
                Debug.DrawRay(rayStartPos, -spiderObj.transform.up * raySurfaceCheckDist, Color.red);
                Debug.DrawRay(rayStartPos, spiderObj.transform.right * raySurfaceCheckDist, Color.red);
                Debug.DrawRay(rayStartPos, -spiderObj.transform.right * raySurfaceCheckDist, Color.red);

            }


        }

    }



    private void setOnAir() 
    {
        Vector3 rayStartPos = new Vector3(spiderObj.transform.position.x, spiderObj.transform.position.y + 0.1f, spiderObj.transform.position.z);
        onAir = true;
        spiderAnimCont.SetStarterPosBeforeAir();   
    }

    private void setOnFloor() 
    {
        Vector3 rayStartPos = new Vector3(spiderObj.transform.position.x, spiderObj.transform.position.y + 0.1f, spiderObj.transform.position.z);
        onAir = false;
        Debug.DrawRay(rayStartPos, -spiderObj.transform.up * raySurfaceCheckDist, Color.green);

    }

    public bool isOnAir()
    {
        return onAir;
    }

}
