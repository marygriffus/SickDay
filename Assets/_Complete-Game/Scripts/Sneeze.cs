using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Sneeze : MovingObject
	{
		public AudioClip sneezeSound;


		protected void Start ()
		{
			base.Start ();
			MoveSneeze();
			Invoke ("Disappear", 2);
		}

		public void MoveSneeze () {
			AttemptMove <Classmate> (0, 0);
		}

		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			base.AttemptMove <T> (xDir, yDir);
		}


		protected override void OnCantMove <T> (T component) {
			Classmate classmate = component as Classmate;
			classmate.GetSick ();
		}

		protected void Disappear() {
			gameObject.SetActive (false);
		}
	}
}
