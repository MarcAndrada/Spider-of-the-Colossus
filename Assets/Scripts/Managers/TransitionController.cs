using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{

    static private Animator animator;
    public GameObject publicLoadingIcon;
    static public GameObject loadingIcon;

    private float timeWaited;
    private float TimeToWait = 2000;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        loadingIcon = publicLoadingIcon;
    }

    private void Update()
    {
        float delta = Time.deltaTime * 1000;
        if (publicLoadingIcon.activeInHierarchy)
        {
            timeWaited += delta;
        }


        if (timeWaited >= TimeToWait)
        {
            loadingIcon.SetActive(false);
            timeWaited = 0;
        }
    }

    public static void ChangeScene()
    {
        animator.SetTrigger("Salida");
    }

    public static void ActiveLoadIcon()
    {
        loadingIcon.SetActive(true);
    }

    public static void DesactiveLoadIcon()
    {
        loadingIcon.SetActive(false);

    }
}
