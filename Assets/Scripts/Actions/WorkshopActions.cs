using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopActions : MonoBehaviour {

    public GameObject upgrade;
    public void UpdateWorkshop(){
        if (GameData._playerData.WorkshopOpen >= GameConfigs.MaxLv_Workshop)
            upgrade.SetActive(false);
        else
            upgrade.SetActive(true);
    }
}
