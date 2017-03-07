using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FarmLandManager : MonoBehaviour
{
	enum FARM_STATE
	{
		NONE,
		GROWING,
		WAITING_FOR_HARVEST}

	;

	public GameObject FarmLandMenu = null, FarmTimerText = null;
	public float longPressTime = 0.5f;
	float time = 0, harvestTime = 0;
	bool isPressed = false;
	bool isLongPressed = false;
	bool isSeedPlanted = false;
	bool toggleFarmLandMenu = false;
	int seedIndex = 1;
	FARM_STATE farmState = FARM_STATE.NONE;





	void Start ()
	{
		farmState = LoadFarmState ();
		switch (farmState) {
			case FARM_STATE.GROWING:
				PlantIsGrowing ();
				break;
			case FARM_STATE.WAITING_FOR_HARVEST:
				PlantIsWaitingForHarvest ();
				break;
			case FARM_STATE.NONE:

				break;
			default:
				break;
		}
	}

	void OnMouseDown ()
	{	
		isPressed = true;
	}

	void PlantIsGrowing ()
	{
		GetComponent <SpriteRenderer> ().color = Color.green;
		isSeedPlanted = true;
		SaveFarmState (FARM_STATE.GROWING);
	}

	void PlantIsWaitingForHarvest ()
	{
		print ("Plant ready to harvest");
		GetComponent <SpriteRenderer> ().color = Color.white;
		isSeedPlanted = false;
		harvestTime = 0;
		SaveFarmState (FARM_STATE.WAITING_FOR_HARVEST);
	}

	void OnMouseEnter ()
	{
		if (GameEventManager.isSeedSelected == true && isSeedPlanted == false) {
			PlantIsGrowing ();
		}
	}

	void OnMouseUp ()
	{
		if (!isLongPressed && isSeedPlanted == false) {
			FarmLandMenu.transform.position = transform.position;
			toggleFarmLandMenu = !toggleFarmLandMenu;
			if (toggleFarmLandMenu) {
				FarmLandMenu.SetActive (true);
			} else {
				FarmLandMenu.SetActive (false);
			}

		} else {
			FarmTimerText.transform.position = transform.position;
			FarmTimerText.SetActive (true);
			FarmTimerText.GetComponent <TextMeshPro> ().text = harvestTime.ToString ("F0");
		}
		if (isLongPressed) {
			GetComponent <SpriteRenderer> ().color = Color.white;
		}
		time = 0;
		isPressed = false;
		isLongPressed = false;
	}

	void Update ()
	{
		if (isSeedPlanted) {
			harvestTime += Time.deltaTime;
			if (harvestTime >= (seedIndex * 100)) {
				PlantIsWaitingForHarvest ();
			}
		}
		if (isPressed) {
			if (time >= longPressTime) {
				GetComponent <SpriteRenderer> ().color = Color.red;
				isLongPressed = true;
				return;
			}
			time += Time.deltaTime;
		}
	}

	void SaveFarmState (FARM_STATE fState)
	{
		farmState = fState;
		ES2.Save (fState, "state");
	}

	FARM_STATE LoadFarmState ()
	{
		return ES2.Load<FARM_STATE> ("state");
	}
}

