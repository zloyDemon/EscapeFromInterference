using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var needKey = GameplayManager.Instance.CurrentMissionInfo.NeedKeys;
            Debug.LogFormat("NeedKey {0} CurrentKeyCount {1}", needKey, GameItems.Instance.KeyCount);
            if(GameItems.Instance.KeyCount >= needKey)
                GameplayManager.Instance.MissionComplete();
            else
                SubtitleManager.Instance.SetSubtitle("I need more key for this door");
        }
    }
}
