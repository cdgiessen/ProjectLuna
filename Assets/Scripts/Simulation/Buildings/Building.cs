using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simulation
{
	public struct BuildingDetails
	{
		public bool CanBeBuiltOnUnevenTerrain;
		public BuildingFootprint footprint;

		public BuildingDetails(bool CanBeBuiltOnUnevenTerrain, BuildingFootprint footprint)
		{
			this.CanBeBuiltOnUnevenTerrain = CanBeBuiltOnUnevenTerrain;
			this.footprint = footprint;
		}
	}



	public struct BuildingLocation
	{
		int x;
		int z;

		public BuildingLocation(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public int GetX() { return x; }
		public int GetZ() { return z; }
	}

	public struct ivec2
	{
		public int x;
		public int z;

		public ivec2(int x, int z)
		{
			this.x = x;
			this.z = z;
		}
	}

	public struct BuildingFootprint
	{
		public ivec2[] footprint;

		public BuildingFootprint(ivec2[] footprint)
		{
			this.footprint = footprint;
		}
	}

	public class Building : MonoBehaviour
	{
		public enum BuidlingType
		{
			Null = 0,
			RecRoom,
			Habitat,
			CommCenter,
			Cafeteria,
			Hallway,
			Outpost,
			Lander,
			Bunker,
			VehicleBay,
			Greenhouse,
			BlastFurnace,
			RegolithProcessor,
			HeliumExtractor,
			ShaftMine,
			Electrolyzer,
			RadarStation,
			MeteorDefense,
			Warehouse,
			FluidTank,
			BatteryBank,
			ResourcePile,
			SolarPanels,
			PowerLine,
			PowerPoles,
			SupplyCrates,
			OxygenRecharger,
			OxygenLine,
			Flag
		}


		//Set in the prefab instance so this building knows what type it is.
		public BuidlingType type;

		BuildingManager buildMan;

		public BuildingLocation location;

		private BuildingDetails details;

		public bool isBuilt;

		public void Awake()
		{
			if (type == BuidlingType.Null)
			{
				Debug.LogError("Building type not set!");
				Application.Quit();
			}
		}

		public virtual void Initialize(BuildingManager buildMan, BuildingLocation location)
		{
			this.buildMan = buildMan;
			this.location = location;
			details = BuildingManager.GetBuildingDetails(type);

			isBuilt = false;
		}

		public BuildingDetails GetDetails()
		{
			return details;
		}

		//Gets called once enough resources are brought here and workers work on it.
		public void Build()
		{
			isBuilt = true;
		}
	}
}
