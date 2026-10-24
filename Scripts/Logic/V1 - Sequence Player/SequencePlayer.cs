using System;
using System.Collections.Generic;
using UnityEngine;

////////////////////
//
//  THE CONDUCTOR
//
////////////////////

public class SequencePlayer : MonoBehaviour
{
    private PhaseTimeline currentTimeline;
    private float elapsedTime = 0f;
    private int currentIndex = 0;
    private bool isPlaying = false;

    // Action/Event system so other scripts (like Spawners or UI) can listen in
    public static event Action<TimelineEvent> OnAttackTriggered;
    public static event Action OnTimelineFinished;

    public void StartTimeline(PhaseTimeline timeline)
    {
        currentTimeline = timeline;
        
        // Sort events by timestamp just in case they were misconfigured in data
        currentTimeline.events.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
        
        elapsedTime = 0f;
        currentIndex = 0;
        isPlaying = true;
    }

    public void StopTimeline()
    {
        isPlaying = false;
    }

    private void Update()
    {
        if (!isPlaying || currentTimeline == null) return;

        // Progress the clock using delta time accumulation
        elapsedTime += Time.deltaTime;

        // Check if we have events left and if the current time has passed
        // the next target timestamp. The while loop allows for multiple events
        // to trigger in the same frame if they share equal or close timestamps
        while (currentIndex < currentTimeline.events.Count && 
               elapsedTime >= currentTimeline.events[currentIndex].timestamp)
        
        {
            // Fire off the attack event
            TriggerEvent(currentTimeline.events[currentIndex]);
            
            // Move to the next note in the sheet music
            currentIndex++;
        }

        // Loop or end-of-phase checks
        if (currentIndex >= currentTimeline.events.Count
            && elapsedTime > currentTimeline.phaseDuration)
        {
            EndTimeline();
        }
    }

    private void TriggerEvent(TimelineEvent timelineEvent)
    {
        Debug.Log($"[{elapsedTime:F2}s]: Triggering {timelineEvent.id}");
        
        // Notify the game world that it's time to spawn an attack
        OnAttackTriggered?.Invoke(timelineEvent);
    }

    private void EndTimeline()
    {
        isPlaying = false;
        OnTimelineFinished?.Invoke();
        Debug.Log("Timeline completed. Transitioning to next phase..."); 
    }
}
