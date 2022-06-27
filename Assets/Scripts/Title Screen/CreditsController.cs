using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CreditsController : MonoBehaviour
{
    private TitleMainController mainController;
    
    private Canvas canvasComp;
    private Dictionary<EventTrigger, EventTrigger.Entry[]> credTrigs
        = new Dictionary<EventTrigger, EventTrigger.Entry[]>();
    
    private List<TransformDestablizer> transfDestabs = new List<TransformDestablizer>();
    
    private TextMeshProUGUI credsTextComp;
    private TransformDestablizer credsDestabComp;
    //private List<CreditsNameHoverAlerter> hoverAlerters;
    
    void Start()
    {
        mainController = transform.parent.Find("Main").GetComponent<TitleMainController>();
        
        canvasComp = GetComponent<Canvas>();
        
        Transform credsBox = transform.Find("CredsBox");
        credsTextComp = credsBox.GetComponent<TextMeshProUGUI>();
        credsDestabComp = credsBox.GetComponent<TransformDestablizer>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnReturnPointerClick((PointerEventData) data); });
        credTrigs[transform.Find("BackToMain").GetComponent<EventTrigger>()]
            = new EventTrigger.Entry[1] { entry };
        
        foreach (Transform child in transform)
        {
            TransformDestablizer transfDestab = child.GetComponent<TransformDestablizer>();
            if (transfDestab)
            {
                transfDestabs.Add(transfDestab);
            }
        }
        
        // I really can't be bothered to modularize this stuff rn
        // Wake me up when I don't have effectively 8 hours left lol
        foreach (Transform child in transform.Find("Contributors"))
        {
            TransformDestablizer transfDestab = child.GetComponent<TransformDestablizer>();
            if (transfDestab)
            {
                transfDestabs.Add(transfDestab);
            }
        }
        
        //Transform contribTransf = transform.Find("Contributors").transform;
        //hoverAlerters = new List<CreditsNameHoverAlerter>(contribTransf.childCount);
        /*foreach (Transform child in contribTransf)
        {
            // inform the alerter of where the text should be located
            CreditsNameHoverAlerter alerter = child.gameObject
                .GetComponent<CreditsNameHoverAlerter>();
            hoverAlerters.Add(alerter);
            credsTextComp.text = alerter.GetCredits();
            credsTextComp.ForceMeshUpdate(); 
            float halfCanvasHeight = GameObject.FindGameObjectWithTag("CanvasRoot")
                .GetComponent<CanvasScaler>().referenceResolution.y;
            float halfTextHeight = credsTextComp.preferredHeight / 2.0f;
            float desiredY = child.transform.localPosition.y;
            alerter.credsDispY = desiredY + halfTextHeight > halfCanvasHeight
                    ? halfCanvasHeight - halfTextHeight
                    : desiredY;
        }*/
        // clear the text
        //credsTextComp.text = "";
    }
    
    public void EnableCredits()
    {
        canvasComp.enabled = true;
        
        /*foreach (CreditsNameHoverAlerter alerter in hoverAlerters)
        {
            alerter.StartListening(this);
        }*/
        
        foreach (KeyValuePair<EventTrigger, EventTrigger.Entry[]> kvp in credTrigs)
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
    
    public void DisableCredits()
    {
        canvasComp.enabled = false;
        
        /*foreach (CreditsNameHoverAlerter alerter in hoverAlerters)
        {
            alerter.StopListening();
        }*/
        
        foreach (KeyValuePair<EventTrigger, EventTrigger.Entry[]> kvp in credTrigs)
        {
            kvp.Key.triggers.Clear();
        }
        
        foreach (TransformDestablizer transfDestab in transfDestabs)
        {
            transfDestab.StopDestablizing();
        }
    }
    
    public void OnCreditsNameEnter(string credits, float dispY)
    {
        credsDestabComp.ResetOffset();
        credsTextComp.text = credits;
        Vector3 curPos = credsTextComp.transform.localPosition;
        credsTextComp.transform.localPosition = new Vector3(curPos.x, dispY, curPos.z);
    }
    
    public void OnCreditsNameExit()
    {
        credsTextComp.text = "";
    }
    
    public void OnReturnPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) {
            return;
        }
        
        DisableCredits();
        mainController.EnableTitleMain();
    }
}