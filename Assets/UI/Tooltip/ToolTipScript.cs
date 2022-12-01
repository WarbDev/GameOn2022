using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponant;
    [SerializeField] RectTransform myRectTransform;
    [SerializeField] Camera myCamera;
    private RectTransform parentRectTransform;

    private static ToolTipScript instance;

    private void Awake()
    {
        instance = this;
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        HideToolTip();
    }

    //private void Start()
    //{
    //    ShowToolTip("Punch: fool fool fool fishing fooolish fool fool FOOOOOOOOOOOOOOOOOOOOL WHIIPSERSNAPPER");
    //}

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, myCamera, out localPoint);
        transform.localPosition = localPoint;
    }

    private void ShowToolTip(string text)
    {
        gameObject.SetActive(true);

        textComponant.text = text;
        int padding = 15;
        Vector2 size = new Vector2(textComponant.preferredWidth*.5f + padding*2, textComponant.preferredHeight * .5f + padding * 2);

        myRectTransform.sizeDelta = size;

    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string tooltipString)
    {
        instance.ShowToolTip(tooltipString);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }
}
