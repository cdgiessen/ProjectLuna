using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum MouseMode
	{
		idle,
		build,
		destroyBuilding,
		order_goto,
		order_work,
		order_transport,
		order_rest,
		order_clean,
		order_dig,
	}
	private MouseMode currentMode = MouseMode.idle;

	public Simulation.SimManager simMan;
	public Transform simManTransform;
	public Transform currentlyHighlighted;
	public Transform currentlySelected;

	public Transform currentBuildListSelection;
	private Transform currentBuildingPreview;
	private bool currentPreviewCanPlace;
	


	public Transform[] listOfBuildings;


	public Material transparentRed;
	public Material transparentGreen;
	public Material transparentWhite;

	public bool mouseOverUI = false;

	private LayerMask terrainMask, buildingMask, colonistMask;


	// Use this for initialization
	void Start () {
		simMan = simManTransform.GetComponent<Simulation.SimManager>();

		terrainMask = 1 << LayerMask.NameToLayer("Terrain");
		buildingMask = 1 << LayerMask.NameToLayer("Building");
		colonistMask = 1 << LayerMask.NameToLayer("Colonist");
	}
	
	// Update is called once per frame
	void Update () {

		if (!mouseOverUI)
		{
			RaycastHit hit, terrainHit, buildingHit, colonistHit;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue);

			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out terrainHit, float.MaxValue, terrainMask);
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out buildingHit, float.MaxValue, buildingMask);
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out colonistHit, float.MaxValue, colonistMask);


			switch (currentMode)
			{
				case (MouseMode.idle):
					if (Input.GetMouseButtonDown(0))
					{
						if (hit.transform != null)
						{
							currentlySelected = hit.transform;
							Debug.Log("Current Selection " + currentlySelected);
							if (currentlySelected.GetComponentInParent<Simulation.TerrainChunk>() != null)
							{
								//Show cell properties (like if its been explored, surveyed, and minerals within it)
							}
							if (currentlySelected.GetComponentInParent<Simulation.Building>() != null)
							{
								//Highlight building then show current building stats
							}
							if (currentlySelected.GetComponentInParent<Simulation.Colonist>() != null)
							{
								//highlight colonist then show colonist stats and current activity.
							}
						}
					}
					break;
				case (MouseMode.build):


					if (currentBuildingPreview == null)
					{
						currentBuildingPreview = Instantiate(currentBuildListSelection, simMan.terrainManager.GetPointOnGrid(terrainHit.point), Quaternion.identity, transform).transform;
						currentBuildingPreview.GetComponentsInChildren<MeshRenderer>()[0].material = transparentGreen;
					}

					currentPreviewCanPlace = simMan.buildingManager.CanPlaceBuildingAtLocation(terrainHit.point, currentBuildListSelection);

					if (currentPreviewCanPlace)
					{
						currentBuildingPreview.GetComponentsInChildren<MeshRenderer>()[0].material = transparentGreen;
					}
					else
					{
						currentBuildingPreview.GetComponentsInChildren<MeshRenderer>()[0].material = transparentRed;
					}
					currentBuildingPreview.transform.position = Simulation.SimManager.AdjustForGridOffset(simMan.terrainManager.GetPointOnGrid(terrainHit.point));


					if (Input.GetMouseButtonDown(0))
					{
						if (currentBuildListSelection != null && !mouseOverUI && currentPreviewCanPlace)
						{
							simMan.buildingManager.InstantiateBuilding(simMan.terrainManager.GetPointOnGrid(terrainHit.point), currentBuildListSelection).GetComponentInChildren<MeshRenderer>().material = transparentWhite;
						}
					}

					break;

				case (MouseMode.destroyBuilding):
					if (Input.GetMouseButtonDown(0))
					{
						Debug.Log(currentlyHighlighted.name);
						if (currentlyHighlighted.GetComponentInParent<Simulation.Building>() != null)
							simMan.buildingManager.RemoveBuilding(currentlyHighlighted.parent);
					}

					break;
				case (MouseMode.order_goto):
					break;
				case (MouseMode.order_work):
					break;
				case (MouseMode.order_transport):
					break;
				case (MouseMode.order_rest):
					break;
				case (MouseMode.order_clean):
					break;

				case (MouseMode.order_dig): //CURRENTLY FOR MANUALLY EDITING TERRAIN
					if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
					{
						simMan.terrainManager.ChangeTerrainCellHeight(terrainHit.point, -1);
					}

					else if (Input.GetMouseButtonDown(0))
					{
						simMan.terrainManager.ChangeTerrainCellHeight(terrainHit.point, 1);
					}
					break;
			}


			if (Input.GetMouseButtonDown(1))
			{
				currentMode = MouseMode.idle;
				currentPreviewCanPlace = false;
				currentBuildListSelection = null;
				if (currentBuildingPreview != null)
					Destroy(currentBuildingPreview.gameObject);
			}


			if (Input.GetKey(KeyCode.F))
			{
				if (currentlySelected != null && currentlySelected.GetComponentInParent<Simulation.Colonist>() != null)
				{

					currentlySelected.GetComponentInParent<Simulation.Colonist>().GotoTarget(terrainHit.point);

				}
			}
			/*
			if (Input.GetMouseButtonDown(0)) {
				simMan.terrainManager.ChangeTerrainCellHeight(terrainHit.point, 1);
			}

			if (Input.GetMouseButtonDown(1)) {
				simMan.terrainManager.ChangeTerrainCellHeight(terrainHit.point, -1);
			}*/
		}
	}

	public void SetModeDestroyBuilding()
	{
		currentMode = MouseMode.destroyBuilding;
		Debug.Log("Set input mode Destory Building");
	}

	public void SetModeOrderGoto()
	{
		currentMode = MouseMode.order_goto;
		Debug.Log("Set input mode goto");
	}

	public void SetModeOrderWork()
	{
		currentMode = MouseMode.order_work;
		Debug.Log("Set input mode work");
	}

	public void SetModeOrderTransport()
	{
		currentMode = MouseMode.order_transport;
		Debug.Log("Set input mode transport");
	}

	public void SetModeOrderRest()
	{
		currentMode = MouseMode.order_rest;
		Debug.Log("Set input mode rest");
	}

	public void SetModeOrderClean()
	{
		currentMode = MouseMode.order_clean;
		Debug.Log("Set input mode clean");
	}

	public void SetModeOrderDig()
	{
		currentMode = MouseMode.order_dig;
		Debug.Log("Set input mode dig CURRENTLY FOR MANUALLY EDITING TERRAIN");
	}

	public void SelectBuildListItem(string buildingName)
	{
		currentMode = MouseMode.build;

		currentBuildListSelection = null;
		if(currentBuildingPreview != null)	
			Destroy(currentBuildingPreview.gameObject);
		currentBuildingPreview = null;
		foreach (Transform trans in listOfBuildings)
		{
			if(trans.name == buildingName)
			{
				currentBuildListSelection = trans;
			}
		}
		currentPreviewCanPlace = true;
		Debug.Log("Selected building: " + buildingName);

	}

	public void MouseOverUI()
	{
		mouseOverUI = true;
	}

	public void MouseExitUI()
	{
		mouseOverUI = false;
	}
}
