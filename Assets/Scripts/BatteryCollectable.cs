using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollectable : MonoBehaviour
{
    
    public enum CollectableType
    {
        Battery,
        Key,
        None,
    }

    public CollectableType currentCollectable = CollectableType.None; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (currentCollectable)
            {
                case CollectableType.Key:
                    GameItems.Instance.SetKeyCount(1);
                    Destroy(gameObject);
                    break;
                case CollectableType.Battery:
                    GameItems.Instance.SetBatteryCount(1);
                    Destroy(gameObject);
                    break;
                default:
                    throw new Exception("BatteryCollectable: Unknow collectable type");
            }
        }
    }
}