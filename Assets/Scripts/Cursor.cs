using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour
{
    // Update is called once per frame
    public void changePosition(GameObject pEmote)
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(pEmote.GetComponent<RectTransform>().anchoredPosition.x + 10, this.GetComponent<RectTransform>().anchoredPosition.y);
    }
}
