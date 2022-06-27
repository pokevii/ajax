using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleMainController : MonoBehaviour
{
    private CreditsController credsController;
    
    private Canvas canvasComp;
    private Dictionary<EventTrigger, EventTrigger.Entry[]> titleMainTrigs
        = new Dictionary<EventTrigger, EventTrigger.Entry[]>();
    
    private List<TransformDestablizer> transfDestabs = new List<TransformDestablizer>();
    
    void Start()
    {
        credsController = transform.parent.Find("Credits").GetComponent<CreditsController>();
        
        canvasComp = GetComponent<Canvas>();
        
        foreach (Transform child in transform)
        {
            TransformDestablizer transfDestab = child.GetComponent<TransformDestablizer>();
            if (transfDestab)
            {
                transfDestabs.Add(transfDestab);
            }
        }
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnCreditsPointerClick((PointerEventData) data); });
        titleMainTrigs[transform.Find("ShowCredits").GetComponent<EventTrigger>()]
            = new EventTrigger.Entry[1] { entry };
        
        SetupActivity();
    }
    
    public void SetupActivity()
    {
        foreach (KeyValuePair<EventTrigger, EventTrigger.Entry[]> kvp in titleMainTrigs)
        {
            foreach (EventTrigger.Entry entry in kvp.Value)
            {
                kvp.Key.triggers.Add(entry);
            }
        }
        
        foreach (TransformDestablizer transfDestab in transfDestabs)
        {
            transfDestab.StartDestablizing();
        }
    }
    
    public void EnableTitleMain()
    {
        canvasComp.enabled = true;
        SetupActivity();
    }
    
    public void DisableTitleMain()
    {
        canvasComp.enabled = false;
        
        foreach (KeyValuePair<EventTrigger, EventTrigger.Entry[]> kvp in titleMainTrigs)
        {
            kvp.Key.triggers.Clear();
        }
        
        foreach (TransformDestablizer transfDestab in transfDestabs)
        {
            transfDestab.StopDestablizing();
        }
    }
    
    public void OnCreditsPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) {
            return;
        }
        
        DisableTitleMain();
        credsController.EnableCredits();
    }
}