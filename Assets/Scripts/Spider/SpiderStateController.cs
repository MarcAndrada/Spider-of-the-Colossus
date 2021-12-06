using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderStateController : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField]
    Slider invisivilityBar;
    [SerializeField]
    GameObject alertBar;
    [SerializeField]
    private float invisibleConsumeSpeed;
    [SerializeField]
    private float invisibleReloadSpeed;
    [SerializeField]
    private float warnSpeed;
    [SerializeField]
    private float unwarnedSpeed;
    [SerializeField]
    private SkinnedMeshRenderer SpiderModel;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material invisibleMaterial;


    private bool canMove = true;
    private bool isInvisible = false;
    private float currentInvisibleTime = 1;
    private bool isObserved;
    private float warnLevel;
    private float timeToWaitAlertDisapear = 1.5f;
    private float timeWaitedAlertDisapear = 0;

    private SpiderController spiderMove;
    private Slider alertBarSlider;
    // Start is called before the first frame update
    void Start()
    {
        spiderMove = GetComponent<SpiderController>();
        alertBarSlider = alertBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvisibleControls();
        CheckIfInvisible();
        CheckMovment();
        CheckIfObserved();
        SetSliderHudValues();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isObserved = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isObserved = false;
        }
    }

    private void TurnInvisible() {
        //Volverse Invisible y bloquear movimiento
        canMove = false;
        isInvisible = true;
        SpiderModel.material = invisibleMaterial;

    }

    private void TurnVisible() {
        //Volverse visible y desbloquear movimiento
        canMove = true;
        isInvisible = false;
        SpiderModel.material = normalMaterial;
    }

    private void CheckInvisibleControls() {
        if (Input.GetButtonDown("Invisibility"))
        {
            TurnInvisible();
        }

        if (Input.GetButtonUp("Invisibility"))
        {
            TurnVisible();
        }
    }

    private void CheckIfInvisible() {
        if (isInvisible)
        {
            currentInvisibleTime -= invisibleConsumeSpeed / 100 * Time.deltaTime;
            if (currentInvisibleTime <= 0)
            {
                currentInvisibleTime = 0;
                TurnVisible();

            }
        }
        else if (currentInvisibleTime < 1)
        {
            currentInvisibleTime += invisibleReloadSpeed / 100 * Time.deltaTime;
        }
        else 
        {
            currentInvisibleTime = 1;
        }


    }

    public void SetSliderHudValues() {
        invisivilityBar.value = currentInvisibleTime;
        alertBarSlider.value = warnLevel;
    }

    private void CheckIfObserved() {
        if (isObserved)
        {
            timeWaitedAlertDisapear = 0;
            alertBar.SetActive(true);
            warnLevel += warnSpeed / 100 * Time.deltaTime;
            if (currentInvisibleTime >= 1)
            {
                MissionFailed();
            }
        }
        else if (warnLevel > 0)
        {
            warnLevel -= unwarnedSpeed / 100 * Time.deltaTime;
        }
        else
        {
            warnLevel = 0;
            
            if (alertBar.activeInHierarchy)
            {
                timeWaitedAlertDisapear += Time.deltaTime;
                if (timeWaitedAlertDisapear >= timeToWaitAlertDisapear)
                {
                    alertBar.SetActive(false);
                    timeWaitedAlertDisapear = 0;
                }
            }

            

        }
    }

    private void CheckMovment() {
        
        if (canMove && !spiderMove.enabled)
        {
            spiderMove.enabled = true;
        }
        else if (!canMove && spiderMove.enabled)
        {
            spiderMove.enabled = false;
        }
    }




    private void MissionFailed() {
    
    }
}
