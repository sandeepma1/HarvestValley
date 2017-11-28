using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MASTER_SaveEverything : MonoBehaviour
{
    public static MASTER_SaveEverything Instance = null;

    void Awake()
    {
        Instance = this;
    }

    public void SaveAll()
    {

    }

    void SavePlayerInventory<T>(T parm)
    {

    }

    void SavePlayerProfile()
    {

    }

    void SaveGameFarmFields()
    {

    }
}
