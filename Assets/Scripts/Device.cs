using UnityEngine;

[RequireComponent(typeof(CheckEnemy))]
public class Device : MonoBehaviour
{
    private CheckEnemy checkEnemy;

    public float DeviceValue { get; set; }
    
    private void Awake()
    {
        checkEnemy = GetComponent<CheckEnemy>();
        checkEnemy.EnemyChecked += EnemyChecked;
        checkEnemy.Init(CheckEnemy.CheckTargetType.Ghost);
    }

    private void Start()
    {
        DeviceManager.Instance.SetSignalValue(0.01f);
    }

    private void OnDestroy()
    {
        checkEnemy.EnemyChecked -= EnemyChecked;
    }

    private void EnemyChecked(GameObject[] enemies)
    {
        if (enemies.Length <= 0) return;
        var value = CalcDeviceValue(enemies[0].transform);
        DeviceManager.Instance.SetSignalValue(value);
    }

    private float CalcDeviceValue(Transform nearEnemy)
    {
        var distance = Vector2.Distance(transform.position, nearEnemy.transform.position); // 20
        
        if (distance > DeviceManager.CheckDistance)
            return 0.01f;
        
        var deviceValue = 1 - ((distance / DeviceManager.CheckDistance));
        
        if (deviceValue > 1)
            deviceValue = 1;
        
        return deviceValue;
    }
}
