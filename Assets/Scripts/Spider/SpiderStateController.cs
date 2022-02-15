using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SpiderStateController : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField]
    private GameObject invisibilityBar;
    [SerializeField]
    private GameObject alertBar;
    [SerializeField]
    private GameObject hackingBar;
    [SerializeField]
    private Text NeededHPoints;
    [SerializeField]
    private Text hackedPointsT;
    [SerializeField]
    private GameObject missionComplete;
    [SerializeField]
    private GameObject missionFailed;
    [Header("Stats")]
    [SerializeField]
    private float invisibleConsumeSpeed;
    [SerializeField]
    private float invisibleReloadSpeed;
    [SerializeField]
    private float warnSpeed;
    [SerializeField]
    private float unwarnedSpeed;
    [SerializeField]
    private float hackingSpeed;
    [SerializeField]
    private float hackingDowngradeSpeed;
    [Header("Invisibility Materials")]
    [SerializeField]
    private SkinnedMeshRenderer SpiderModel;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material invisibleMaterial;
    [Header ("Others")]
    [SerializeField]
    private MainMenu sceneManager;

    private bool canMove = true;
    private bool isInvisible = false;
    private float currentInvisibleTime = 1;
    private bool isObserved;
    private float warnLevel;
    private float timeToWaitInvBarDisapear = 1.5f;
    private float timeWaitedInvBarDisapear = 0;
    private float timeToWaitAlertDisapear = 1.5f;
    private float timeWaitedAlertDisapear = 0;
    private bool onHackingZone = false;
    private bool isHacking = false;
    private float hackingProgress = 0;
    private float timeToWaitHBDisapear = 1.5f;
    private float timeWaitedHBDisapear = 0;
    private int hackedPoints = 0;
    private int totalNeededPoints = 0;
    private bool win = false;
    private bool lost = false;

    private SpiderController spiderMove;
    private SpiderProceduralAnimation spiderLegs;
    private Slider invisivilityBarSlider;
    private Slider alertBarSlider;
    private GameObject lastHackingPoint;
    private Slider hackingBarSlider;
    private SoundManager soundCont;

    // Start is called before the first frame update
    void Start()
    {

        spiderMove = GetComponent<SpiderController>();
        spiderLegs = GetComponentInChildren<SpiderProceduralAnimation>();
        invisivilityBarSlider = invisibilityBar.GetComponent<Slider>();
        alertBarSlider = alertBar.GetComponent<Slider>();
        hackingBarSlider = hackingBar.GetComponent<Slider>();
        GameObject[] hackingPointsScene = GameObject.FindGameObjectsWithTag("HackingPoint");
        NeededHPoints.text = hackingPointsScene.Length.ToString();
        totalNeededPoints = hackingPointsScene.Length;
        if (GameObject.FindGameObjectWithTag("SoundManager") != null)
        {
            soundCont = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvisibleControls();
        CheckIfInvisible();
        CheckMovment();
        CheckIfObserved();
        SetHudValues();
        CheckHackZone();
        CheckIfMissionComplete();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isObserved = true;
        }

        if (other.gameObject.tag == "HackingPoint")
        {
            onHackingZone = true;
            lastHackingPoint = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isObserved = false;
        }

        if (other.gameObject.tag == "HackingPoint")
        {
            onHackingZone = false;
        }
    }

    private void TurnInvisible() {
        //Volverse Invisible
        isInvisible = true;
        SpiderModel.material = invisibleMaterial;
        soundCont.TurnInvisible();

    }

    private void TurnVisible() {
        //Volverse visible
        isInvisible = false;
        SpiderModel.material = normalMaterial;
        soundCont.TurnVisible();
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
            invisibilityBar.SetActive(true);

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
            if (invisibilityBar.activeInHierarchy)
            {
                timeWaitedInvBarDisapear += Time.deltaTime;
                if (timeWaitedInvBarDisapear >= timeToWaitInvBarDisapear)
                {
                    invisibilityBar.SetActive(false);
                    timeWaitedInvBarDisapear = 0;
                }
            }
        }


    }

    public void SetHudValues() {
        invisivilityBarSlider.value = currentInvisibleTime;
        alertBarSlider.value = warnLevel;
        //Hacer que un slider con una barra de hackeo aumente
        hackingBarSlider.value = hackingProgress;
        hackedPointsT.text = hackedPoints.ToString();

    }

    private void CheckIfObserved() {
        if (isObserved && !isInvisible)
        {
            timeWaitedAlertDisapear = 0;
            alertBar.SetActive(true);
            warnLevel += warnSpeed / 100 * Time.deltaTime;
            if (warnLevel >= 1 && !lost)
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

        if (!isInvisible && !isHacking)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        if (canMove && !spiderMove.enabled)
        {
            spiderMove.enabled = true;
        }
        else if (!canMove && spiderMove.enabled)
        {
            spiderMove.enabled = false;
        }
    }

    private void CheckHackZone() {

        if (onHackingZone)
        {
            timeWaitedHBDisapear = 0;
            //hacer visible el boton de E
            if (Input.GetButton("Hack"))
            {

                hackingBar.SetActive(true);
                //Bloquear movimiento
                isHacking = true;
                //Hacer que un slider con una barra de hackeo aumente
                hackingProgress += hackingSpeed / 100 * Time.deltaTime;
                if (hackingProgress >= 1)
                {
                    HackComplete();
                }
            }
            else if (hackingProgress > 0)
            {
                isHacking = false;

                hackingProgress -= hackingDowngradeSpeed / 100 * Time.deltaTime;
            }
            else
            {
                //cambiar el estado de isHacking
                isHacking = false;
                hackingProgress = 0;
            }

            if (Input.GetButtonDown("Hack"))
            {
                soundCont.StartHack();
            }
            else if (Input.GetButtonUp("Hack"))
            {
                soundCont.StopHack();

            }
        }
        else
        {
            //Esconder Boton de E
            //cambiar el estado de isHacking
            isHacking = false;
            hackingProgress = 0;

            if (hackingBar.activeInHierarchy)
            {
                timeWaitedHBDisapear += Time.deltaTime;
                if (timeWaitedHBDisapear >= timeToWaitHBDisapear)
                {
                    hackingBar.SetActive(false);
                    timeWaitedHBDisapear = 0;
                }
            }
        }
    }

    private void HackComplete() {

        Destroy(lastHackingPoint);
        onHackingZone = false;
        //sumar 1 en el contador de puntos hackeados
        hackedPoints++;
        soundCont.StopHack();
        soundCont.HackCompleted();


    }

    private void MissionFailed() {
        soundCont.StopHack();
        missionFailed.SetActive(true);
        StartCoroutine(RestartPosFailed());
        soundCont.MissionFailed();
        lost = true;

    }

    private void CheckIfMissionComplete() {

        if (totalNeededPoints <= hackedPoints && !win)
        {
            missionComplete.SetActive(true);
            //hacer sonido de victoria
            //hacer que si la escena no es tutorial que llame a la funcion de volver al menu

            StartCoroutine(sceneManager.MissionComplete());

            soundCont.MissionComplete();
            win = true;
        }

    }

    public bool IsInvisible()
    {

        return isInvisible;
    }

    public void IsSeen() {
        isObserved = true;
        
    }

    public void IsntSeen()
    {

        isObserved = false;
    }


    IEnumerator RestartPosFailed() {
        TransitionController.ChangeScene();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 100; i++)
        {
            spiderLegs.RestartPos();
            spiderMove.RestartPosition();
        }
        warnLevel = 0;
        missionFailed.SetActive(false);
        lost = false;


    }

}
