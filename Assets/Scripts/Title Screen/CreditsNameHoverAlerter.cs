using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsNameHoverAlerter : MonoBehaviour
{
    private CreditsController receiver;
    private EventTrigger trig;
    private EventTrigger.Entry[] trigEntries = new EventTrigger.Entry[2];
    
    public float credsDispY;
    
    [TextArea(3, 3)]
    public string credits
#if UNITY_EDITOR
    ;
#else
    { get; }
#endif

    void Start()
    {
        trig = GetComponent<EventTrigger>();
        
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData) data); });
        
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData) data); });
        
        trigEntries[0] = enterEntry;
        trigEntries[1] = exitEntry;
    }
    
    public void StartListening(CreditsController receiver) {
        this.receiver = receiver;
        
        foreach (EventTrigger.Entry entry in trigEntries)
        {
            trig.triggers.Add(entry);
        }
    }
    
    public void StopListening()
    {
        trig.triggers.Clear();
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        receiver.OnCreditsNameEnter(credits, credsDispY);
    }
    
    public void OnPointerExit(PointerEventData data)
    {
        receiver.OnCreditsNameExit();
    }
    
    public string GetCredits()
    {
        return credits;
    }
}