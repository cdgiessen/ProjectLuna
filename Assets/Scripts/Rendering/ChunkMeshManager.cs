using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMeshManager : MonoBehaviour {

	private Simulation.SimManager simManager;

	public Material MeshMaterial;

	/*
	// Use this for initialization
	void Start () {
		GameObject sMan = GameObject.Find("SimulationManager");
		if(sMan == null)
		{
			Debug.LogError("Couldn't Find Simulation!");
		}
		simManager = sMan.GetComponent<Simulation.SimManager>();

		Dictionary<Simulation.ChunkLocation, Simulation.TerrainChunk> chunks = simManager.terrainManager.GetActiveChunks();

		foreach(Simulation.TerrainChunk chunk in chunks.Values)
		{
			GameObject newRenderChunk = new GameObject();
			newRenderChunk.transform.name = "Chunk Mesh";
			newRenderChunk.transform.parent = transform;
			newRenderChunk.transform.position = new Vector3(chunk.GetLocation().GetX()*simManager.chunkSize, 0 , chunk.GetLocation().GetZ() * simManager.chunkSize);
			newRenderChunk.AddComponent<ChunkMesh>().GenerateMesh(chunk.GetTerrainCells(), simManager.terrainManager, MeshMaterial);
			newRenderChunk.layer = gameObject.layer;
		}
	}
	*/
	// Update is called once per frame
	void Update () {
		
	}
}
