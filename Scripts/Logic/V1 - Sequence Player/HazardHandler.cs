using UnityEngine;
using System.Collections.Generic;

////////////////////
//
// THE ORCHESTRA
//
////////////////////

public class HazardHandler : MonoBehaviour
{
    
    private void OnEnable() => SequencePlayer.OnAttackTriggered += HandleEvent;
    private void OnDisable() => SequencePlayer.OnAttackTriggered -= HandleEvent;

    private void HandleEvent(TimelineEvent attackEvent)
    {
        switch (attackEvent.id)
        {
            // TODO: Add the action hazard events here.
            case "AoE_Circle":
                //SpawnCircleTelegraph(attackEvent.parameters);
                break;
                
            case "Bullet_Rain":
                //StartCoroutine(ExecuteBulletRain(attackEvent.parameters));
                break;
                
            default:
                Debug.LogWarning($"Unknown attack ID: {attackEvent.id}");
                break;
        }
    }

    private void SpawnCircleTelegraph(Dictionary<string, float> parameters)
    {
        // Logic for drawing FFXIV-style orange warning circle on the ground
    }

    private System.Collections.IEnumerator ExecuteBulletRain(Dictionary<string, float> parameters)
    {
        // Logic for Undertale-style sustained bullet spawning over a duration
        yield return null;
    }
}
