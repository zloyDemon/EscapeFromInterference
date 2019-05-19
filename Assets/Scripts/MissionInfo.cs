using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo
{
    private int missionId;
    private int needKey;
    private int batteryCount;
    private int level;
    private int ghostCount;

    public int MissionId { get { return missionId; }}
    public int NeedKey { get { return needKey; }}
    public int BatteryCount { get { return batteryCount; }}
    public int Level { get { return level; }}
    public int GhostCount { get { return ghostCount; }}

    public MissionInfo(int missionId, int needKey, int batteryCount, int ghostCount)
    {
        this.missionId = missionId;
        this.needKey = needKey;
        this.batteryCount = batteryCount;
        this.ghostCount = ghostCount;
        level = missionId;
    }
}
