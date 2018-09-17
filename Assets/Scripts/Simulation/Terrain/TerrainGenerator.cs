using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct NoiseGeneratorSettings
{
	//Terrain generation settings.
	public int octaves;
	public float persistence;
	public float frequency;
	public float amplitude;
	public float heightPerLevel;
	public int numLevels;

	public NoiseGeneratorSettings(int octaves, float persistence, float frequency, float amplitude, float heightPerLevel, int numLevels)
	{
		this.octaves = octaves;
		this.persistence = persistence;
		this.frequency = frequency;
		this.amplitude = amplitude;
		this.heightPerLevel = heightPerLevel;
		this.numLevels = numLevels;
	}

	public NoiseGeneratorSettings(int octaves, float persistence, float frequency, float amplitude)
	{
		this.octaves = octaves;
		this.persistence = persistence;
		this.frequency = frequency;
		this.amplitude = amplitude;
		this.heightPerLevel = 1;
		this.numLevels = 1;
	}
}


public class TerrainGenerator
{
	//Overall generation seed;
	float seed = 10;

	private NoiseGeneratorSettings surfaceGenerator;
	private NoiseGeneratorSettings ironOreGenerator;
	private NoiseGeneratorSettings iceGenerator;
	private NoiseGeneratorSettings heliumGenerator;
	private NoiseGeneratorSettings regolithGenerator;
	private NoiseGeneratorSettings rockGenerator;


	private int BigAssOffset = 100000;

	public TerrainGenerator(float seed, NoiseGeneratorSettings surfaceGenerator, NoiseGeneratorSettings ironOreGenerator, 
		NoiseGeneratorSettings iceGenerator, NoiseGeneratorSettings heliumGenerator, NoiseGeneratorSettings regolithGenerator)
	{
		this.seed = seed;
		this.surfaceGenerator = surfaceGenerator;
		this.ironOreGenerator = ironOreGenerator;
		this.iceGenerator = iceGenerator;
		this.heliumGenerator = heliumGenerator;
		this.regolithGenerator = regolithGenerator;

		rockGenerator = new NoiseGeneratorSettings(1, 1, 1, 1);
	}

	//For terrain generation (stepped height changes, like a ladder)
	public float GetTerrainHeight(float x, float z)
	{
		return surfaceGenerator.heightPerLevel * Mathf.Floor(OctavePerlinNoise(x, z, surfaceGenerator) * surfaceGenerator.numLevels)/surfaceGenerator.numLevels;
	}

	//Smooth sampling of Iron Generator
	public float GetIronHeight(float x, float z)
	{
		return OctavePerlinNoise(x, z, ironOreGenerator);
	}

	//Smooth sampling of Ice Generator
	public float GetIceHeight(float x, float z)
	{
		return OctavePerlinNoise(x, z, iceGenerator);
	}

	//Smooth sampling of Helium Generator
	public float GetHeliumHeight(float x, float z)
	{
		return OctavePerlinNoise(x, z, heliumGenerator);
	}

	//Smooth sampling of Regolith Generator
	public float GetRegolithHeight(float x, float z)
	{
		return OctavePerlinNoise(x, z, regolithGenerator);
	}

	public float GetIfContainsRock(float x, float z)
	{
		return OctavePerlinNoise(x, z, rockGenerator);
	}

	public int GetNumberLevels()
	{
		return surfaceGenerator.numLevels;
	}

	public float GetHeightPerLevel()
	{
		return surfaceGenerator.heightPerLevel;
	}

	private float OctavePerlinNoise(float x, float z, NoiseGeneratorSettings noiseGenerator)
	{
		float total = 0.0f;
		float frequency = noiseGenerator.frequency;
		float amplitude = noiseGenerator.amplitude;
		float maxValue = 0.0f;  // Used for normalizing result to 0.0 - 1.0
		for (int i = 0; i < noiseGenerator.octaves; i++)
		{
			total += Mathf.PerlinNoise(BigAssOffset + 2*i +seed + x * frequency, BigAssOffset + 2 * i +seed + z * frequency) * amplitude;
			maxValue += amplitude;

			amplitude *= noiseGenerator.persistence;
			frequency *= 2;
		}
		return total/ maxValue;
	}
}
