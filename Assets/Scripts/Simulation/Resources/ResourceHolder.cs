using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory of resources.
public class ResourceHolder {

	Dictionary<Resource.ResourceType, Resource> resources;

	public ResourceHolder()
	{
		resources = new Dictionary<Resource.ResourceType, Resource>();
	}

	public void AddResource(Resource.ResourceType type, float amount)
	{
		Resource value;
		if (resources.TryGetValue(type, out value))
		{
			value.AddToThis(amount);
		}
		else
		{
			resources.Add(type, new Resource(type, amount));
		}
	}

	public void AddResource(Resource resource)
	{
		AddResource(resource.GetResourceType(), resource.GetAmount());
	}

	public void RemoveResource(Resource.ResourceType type, float amount)
	{
		Resource value;
		if (resources.TryGetValue(type, out value))
		{
			value.RemoveFromThis(amount);
		}
	}

	public void RemoveResource(Resource resource)
	{
		RemoveResource(resource.GetResourceType(), resource.GetAmount());
	}
}
