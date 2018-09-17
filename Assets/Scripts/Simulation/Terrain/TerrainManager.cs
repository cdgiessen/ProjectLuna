using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

namespace Simulation
{

	public class TerrainManager : MonoBehaviour
	{
		private SimManager simMan;

		TerrainChunk[,] activeChunks;

		TerrainGenerator terrainGenerator;

		int cellsPerChunk;
		int numberOfChunks;

		public TerrainManager Initialize(SimManager simMan, TerrainGenerator terrainGenerator, int cellsPerChunk, int numberOfChunks, Material chunkMaterial)
		{
			this.simMan = simMan;
			this.terrainGenerator = terrainGenerator;
			this.cellsPerChunk = cellsPerChunk;
			this.numberOfChunks = numberOfChunks;
			transform.name = "TerrainManager";

			activeChunks = new TerrainChunk[numberOfChunks, numberOfChunks];
			

			ChunkLocation[] locs = new ChunkLocation[numberOfChunks * numberOfChunks];
			for (int i = 0; i < numberOfChunks; i++)
			{
				for (int j = 0; j < numberOfChunks; j++)
				{
					locs[i * numberOfChunks + j] = new ChunkLocation(i, j, cellsPerChunk);
					activeChunks[i, j] = new GameObject().AddComponent<TerrainChunk>().Initialize(this, cellsPerChunk, locs[i * numberOfChunks + j], terrainGenerator, chunkMaterial);
					activeChunks[i, j].transform.parent = transform;
				}
			}	

			foreach(TerrainChunk chunk in activeChunks)
			{
				chunk.UpdateChunkMesh();
			}

			return this;
		}


		public void SetFootprintOccupied(BuildingLocation location, BuildingFootprint footprint, bool value)
		{
			for (int i = 0; i < footprint.footprint.Length; i++)
			{
				SetCellOccupied(location.GetX() + footprint.footprint[i].x, location.GetZ() + footprint.footprint[i].z, value);
			}
		}

		public void SetCellOccupied(int x, int z, bool value)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk * numberOfChunks || z >= cellsPerChunk * numberOfChunks)
			{
				Debug.Log("Specifid Position Out of bounds");
			}

			int chunkLocX = Mathf.FloorToInt(x / cellsPerChunk);
			int chunkLocZ = Mathf.FloorToInt(z / cellsPerChunk);
			int cellLocX = Mathf.RoundToInt(x % cellsPerChunk);
			int cellLocZ = Mathf.RoundToInt(z % cellsPerChunk);

			activeChunks[chunkLocX, chunkLocZ].GetTerrainCells()[cellLocX, cellLocZ].isOccupied = value;
		}

		public bool CheckIfFootprintOccupied(Vector3 point, BuildingFootprint footprint)
		{
			for (int i = 0; i < footprint.footprint.Length; i++)
			{
				if (CheckIfOccupied((int)point.x + footprint.footprint[i].x, (int)point.z + footprint.footprint[i].z))
					return true;
			}
			return false;
		}	

		public bool CheckIfOccupied(int x, int z)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk * numberOfChunks || z >= cellsPerChunk * numberOfChunks)
			{
				Debug.Log("Specifid Position Out of bounds");
				return true;
			}

			int chunkLocX = Mathf.FloorToInt(x / cellsPerChunk);
			int chunkLocZ = Mathf.FloorToInt(z / cellsPerChunk);
			int cellLocX = Mathf.RoundToInt(x % cellsPerChunk);
			int cellLocZ = Mathf.RoundToInt(z % cellsPerChunk);

			return activeChunks[chunkLocX, chunkLocZ].GetTerrainCells()[cellLocX,cellLocZ].isOccupied;
		}

		//Takes a 3d point and returns the position of the closest cell, with y being the height of the cell
		public Vector3 GetPointOnGrid(Vector3 inputPoint)
		{
			return new Vector3((int)inputPoint.x, GetTerrainCellHeight(inputPoint), (int)inputPoint.z);
		}

		public float GetTerrainCellHeight(Vector3 point)
		{
			return GetTerrainCellHeight((int)(point.x), (int)(point.z));
		}

		

		public float GetTerrainCellHeight(int x, int z)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk * numberOfChunks || z >= cellsPerChunk * numberOfChunks)
			{
				return 0;
			}

			int chunkLocX = Mathf.FloorToInt(x / cellsPerChunk);
			int chunkLocZ = Mathf.FloorToInt(z / cellsPerChunk);
			int cellLocX = Mathf.RoundToInt(x % cellsPerChunk);
			int cellLocZ = Mathf.RoundToInt(z % cellsPerChunk);

			return activeChunks[chunkLocX, chunkLocZ].GetCellHeight(cellLocX, cellLocZ);
		}

		public void ChangeTerrainCellHeight(Vector3 pos, int amount)
		{
			if (pos.x < 0 || pos.z < 0 || pos.x >= cellsPerChunk * numberOfChunks || pos.z >= cellsPerChunk * numberOfChunks)
			{
				Debug.Log("Can't change whats not in the grid");
				return;
			}

			int chunkLocX = Mathf.FloorToInt(pos.x / cellsPerChunk);
			int chunkLocZ = Mathf.FloorToInt(pos.z / cellsPerChunk);
			int cellLocX = Mathf.RoundToInt(pos.x % cellsPerChunk);
			int cellLocZ = Mathf.RoundToInt(pos.z % cellsPerChunk);

			float terrainChangeAmount = amount * terrainGenerator.GetHeightPerLevel() / terrainGenerator.GetNumberLevels();

			activeChunks[chunkLocX, chunkLocZ].ChangeCellHeight(cellLocX, cellLocZ, terrainChangeAmount);

			if (chunkLocX - 1 >= 0) {
				activeChunks[chunkLocX - 1, chunkLocZ].ChangeCellHeight(cellsPerChunk + cellLocX, cellLocZ, terrainChangeAmount);
			}
			if (chunkLocZ - 1 >= 0) {
				activeChunks[chunkLocX, chunkLocZ - 1].ChangeCellHeight(cellLocX, cellsPerChunk + cellLocZ, terrainChangeAmount);
			}
			if (chunkLocX + 1 < numberOfChunks)
			{
				activeChunks[chunkLocX + 1, chunkLocZ].ChangeCellHeight(cellLocX - cellsPerChunk, cellLocZ, terrainChangeAmount);
			}
			if (chunkLocZ + 1 < numberOfChunks) {
				activeChunks[chunkLocX, chunkLocZ + 1].ChangeCellHeight(cellLocX, cellLocZ - cellsPerChunk, terrainChangeAmount);
			}

			if (chunkLocX - 1 >= 0 && chunkLocZ - 1 >= 0)
			{
				activeChunks[chunkLocX - 1, chunkLocZ - 1].ChangeCellHeight(cellLocX + cellsPerChunk, cellLocZ + cellsPerChunk, terrainChangeAmount);
			}																											
			if (chunkLocX - 1 >= 0 && chunkLocZ + 1 < numberOfChunks)														
			{																											
				activeChunks[chunkLocX - 1, chunkLocZ + 1].ChangeCellHeight(cellLocX + cellsPerChunk, cellLocZ - cellsPerChunk, terrainChangeAmount);
			}																											
			if (chunkLocX + 1 < numberOfChunks && chunkLocZ - 1 >= 0)														
			{																											
				activeChunks[chunkLocX + 1, chunkLocZ - 1].ChangeCellHeight(cellLocX - cellsPerChunk, cellLocZ + cellsPerChunk, terrainChangeAmount);
			}																											
			if (chunkLocX + 1 < numberOfChunks && chunkLocZ + 1 < numberOfChunks)													
			{																											
				activeChunks[chunkLocX + 1, chunkLocZ + 1].ChangeCellHeight(cellLocX - cellsPerChunk, cellLocZ - cellsPerChunk, terrainChangeAmount);
			}

			//dirtyChunks.Enqueue(activeChunks[chunkLocX, chunkLocZ]);
			var guo = new GraphUpdateObject(activeChunks[chunkLocX, chunkLocZ].GetComponent<Collider>().bounds);
			
			AstarPath.active.UpdateGraphs(guo);
			
		}

		//Returns true if any of the cell is not flat
		public bool CheckIfFootprintFlat(Vector3 point, BuildingFootprint footprint)
		{
			for (int i = 0; i < footprint.footprint.Length; i++)
			{
				if (!CheckIfFlat((int)point.x + footprint.footprint[i].x, (int)point.z + footprint.footprint[i].z))
					return false;
			}
			return true;
		}

		public bool CheckIfFlat(int x, int z)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk * numberOfChunks || z >= cellsPerChunk * numberOfChunks)
			{
				Debug.Log("Can't change whats not in the grid");
				return false;
			}

			int chunkLocX = Mathf.FloorToInt(x / cellsPerChunk);
			int chunkLocZ = Mathf.FloorToInt(z / cellsPerChunk);
			int cellLocX = Mathf.RoundToInt(x % cellsPerChunk);
			int cellLocZ = Mathf.RoundToInt(z % cellsPerChunk);

			return activeChunks[chunkLocX, chunkLocZ].CheckIfCellIsFlat(cellLocX, cellLocZ);
		}

		public void Update()
		{
			//var guo = new GraphUpdateObject(dirtyChunks.Dequeue().GetComponent<Collider>().bounds);

			//AstarPath.active.UpdateGraphs(guo);
		}
	}
}