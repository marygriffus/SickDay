using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Completed
{

	public class Classmate : MovingObject
	{
		public bool isSick;
		public AudioClip greetSound1;
		public AudioClip greetSound2;
		
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		

		protected override void Start ()
		{ 

			GameManager.instance.AddClassmateToList (this);
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		

		protected override void AttemptMove <T> (int xDir, int yDir)
		{
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
			int xDir = Random.Range (-1, 1);
			int yDir = Random.Range (-1, 1);
			
			AttemptMove <Player> (xDir, yDir);
		}
		
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component) {
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			animator.SetTrigger ("classmateGreet");

			SoundManager.instance.RandomizeSfx (greetSound1, greetSound2);
		}

		public bool GetSick () {
			if (!isSick) {
				isSick = true;
				return isSick;
			} else {
				return false;
			}
		}
	}
}
