using System.Collections;
using UnityEngine;

public class TransformDestablizer : MonoBehaviour
{
    private Coroutine destablizingCoro;
    
    private Vector2 curOffset;
    
    public float leftLimit = 3;
    public float rightLimit = 3;
    public float upLimit = 3;
    public float downLimit = 3;
    
    public float minTime = 0.25f;
    public float maxTime = 1.5f;
    
    public void StartDestablizing()
    {
        if (destablizingCoro != null) {
            return;
        }
        StartLoop();
    }
    
    private void StartLoop()
    {
        destablizingCoro = StartCoroutine(TransformDestabilityLoop(Random.Range(minTime, maxTime)));
    }
    
    public void StopDestablizing()
    {
        if (destablizingCoro == null)
        {
            return;
        }
        StopCoroutine(destablizingCoro);
        destablizingCoro = null;
        
        Vector3 curPos = transform.localPosition;;
        transform.localPosition = new Vector3(
            curPos.x - curOffset.x,
            curPos.y - curOffset.y,
            curPos.z
        );
        curOffset = new Vector2();
    }
    
    private IEnumerator TransformDestabilityLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        JumpPosition();
        StartLoop();
    }
    
    private void JumpPosition()
    {
        Vector2 newOffset = new Vector2(Random.Range(-leftLimit, rightLimit), Random.Range(-downLimit, upLimit));
        Vector3 curPos = transform.localPosition;
        transform.localPosition = new Vector3(
            curPos.x - curOffset.x + newOffset.x,
            curPos.y - curOffset.y + newOffset.y,
            curPos.z
        );
        curOffset = newOffset;
    }
    
    public void ResetOffset()
    {
        curOffset = new Vector2();
    }
}