using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShowText : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The text shown will be formatted using this string.  {0} is replaced with the actual value")]
    private readonly string formatText = "{0}%";


    private void Start()
    {

        GetComponentInParent<Slider>().onValueChanged.AddListener(HandleValueChanged);
    }



    private void HandleValueChanged(float arg0)
    {
        GetComponent<UnityEngine.UI.Text>().text = string.Format(formatText, GetComponentInParent<Slider>().value);

    }
}