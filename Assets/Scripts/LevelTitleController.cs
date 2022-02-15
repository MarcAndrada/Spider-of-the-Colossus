using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelTitleController : MonoBehaviour
{
    [SerializeField]
    private GameObject ParentCanvas;
    [SerializeField]
    private string textContent;

    private Text text;
    private float textAlpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = textContent;

    }

    // Update is called once per frame
    void Update()
    {
        textAlpha -= 0.1f * Time.deltaTime;


        text.color = new Color(text.color.r, text.color.g, text.color.b, textAlpha);

        if (textAlpha <= 0) {
            Destroy(ParentCanvas);
        }
    }
}
