using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Completed
{

	public class Classmate : MovingObject
	{
		public bool isSick;
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		

		protected override void Start () { 

			GameManager.instance.AddClassmateToList (this);
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}

		protected override void AttemptMove <T> (int xDir, int yDir) {
			//Check if skipMove is true, if so set it to false and skip this turn.
			if (skipMove) {
				skipMove = false;
				return;
			}
			
			//Call the AttemptMove function from MovingObject.
			base.AttemptMove <T> (xDir, yDir);
			
			//Now that Enemy has moved, set skipMove to true to skip next move.
			skipMove = true;
		}
		

		public void MoveClassmate () {
			int xDir = (int)Random.Range (-1, 2);
			int yDir = (int)Random.Range (-1, 2);
			
			AttemptMove <Wall> (xDir, yDir);
		}
		

		protected override void OnCantMove <T> (T component) {

			Wall hitWall = component as Wall;
			hitWall.RunIntoWall (0);
		}

		public void GetSick () {
			isSick = true;
		}

		private void OnTriggerEnter2D (Collider2D other)
		{
			GetSick ();
		}
	}
}
