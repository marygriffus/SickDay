using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Completed
{
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int pointsPerFood = 10;
		public int teacherRagePerEat = 5;
		public int teacherRageThreshold = 25;
		public Text coldText;
		public Text hallPassText;
		public Text teacherRageText;
		public AudioClip moveSound1;
		public AudioClip moveSound2;
		public AudioClip eatSound1;
		public AudioClip eatSound2;
		public AudioClip gameOverSound;
		
		private Animator animator;
		private int cold;
		private int hallPasses;
		private int teacherRage;
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif
		

		protected override void Start () {
			animator = GetComponent<Animator>();

			cold = GameManager.instance.playerColdPoints;
			hallPasses = GameManager.instance.hallPasses;
			teacherRage = 0;

			coldText.text = "Cold level: " + cold;
			hallPassText.text = "Hall passes: " + hallPasses;
			teacherRageText.text = "Teacher Rage: " + teacherRage;

			base.Start ();
		}
		

		private void OnDisable () {
			GameManager.instance.playerColdPoints = cold;
			GameManager.instance.hallPasses = hallPasses;
		}
		
		
		private void Update ()
		{
			if(!GameManager.instance.playersTurn) return;
			
			int horizontal = 0;
			int vertical = 0;
			
			//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			vertical = (int) (Input.GetAxisRaw ("Vertical"));

			if(horizontal != 0) {
				vertical = 0;
			}
			//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif //End of mobile platform dependendent compilation section started above with #elif
			//Check if we have a non-zero value for horizontal or vertical
			if (horizontal != 0 || vertical != 0) {
				AttemptMove<Wall> (horizontal, vertical);
			}
		}
			
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			//Every time player moves, subtract from food points total.
			cold--;
			
			//Update food text display to reflect current score.
			coldText.text = "Cold level: " + cold;

			base.AttemptMove <T> (xDir, yDir);

			RaycastHit2D hit;

			if (Move (xDir, yDir, out hit)) {
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}

			CheckIfGameOver ();

			GameManager.instance.playersTurn = false;
		}
		

		protected override void OnCantMove <T> (T component) {
			Wall hitWall = component as Wall;
			hitWall.RunIntoWall ();
		}
		

		private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if (other.tag == "Exit") {
				Invoke ("Restart", restartLevelDelay);
				
				//Disable the player object since level is over.
				enabled = false;
			}
			
			//Check if the tag of the trigger collided with is Food.
			else if (other.tag == "Food") {

				cold += pointsPerFood;
				teacherRage += teacherRagePerEat;
				coldText.text = "+" + pointsPerFood + " Cold level: " + cold;
				teacherRageText.text = "+" + teacherRagePerEat + " Teacher Rage: " + teacherRage;
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				other.gameObject.SetActive (false);
			}
		}
		
		
		//Restart reloads the scene when called.
		private void Restart () {
			if (teacherRage >= 25) {
				cold -= 5;
				hallPasses -= 1;
				coldText.text = "-"+ 5 + " Cold level: " + cold;
				GameManager.instance.messageForLevel = "You just got your nose wiped, mister! OUT!!!";
			} else {
				GameManager.instance.messageForLevel = "You got out before you got kicked out!";
			}
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		public void Sneeze () {

			animator.SetTrigger ("playerSneeze");
			cold -= 5;
			teacherRage += 2;
			coldText.text = "-"+ 5 + " Cold level: " + cold;
			teacherRageText.text = "-"+ 2 + " Teacher Rage level: " + teacherRage;
			CheckIfGameOver ();
		
		}

		public void Lick () {

			animator.SetTrigger ("playerLick");
			cold -= 1;
			teacherRage += 5;
			coldText.text = "-"+ 1 + " Cold level: " + cold;
			teacherRageText.text = "-"+ 5 + " Teacher Rage level: " + teacherRage;
			CheckIfGameOver ();

		}

		public void Spitball () {

			animator.SetTrigger ("playerSpitball");
			cold -= 3;
			teacherRage += 2;
			coldText.text = "-"+ 3 + " Cold level: " + cold;
			teacherRageText.text = "-"+ 2 + " Teacher Rage level: " + teacherRage;
			CheckIfGameOver ();

		}
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (cold <= 0 || hallPasses <= 0) {
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			} else if (teacherRage >= teacherRageThreshold) {
				Invoke ("Restart", restartLevelDelay);
			}
		}
	}
}

