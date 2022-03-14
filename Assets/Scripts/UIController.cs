using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject NoteImage;

    private bool bShouldShowNote = false;
    private float hoverTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        bShouldShowNote = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bShouldShowNote = true;
        hoverTimer = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bShouldShowNote = false;
        NoteImage.SetActive(false);
    }

    private void Update()
    {
        hoverTimer += Time.deltaTime;

        if (bShouldShowNote && hoverTimer - 0.7f > 0f)
        {
            NoteImage.SetActive(true);
        }
    }
}
