using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
	public class ColonistManager : MonoBehaviour
	{
		private SimManager simMan;
		private WorkTaskManager taskMan;

		// Update is called once per frame
		void Update()
		{

		}

		public ColonistManager Initialize(SimManager simMan, WorkTaskManager taskMan)
		{
			this.simMan = simMan;
			this.taskMan = taskMan;
			transform.name = "Colonist Manager";
			return this;
		}
	}
}