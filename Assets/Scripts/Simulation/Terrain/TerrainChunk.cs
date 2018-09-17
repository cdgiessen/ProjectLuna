using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Simulation
{
	public struct ChunkLocation
	{
		int x;
		int z;
		private int cellsPerChunk;
		public ChunkLocation(int x, int z, int cellsPerChunk)
		{
			this.x = x;
			this.z = z;
			this.cellsPerChunk = cellsPerChunk;
		}

		public override int GetHashCode()
		{
			return (x * 0x1f1f1f1f) ^ z;
		}

		public int GetX() {	return x; }
		public int GetAbsX() { return x * cellsPerChunk; }
		public int GetZ() {	return z; }
		public int GetAbsZ() { return z * cellsPerChunk; }
	}

	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class TerrainChunk : MonoBehaviour
	{
		private TerrainManager terrainManager;

		ChunkLocation location;

		int cellsPerChunk;
		TerrainCell[,] chunkCells;
		float[,] cellHeights;

		Mesh m_mesh;
		Material m_material;

		public TerrainChunk Initialize(TerrainManager terrainManager, int cellsPerChunk, ChunkLocation location, TerrainGenerator terrainGenerator, Material chunkMaterial)
		{
			this.terrainManager = terrainManager;

			transform.position = new Vector3(location.GetX() * cellsPerChunk, 0, location.GetZ() * cellsPerChunk);
			transform.name = "Chunk";
			gameObject.layer = 8;

			this.location = location;
			this.cellsPerChunk = cellsPerChunk;

			cellHeights = new float[cellsPerChunk + 1, cellsPerChunk + 1];
			for (int i = 0; i < cellsPerChunk + 1; i++)
			{
				for (int j = 0; j < cellsPerChunk + 1; j++)
				{
					cellHeights[i,j] = terrainGenerator.GetTerrainHeight(i + location.GetAbsX(), j  + location.GetAbsZ());
				}
			}

			chunkCells = new TerrainCell[cellsPerChunk, cellsPerChunk];
			for (int i = 0; i < cellsPerChunk; i++)
			{
				for (int j = 0; j < cellsPerChunk; j++)
				{
					CellLocation loc = new CellLocation(i, j, location);

					chunkCells[i, j] = new TerrainCell(loc, terrainGenerator);
				}
			}

			m_material = chunkMaterial;
			GetComponent<MeshRenderer>().material = m_material;

			return this;
		}

		public void UpdateChunkMesh()
		{
			m_mesh = GenChunkByVertex(cellsPerChunk, "Terrain Chunk");
			GetComponent<MeshFilter>().mesh = m_mesh;
			GetComponent<MeshCollider>().sharedMesh = m_mesh;
		}


		public void ChangeCellHeight(int x, int z, float amount)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk + 1 || z >= cellsPerChunk + 1)
			{
				//Debug.Log("Specifid Cell Position out of bounds");
				return;
			}
			cellHeights[x, z] = Math.Max(cellHeights[x, z] + amount, 0);

			UpdateChunkMesh();
		}

		public ChunkLocation GetLocation()
		{
			return location;
		}

		public TerrainCell[,] GetTerrainCells()
		{
			return chunkCells;
		}

		public float GetCellHeight(int x, int z)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk + 1 || z >= cellsPerChunk + 1)
			{
				//Debug.Log("Specifid Cell Position out of bounds");
				return -1;
			}
			return cellHeights[x, z];
		}

		public bool CheckIfCellIsFlat(int x, int z)
		{
			if (x < 0 || z < 0 || x >= cellsPerChunk || z >= cellsPerChunk)
			{
				//Debug.Log("Specifid Cell Position out of bounds");
				return false;
			}
			return chunkCells[x, z].isFlat;
		}

		static Color nothing = new Color(1f, 0f, 0f);
		static Color iron = new Color(1f, 0f, 0f);
		static Color ice = new Color(0f, 1f, 0f);
		static Color helium = new Color(0f, 0f, 1f);

		Mesh GenChunkByVertex(int numCells, string name)
		{
			int numVerts = (numCells + 1) * (numCells + 1) * 6;
			int numTriangles = (numCells + 1) * (numCells + 1) * 6;
			Vector3[] verts = new Vector3[numVerts];
			int[] triangles = new int[numTriangles];
			Vector3[] normals = new Vector3[numVerts];
			Vector2[] uv = new Vector2[numVerts];
			Color[] colors = new Color[numVerts];

			int counter = 0;
			for (int i = 0; i < numCells; i++)
			{
				for (int j = 0; j < numCells; j++)
				{
					TerrainCell curCell = chunkCells[i, j];
					float regolith = curCell.GetRegolithAmount();

					Vector3 uL = new Vector3(i, cellHeights[i, j], j);
					Vector3 uR = new Vector3(i, cellHeights[i, j + 1], j + 1);
					Vector3 lL = new Vector3(i + 1, cellHeights[i + 1, j], j);
					Vector3 lR = new Vector3(i + 1, cellHeights[i + 1, j + 1], j + 1);

					curCell.UpdateIsFlat(cellHeights[i, j] == cellHeights[i, j + 1] && cellHeights[i, j + 1] == cellHeights[i + 1, j] && cellHeights[i + 1, j] == cellHeights[i + 1, j + 1]);


					if (Mathf.Abs(uL.y - lR.y) < Mathf.Abs(uR.y - lL.y))
					{
						verts[counter] = uL;
						verts[counter + 1] = uR;
						verts[counter + 2] = lR;

						verts[counter + 3] = uL;
						verts[counter + 4] = lR;
						verts[counter + 5] = lL;

						Vector3 a = uL - uR;
						Vector3 b = lR - uR;
						Vector3 abCross = Vector3.Cross(b, a).normalized;

						Vector3 c = uL - lL;
						Vector3 d = lR - lL;
						Vector3 cdCross = Vector3.Cross(c, d).normalized;

						normals[counter] = normals[counter + 1] = normals[counter + 2] = abCross;
						normals[counter + 3] = normals[counter + 4] = normals[counter + 5] = cdCross;

						uv[counter] = new Vector2(0, 0);
						uv[counter + 1] = new Vector2(0, 1);
						uv[counter + 2] = new Vector2(1, 1);

						uv[counter + 3] = new Vector2(0, 0);
						uv[counter + 4] = new Vector2(1, 1);
						uv[counter + 5] = new Vector2(1, 0);

					}
					else
					{
						verts[counter] = uL;
						verts[counter + 1] = uR;
						verts[counter + 2] = lL;

						verts[counter + 3] = uR;
						verts[counter + 4] = lR;
						verts[counter + 5] = lL;

						Vector3 a = uL - uR;
						Vector3 b = lL - uR;
						Vector3 abCross = Vector3.Cross(b, a).normalized;

						Vector3 c = uR - lR;
						Vector3 d = lL - lR;
						Vector3 cdCross = Vector3.Cross(d, c).normalized;

						normals[counter] = normals[counter + 1] = normals[counter + 2] = abCross;
						normals[counter + 3] = normals[counter + 4] = normals[counter + 5] = cdCross;

						uv[counter]		= new Vector2(0, 0);
						uv[counter + 1] = new Vector2(0, 1);
						uv[counter + 2] = new Vector2(1, 0);

						uv[counter + 3] = new Vector2(0, 1);
						uv[counter + 4] = new Vector2(1, 1);
						uv[counter + 5] = new Vector2(1, 0);
					}

					/*if (curCell.iron > curCell.ice && curCell.iron > curCell.helium)
						for (int a = counter; a < 6; a++)
							colors[a] = iron;
					else if (curCell.ice > curCell.iron && curCell.ice > curCell.helium)
						for (int a = counter; a < 6; a++)
							colors[a] = ice;
					else if (curCell.helium > curCell.iron && curCell.helium > curCell.ice)
						for (int a = counter; a < 6; a++)
							colors[a] = helium;
					else*/
						for (int a = counter; a < 6; a++)
							colors[a] = ice;
					
					counter += 6;
				}
			}

			for (int i = 0; i < numCells * numCells * 6; i++)
			{
				triangles[i] = i; //no vertex saving, each triangle gets its own three vertices, in order 
			}

			Mesh mesh = new Mesh();
			mesh.name = name;
			mesh.vertices = verts;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.colors = colors;

			return mesh;
		}
	}
}