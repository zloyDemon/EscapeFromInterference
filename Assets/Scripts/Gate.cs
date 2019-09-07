﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var needKey = MissionManager.Instance.CurrentMission.NeedKeys;
            if(GameItems.Instance.KeyCount >= needKey)
                MissionManager.Instance.MissionComplete();
            else
                SubtitleManager.Instance.SetSubtitle("I need more key for this door");
        }
    }
}
