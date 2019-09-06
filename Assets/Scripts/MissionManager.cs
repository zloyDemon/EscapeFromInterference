using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager 
{
    public static MissionManager Instance { get; private set; } = new MissionManager();
    
    public MissionInfo CurrentMission { get; private set; }
    public List<MissionInfo> Missions { get; private set; }
    public event Action<MissionInfo> MissionCompleted = m => { };
    
    public void Init()
    {
        Missions = GenetatesMissions();
        CurrentMission = Missions[0];
        Debug.Log("MenuManager Init");
    }

    public MissionInfo NextMission()
    {
        var nextMissionIndex = CurrentMission.MissionIndex + 1;
        MissionInfo result = null;

        if(nextMissionIndex > 0 && nextMissionIndex < Missions.Count)
            result = Missions[nextMissionIndex];
        
        return result;
    }

    public void ChangeMission(int missionIndex)
    {
        if(missionIndex < 0 && missionIndex >= Missions.Count)
            throw new Exception($"MissionManager: ChangeMission: Mission with {missionIndex} not found.");
        
        CurrentMission = Missions[missionIndex];    
    }

    public void MissionComplete()
    {
        CurrentMission.IsCompleted = true;
        var completedMission = CurrentMission.Clone() as MissionInfo;
        var nextMission = NextMission();
        if(nextMission != null)
            ChangeMission(nextMission.MissionIndex);
        Debug.Log($"MissionCompleted: completedMission {completedMission.Level} nextMission {CurrentMission.Level} ");
        MissionCompleted(completedMission);
    }

    public bool CheckAllMissionsCompleted()
    {
        return Missions.All(m => m.IsCompleted);
    }
    
    //TODO For test
    private List<MissionInfo> GenetatesMissions()
    {
        List<MissionInfo> missions = new List<MissionInfo>
        {
            new MissionInfo(0, 3,2,2,false),
            new MissionInfo(1, 2,3,2,false),
        };
        return missions;
    }
}
