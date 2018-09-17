using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
	public class Colonist : MonoBehaviour
	{
		ColonistManager colMan;

		Vector3 target;

		public Colonist Initialize(ColonistManager colMan)
		{
			this.colMan = colMan;
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

		public void GotoTarget(Vector3 target)
		{
			this.target = target;
			GetComponent<AIPath>().target = target;
		}
	}
}
