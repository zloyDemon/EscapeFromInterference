using UnityEngine;

[RequireComponent(typeof(CheckEnemy))]
public class Device : MonoBehaviour
{
    public static readonly float DistanceForEnd = 1.2f;
    public static readonly float CheckGhostDeviceMaxValue = 5f;
    public static readonly float CheckDistance = 3f;
    
    private CheckEnemy checkEnemy;

    public float DeviceValue { get; set; }
    
    private void Awake()
    {
        checkEnemy = GetComponent<CheckEnemy>();
        checkEnemy.EnemyChecked += EnemyChecked;
    }

    private void OnDestroy()
    {
        checkEnemy.EnemyChecked -= EnemyChecked;
    }

    private void EnemyChecked(GameObject[] enemies)
    {
        if (enemies.Length <= 0) return;
        var nearEnemy = enemies[0];
        var distance = Vector2.Distance(transform.position, nearEnemy.transform.position);
        var deviceValue = DeviceValueF(distance);
        if (deviceValue > EFIUtils.CheckGhostDeviceMaxValue)
            deviceValue = EFIUtils.CheckGhostDeviceMaxValue;
        GameItems.Instance.SetDeviceValue(deviceValue / EFIUtils.CheckGhostDeviceMaxValue);
        if (distance > EFIUtils.CheckDistance)
            GameItems.Instance.SetDeviceValue(0);
    }
    
    public float DeviceValueF(float distance)
    {
        return CheckGhostDeviceMaxValue - ((distance * CheckGhostDeviceMaxValue) / CheckDistance);
    }
}
