using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkMesh : MonoBehaviour
{

	Mesh mesh;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GenerateMesh(Simulation.TerrainCell[,] cellHeights, Simulation.TerrainManager terMan, Material MeshMaterial)
	{
		Vector3 tp = transform.position;

		int numVerts = (cellHeights.GetLength(0) + 1) * (cellHeights.GetLength(1) + 1) * 6;
		int numTriangles = (cellHeights.GetLength(0) + 1) * (cellHeights.GetLength(1) + 1) * 6;

		Vector3[] verts = new Vector3[numVerts];
		int[] triangles = new int[numTriangles];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		int counter = 0;
		for (int i = 0; i < cellHeights.GetLength(0); i++)
		{
			for (int j = 0; j < cellHeights.GetLength(1); j++)
			{
				float cellHeight = 0;// cellHeights[i, j].GetHeight();

				float downCell = 0, upCell = 0, rightCell = 0, leftCell = 0;
				float downRightCell = 0, downLeftCell = 0, upRightCell = 0, upLeftCell = 0;

				if (i == 0 || i == cellHeights.GetLength(0) - 1 || j == 0 || j == cellHeights.GetLength(1) - 1)
				{
					//downCell = terMan.GetHeightAtCell(i + 1 + tp.x, j + tp.z);
					//upCell = terMan.GetHeightAtCell(i - 1 + tp.x, j + tp.z);
					//rightCell = terMan.GetHeightAtCell(i + tp.x, j + 1 + tp.z);
					//leftCell = terMan.GetHeightAtCell(i + tp.x, j - 1 + tp.z);
					//
					//downRightCell = terMan.GetHeightAtCell(i + 1 + tp.x, j + 1 + tp.z);
					//downLeftCell = terMan.GetHeightAtCell(i + 1 + tp.x, j - 1 + tp.z);
					//upRightCell = terMan.GetHeightAtCell(i - 1 + tp.x, j + 1 + tp.z);
					//upLeftCell = terMan.GetHeightAtCell(i - 1 + tp.x, j - 1 + tp.z);
				}
				else
				{

					//downCell = cellHeights[i + 1, j].GetHeight();
					//upCell = cellHeights[i - 1, j].GetHeight();
					//rightCell = cellHeights[i, j + 1].GetHeight();
					//leftCell = cellHeights[i, j - 1].GetHeight();
					//
					//downRightCell = cellHeights[i + 1, j + 1].GetHeight();
					//downLeftCell = cellHeights[i + 1, j - 1].GetHeight();
					//upRightCell = cellHeights[i - 1, j + 1].GetHeight();
					//upLeftCell = cellHeights[i - 1, j - 1].GetHeight();
				}
				float topRightCorner = Mathf.Max(cellHeight, downCell, rightCell, downRightCell); ;
				float topLeftCorner = Mathf.Max(cellHeight, downCell, leftCell, downLeftCell);
				float bottomRightCorner = Mathf.Max(cellHeight, upCell, rightCell, upRightCell); ;
				float bottomLeftCorner = Mathf.Max(cellHeight, upCell, leftCell, upLeftCell); ;


				if (topRightCorner == bottomLeftCorner)
				{
					verts[counter] = new Vector3(i, bottomLeftCorner, j);
					verts[counter + 1] = new Vector3(i, bottomRightCorner, j + 1);
					verts[counter + 2] = new Vector3(i + 1, topRightCorner, j + 1);

					verts[counter + 3] = new Vector3(i, bottomLeftCorner, j);
					verts[counter + 4] = new Vector3(i + 1, topRightCorner, j + 1);
					verts[counter + 5] = new Vector3(i + 1, topLeftCorner, j);

					Vector3 bL = new Vector3(i, bottomLeftCorner, j);
					Vector3 bR = new Vector3(i, bottomRightCorner, j + 1);
					Vector3 tL = new Vector3(i + 1, topLeftCorner, j);
					Vector3 tR = new Vector3(i + 1, topRightCorner, j + 1);

					Vector3 upperCross = Vector3.Cross(tR - bR, bL - bR).normalized;
					Vector3 lowerCross = Vector3.Cross(bL - tL, tR - tL).normalized;

					normals[counter] = normals[counter + 1] = normals[counter + 2] = upperCross;
					normals[counter + 3] = normals[counter + 4] = normals[counter + 5] = lowerCross;

					uv[counter] = new Vector2(i, j);
					uv[counter + 1] = new Vector2(i, j + 1);
					uv[counter + 2] = new Vector2(i + 1, j + 1);

					uv[counter + 3] = new Vector2(i, j);
					uv[counter + 4] = new Vector2(i + 1, j + 1);
					uv[counter + 5] = new Vector2(i + 1, j);
				}
				else
				{
					verts[counter] = new Vector3(i, bottomLeftCorner, j);
					verts[counter + 1] = new Vector3(i, bottomRightCorner, j + 1);
					verts[counter + 2] = new Vector3(i + 1, topLeftCorner, j);

					verts[counter + 3] = new Vector3(i, bottomRightCorner, j + 1);
					verts[counter + 4] = new Vector3(i + 1, topRightCorner, j + 1);
					verts[counter + 5] = new Vector3(i + 1, topLeftCorner, j);

					Vector3 bL = new Vector3(i, bottomLeftCorner, j);
					Vector3 bR = new Vector3(i, bottomRightCorner, j + 1);
					Vector3 tL = new Vector3(i + 1, topLeftCorner, j);
					Vector3 tR = new Vector3(i + 1, topRightCorner, j + 1);

					Vector3 upperCross = Vector3.Cross(tL - tR, bR - tR).normalized;
					Vector3 lowerCross = Vector3.Cross(bR - bL, tL - bL).normalized;

					normals[counter] = normals[counter + 1] = normals[counter + 2] = lowerCross;
					normals[counter + 3] = normals[counter + 4] = normals[counter + 5] = upperCross;

					uv[counter] = new Vector2(i, j);
					uv[counter + 1] = new Vector2(i, j + 1);
					uv[counter + 2] = new Vector2(i + 1, j);

					uv[counter + 3] = new Vector2(i, j + 1);
					uv[counter + 4] = new Vector2(i + 1, j + 1);
					uv[counter + 5] = new Vector2(i + 1, j);
				}
				counter += 6;
			}
		}

		for (int i = 0; i < (cellHeights.GetLength(0) + 1) * (cellHeights.GetLength(1) + 1) * 6; i++)
		{
			triangles[i] = i; //no vertex saving, each triangle gets its own three vertices, in order 
		}

		mesh = new Mesh();
		mesh.name = "Chunk Mesh";
		mesh.vertices = verts;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshRenderer>().material = MeshMaterial;
		GetComponent<MeshCollider>().sharedMesh = mesh;

		
		//OptemizeMesh(mesh, MeshMaterial);
	}

	//Hot piece of garbage that could work but I'm to lazy to find out why it does nothing and crashes.
	public void OptemizeMesh(Mesh mesh_in, Material MeshMaterial)
	{
		Vector3 tp = transform.position;

		Mesh mesh_out = new Mesh();
		mesh_out.name = "Optemized Chunk Mesh";
		mesh_out.vertices = new Vector3[mesh_in.vertices.Length];
		mesh_out.triangles = new int[mesh_in.triangles.Length];
		mesh_out.normals = new Vector3[mesh_in.normals.Length];
		mesh_out.uv = new Vector2[mesh_in.uv.Length];

		FillMeshAtIndexFullQuad(mesh_in, mesh_out, 0, 0);

		float prev_UpLeft = mesh_in.vertices[mesh.triangles[0]].y;
		float prev_UpRight = mesh_in.vertices[mesh.triangles[1]].y;
		float prev_DownLeft = mesh_in.vertices[mesh.triangles[4]].y;
		float prev_DownRight = mesh_in.vertices[mesh.triangles[5]].y;
		
		bool prev_isFlat = false;
		if (prev_UpLeft == prev_UpRight && prev_UpRight == prev_DownLeft && prev_DownLeft == prev_DownRight)
			prev_isFlat = true;

		int cur_Index = 1;
		for (int i = 1; i < mesh_in.vertices.Length/6; i++)
		{
			float cur_UpLeft = mesh_in.vertices[mesh.triangles[i * 6 + 0]].y;
			float cur_UpRight = mesh_in.vertices[mesh.triangles[i * 6 + 1]].y;
			float cur_DownLeft = mesh_in.vertices[mesh.triangles[i * 6 + 4]].y;
			float cur_DownRight = mesh_in.vertices[mesh.triangles[i * 6 + 5]].y;

			bool cur_isFlat = false;
			if (prev_isFlat && cur_UpLeft == cur_UpRight && cur_UpRight == cur_DownLeft && cur_DownLeft == cur_DownRight && prev_UpLeft == cur_UpLeft)
			{
				cur_isFlat = true;

				//Combine previous triangle with current one
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (cur_Index - 1) * 6 + 0,	(cur_Index - 1) * 6 + 0);
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (cur_Index - 1) * 6 + 1,	(cur_Index - 1) * 6 + 1);
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (i) * 6 + 2,		(cur_Index - 1) * 6	+ 2);
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (cur_Index - 1) * 6 + 3,	(cur_Index - 1) * 6 + 3);
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (i) * 6 + 4,		(cur_Index - 1) * 6	+ 4);
				FillMeshAtIndexSingleVertex(mesh_in, mesh_out, (i) * 6 + 5,		(cur_Index - 1) * 6	+ 5);

				cur_UpLeft = prev_UpLeft;
				cur_UpRight =	prev_UpRight;
				cur_DownLeft =	prev_DownLeft;
				cur_DownRight = prev_DownRight;
			}
			else
			{
				FillMeshAtIndexFullQuad(mesh_in, mesh_out, cur_Index * 6, i*6);
				cur_Index++;
			}
		}


		GetComponent<MeshFilter>().mesh = mesh_out;
		GetComponent<MeshRenderer>().material = MeshMaterial;
	}

	private void FillMeshAtIndexFullQuad(Mesh mesh_in, Mesh mesh_out, int index_from, int index_to)
	{
		for (int i = 0; i < 6; i++)
		{
			mesh_out.vertices[index_to + i] = mesh_in.vertices[index_from + i];
			//Debug.Log(mesh_out.vertices[index_to + i] + "	" + mesh_in.vertices[index_from + i]);
			mesh_out.triangles[index_to + i] = mesh_in.triangles[index_from + i];
			mesh_out.normals[index_to + i] =	mesh_in.uv[index_from + i];
			mesh_out.uv[index_to + i] =		mesh_in.uv[index_from + i];
		}
	}

	private void FillMeshAtIndexSingleVertex(Mesh mesh_in, Mesh mesh_out, int index_from, int index_to)
	{		
		mesh_out.vertices[index_from ] =	mesh_in.vertices[index_to ];
		mesh_out.triangles[index_from ] =	mesh_in.triangles[index_to ];
		mesh_out.normals[index_from ] =		mesh_in.uv[index_to ];
		mesh_out.uv[index_from ] =			mesh_in.uv[index_to ];	
	}
}