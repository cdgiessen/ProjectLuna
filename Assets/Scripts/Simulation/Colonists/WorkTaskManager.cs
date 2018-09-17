using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
	public class WorkTaskManager : MonoBehaviour
	{
		private SimManager simMan;

		public WorkTaskManager Initialize(SimManager simMan)
		{
			this.simMan = simMan;
			return this;
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}