using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Physical resources
public class Resource {
	float amount;
	float density;

	public enum ResourceType
	{
		
		Iron = 1,
		Silicon = 2,
		Aluminium = 3,
		Copper = 4,
		Manganese = 5,
		Titanium = 6,
		Ice = 7,
		Helium = 8,
	}

	ResourceType type;

	public Resource(ResourceType type)
	{
		this.type = type;
		this.amount = 0;
	}

	public Resource(ResourceType type, float amount)
	{
		this.type = type;
		this.amount = amount;
	}

	public float GetAmount()
	{
		return amount;
	}

	public ResourceType GetResourceType()
	{
		return type;
	}

	public void AddToThis(float value)
	{
		amount += value;
	}

	public void RemoveFromThis(float value)
	{
		amount = Mathf.Max(amount - value, 0);
	}
}

