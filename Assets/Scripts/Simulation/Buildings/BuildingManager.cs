using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simulation
{
	public class BuildingManager : MonoBehaviour
	{
		private SimManager simMan;

		public static BuildingDetails habitatDetails = new BuildingDetails(false, new BuildingFootprint(new ivec2[] { new ivec2(0, 0), new ivec2(1, -1), new ivec2(1, 0), new ivec2(1, 1), new ivec2(0, 1), new ivec2(0, -1), new ivec2(-1, -1), new ivec2(-1, 0), new ivec2(-1, 1) }));
		public static BuildingDetails cafeteriaDetails = new BuildingDetails(false, new BuildingFootprint(new ivec2[] { new ivec2(0, 0), new ivec2(1, -1), new ivec2(1, 0), new ivec2(1, 1), new ivec2(0, 1), new ivec2(0, -1), new ivec2(-1, -1), new ivec2(-1, 0), new ivec2(-1, 1) }));
		public static BuildingDetails hallwayDetails = new BuildingDetails(true, new BuildingFootprint(new ivec2[] { new ivec2(0, 0) }));
		public static BuildingDetails commCenterDetails = new BuildingDetails(false, new BuildingFootprint(new ivec2[] { new ivec2(0, 0), new ivec2(1, 0), new ivec2(1, 1), new ivec2(0, 1) }));
		public static BuildingDetails recRoomDetails = new BuildingDetails(false, new BuildingFootprint(new ivec2[] { new ivec2(0, 0), new ivec2(1, 0), new ivec2(1, 1), new ivec2(0, 1) }));






		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public BuildingManager Initialize(SimManager simMan)
		{
			transform.name = "Building Manager";
			this.simMan = simMan;
			return this;
		}

		public Transform InstantiateBuilding(Vector3 point, Transform buildingSelection)
		{
			Vector3 offsetPoint = SimManager.AdjustForGridOffset(point);
			offsetPoint.y = simMan.terrainManager.GetTerrainCellHeight(point);
			Building newBdg = GameObject.Instantiate(buildingSelection.gameObject, offsetPoint, Quaternion.identity, transform).GetComponent<Building>();
			newBdg.Initialize(this, new BuildingLocation((int)point.x, (int)point.z));


			simMan.terrainManager.SetFootprintOccupied(newBdg.location, GetBuildingDetails(newBdg.type).footprint, true);
			return newBdg.transform;
		}

		public void RemoveBuilding(Transform building)
		{
			Building bdg = building.GetComponent<Building>();
			if (!bdg.isBuilt)
			{
				//Debug.Log("Destoryed Building");
				Destroy(bdg.gameObject);
			} else
			{
				//Set building as idle and put it on the work queue to dismantle
			} 
		}

		public bool CanPlaceBuildingAtLocation(Vector3 point, Transform building)
		{
			point = simMan.terrainManager.GetPointOnGrid(point);
			BuildingFootprint footprint = GetBuildingDetails(building.GetComponent<Building>().type).footprint;
			return !simMan.terrainManager.CheckIfFootprintOccupied(point, footprint) && simMan.terrainManager.CheckIfFootprintFlat(point, footprint);
		}

		public static BuildingDetails GetBuildingDetails(Building.BuidlingType type)
		{
			switch (type)
			{
				case (Building.BuidlingType.Habitat):
					return habitatDetails;
				case (Building.BuidlingType.CommCenter):
					return commCenterDetails;
				case (Building.BuidlingType.Hallway):
					return hallwayDetails;
				case (Building.BuidlingType.RecRoom):
					return recRoomDetails;
				case (Building.BuidlingType.Cafeteria):
					return cafeteriaDetails;
			}
			Debug.LogError("DIDN'T FIND FOOTPRINT!!! FIX IT RIGHT NOW YOU TWAT");
			return new BuildingDetails();
		}
	}
}