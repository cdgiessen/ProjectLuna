using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
	public class SimManager : MonoBehaviour
	{
		//Terrain Generation Properties
		public float seed = 10;
		

		//Initial generated terrain parameters
		public int chunkSize = 32;
		public int chunkMapWidth = 4;
		public float TerrainHeightPerLevel = 1;
		public int TerrainNumLevels = 4;
		[Space]
		public int TerrainOctaves = 2;
		public float TerrainPersistence = 1;
		public float TerrainFrequency = 0.25f;
		public float TerrainAmplitude = 16.0f;
		[Space]
		public int IronOctaves = 2;
		public float IronPersistence = 0.8f;
		public float IronFrequency = 0.25f;
		public float IronAmplitude = 16.0f;
		[Space]
		public int IceOctaves = 2;
		public float IcePersistence = 0.8f;
		public float IceFrequency = 0.25f;
		public float IceAmplitude = 16.0f;
		[Space]
		public int HeliumOctaves = 2;
		public float HeliumPersistence = 0.8f;
		public float HeliumFrequency = 0.25f;
		public float HeliumAmplitude = 16.0f;
		[Space]
		public int RegolithOctaves = 2;
		public float RegolithPersistence = 0.8f;
		public float RegolithFrequency = 0.25f;
		public float RegolithAmplitude = 16.0f;
		[Space]
		public Material chunkMaterial;

		[HideInInspector]	public TerrainGenerator terrainGenerator;
		[HideInInspector]	public TerrainManager terrainManager;
		[HideInInspector]	public BuildingManager buildingManager;
		[HideInInspector]	public ColonistManager colonistManager;
		[HideInInspector]	public WorkTaskManager workTaskManager;


		//Awake
		private void Awake()
		{
			terrainGenerator = new TerrainGenerator(seed, new NoiseGeneratorSettings(TerrainOctaves, TerrainPersistence, TerrainFrequency, TerrainAmplitude, TerrainHeightPerLevel, TerrainNumLevels),
			new NoiseGeneratorSettings(IronOctaves, IronPersistence, IronFrequency, IronAmplitude),
			new NoiseGeneratorSettings(IceOctaves, IcePersistence, IceFrequency, IceAmplitude),
			new NoiseGeneratorSettings(HeliumOctaves, HeliumPersistence, HeliumFrequency, HeliumAmplitude),
			new NoiseGeneratorSettings(RegolithOctaves, RegolithPersistence, RegolithFrequency, RegolithAmplitude));

			terrainManager = new GameObject().AddComponent<TerrainManager>().Initialize(this, terrainGenerator, chunkSize, chunkMapWidth, chunkMaterial);
			terrainManager.transform.parent = transform;

			buildingManager = new GameObject().AddComponent<BuildingManager>().Initialize(this);
			buildingManager.transform.parent = transform;

			workTaskManager = new GameObject().AddComponent<WorkTaskManager>().Initialize(this);
			workTaskManager.transform.parent = transform;

			colonistManager = new GameObject().AddComponent<ColonistManager>().Initialize(this, workTaskManager);
			colonistManager.transform.parent = transform;


			AstarPath.active.Scan();
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


		//The cell borders are on the integers but their centers are at 0.5|1.5|2.5 etc, this takes a point and offsets it by 0.5 in the x and z dimentions
		public static Vector3 AdjustForGridOffset(Vector3 input)
		{
			return new Vector3(input.x + 0.5f, input.y, input.z + 0.5f);
		}
	}

}