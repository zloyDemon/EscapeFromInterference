using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo
{
    private int missionId;
    private int needKey;
    private int batteryCount;
    private int level;

    public int MissionId { get { return missionId; }}
    public int NeedKey { get { return needKey; }}
    public int BatteryCount { get { return batteryCount; }}
    public int Level { get { return level; }}

    public MissionInfo(int missionId, int needKey, int batteryCount)
    {
        this.missionId = missionId;
        this.needKey = needKey;
        this.batteryCount = batteryCount;
        level = missionId;
    }
}
