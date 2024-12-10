using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePlayer 
{
    
    private static Dictionary<string,HealthController> _playerList = new Dictionary<string, HealthController>();
    public static void RegisterPlayer(string netId,HealthController player){
        _playerList.Add("Player_"+ netId,player);
    }
    public static void UnRegisterPlayer(string netId){
        _playerList.Remove("Player_"+netId);
    }
    public static HealthController GetPlayer(string netId){
        return _playerList[netId];
    }
}
