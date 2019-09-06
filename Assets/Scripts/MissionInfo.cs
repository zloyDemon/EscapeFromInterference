using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo
{
    private int missionIndex;
    private int needKeys;
    private int batteryCount;
    private int level;
    private int ghostCount;

    public int MissionIndex { get { return missionIndex; }}
    public int NeedKeys { get { return needKeys; }}
    public int BatteryCount { get { return batteryCount; }}
    public int Level { get { return level; }}
    public int GhostCount { get { return ghostCount; }}

    public MissionInfo(int missionIndex, int needKeys, int batteryCount, int ghostCount)
    {
        this.missionIndex = missionIndex;
        this.needKeys = needKeys;
        this.batteryCount = batteryCount;
        this.ghostCount = ghostCount;
        level = missionIndex + 1;
    }
}
