using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Simulation
{

	public struct CellLocation
	{
		int x;
		int z;
		ChunkLocation parent;

		public CellLocation(int x, int z, ChunkLocation parent)
		{
			this.x = x;
			this.z = z;
			this.parent = parent;
		}

		public int GetRelX()
		{
			return x;
		}
		public int GetRelZ()
		{
			return z;
		}
		public int GetAbsX()
		{
			return x + parent.GetAbsX();
		}
		public int GetAbsZ()
		{
			return z + parent.GetAbsZ();
		}

	}



	public class TerrainCell
	{
		CellLocation location;
		ResourceHolder cellResources;

		float RegolithAmount;
		bool containsRock;

		public float iron;
		public float ice;
		public float helium;

		public bool isOccupied;
		public bool isVisible;
		public bool isFlat;

		public TerrainCell(CellLocation location, TerrainGenerator terrainGenerator)
		{
			isVisible = false;
			isOccupied = false;
			this.location = location;

			cellResources = new ResourceHolder();
			RegolithAmount = terrainGenerator.GetRegolithHeight(location.GetAbsX(), location.GetAbsZ());
			containsRock = terrainGenerator.GetRegolithHeight(location.GetAbsX(), location.GetAbsZ()) > 0.9 ? true : false;

			iron = terrainGenerator.GetIronHeight(location.GetAbsX(), location.GetAbsZ());
			ice = terrainGenerator.GetIceHeight(location.GetAbsX(), location.GetAbsZ());
			helium = terrainGenerator.GetHeliumHeight(location.GetAbsX(), location.GetAbsZ());
		}

		public void UpdateIsFlat(bool value)
		{
			isFlat = value;
		}

		public float GetRegolithAmount()
		{
			return RegolithAmount;
		}

		public void AddRegolith(float amount)
		{
			RegolithAmount += amount;
		}

		public void RemoveRegolith(float amount)
		{
			RegolithAmount = Mathf.Max(RegolithAmount - amount, 0);
		}

		public void SetRegolithAmount(float value)
		{
			RegolithAmount = Mathf.Max(value, 0);
		}
	}
}
