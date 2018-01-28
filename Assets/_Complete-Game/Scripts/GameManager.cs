using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;

namespace Completed
{
	using System.Collections.Generic;
	using UnityEngine.UI;
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 3f;
		public float turnDelay = 0.01f;
		public static int numberOfLevels = 5;
		public int playerColdPoints = 75;
		public int hallPasses = 5;
		public int numberOfClassmates = 0;
		public int numberOfInfections = 0;
		public static GameManager instance = null;
		[HideInInspector] public bool playersTurn = true;
		public string messageForLevel = "";
		
		private Text levelText;


		private GameObject levelImage;
		private BoardManager boardScript;
		private int level = 0;
		private List<Classmate> classmates;
		private bool classmatesMoving;
		private bool doingSetup = false;
		

		void Awake()
		{
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);	
			DontDestroyOnLoad(gameObject);
			classmates = new List<Classmate>();

			boardScript = GetComponent<BoardManager>();
			InitGame();
		}


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
			if (!instance.doingSetup) {
				instance.InitGame ();
			} else {
				Thread.Sleep(100);
				instance.InitGame ();
			}
		}


		void InitGame() {

			doingSetup = true;

			levelImage = GameObject.Find("LevelImage");
			levelText = GameObject.Find("LevelText").GetComponent<Text>();

			level++;

			if (level == 1) {
				levelText.text = "Classroom " + level;
			} else if (level >= numberOfLevels) {
				double percentageInfected = (float)numberOfInfections / (float)numberOfClassmates;
				string noun = "Peon";
				if (percentageInfected > 0.9) {
					noun = "God";
				} else if (percentageInfected > 0.8) {
					noun = "King";
				} else if (percentageInfected > 0.7) {
					noun = "Duke";
				} else if (percentageInfected > 0.6) {
					noun = "Mayor";
				} else if (percentageInfected > 0.5) {
					noun = "Citizen";
				} else if (percentageInfected > 0.3) {
					noun = "Ratcatcher";
				} else if (percentageInfected > 0.2) {
					noun = "Rat";
				} else if (percentageInfected > 0.1) {
					noun = "Landowning Peasant";
				}
				levelText.text = "You completed all the levels! Congrats! You saw " + numberOfClassmates +
					" classmates and infected " + numberOfInfections + " of them, making you a "
					+ noun + " of Colds!";
				levelStartDelay = 30;
			} else {
				levelText.text = messageForLevel + " Classroom " + level;
			}
				
			IncrementThings (classmates);
			classmates.Clear();

			levelImage.SetActive(true);

			Invoke("HideLevelImage", levelStartDelay);
			Destroy (GameObject.Find("Board"));
			boardScript.SetupScene(level);
		}

		void IncrementThings(List<Classmate> mates) {
			for (int i = 0; i < mates.Count; i++) {
				numberOfClassmates ++;

				if (mates [i].isSick)
					numberOfInfections++; 
				Destroy (mates[i]);
			}
		}
		

		void HideLevelImage() {
			try {
				levelImage.SetActive(false);
				doingSetup = false;
			} catch (MissingReferenceException e) {
				Thread.SpinWait (100);
				doingSetup = false;
			}
		}


		void Update () {

			if (playersTurn || classmatesMoving || doingSetup)
				return;
			
			StartCoroutine (MoveClassmates ());
		}

		public void AddClassmateToList (Classmate script) {
			classmates.Add(script);
		}
		

		public void GameOver () {

			double percentageInfected = (float)numberOfInfections / (float)numberOfClassmates;
			string noun = "Peon";
			if (percentageInfected > 0.9) {
				noun = "God";
			} else if (percentageInfected > 0.8) {
				noun = "King";
			} else if (percentageInfected > 0.7) {
				noun = "Duke";
			} else if (percentageInfected > 0.6) {
				noun = "Mayor";
			} else if (percentageInfected > 0.5) {
				noun = "Citizen";
			} else if (percentageInfected > 0.3) {
				noun = "Ratcatcher";
			} else if (percentageInfected > 0.2) {
				noun = "Rat";
			} else if (percentageInfected > 0.1) {
				noun = "Landowning Peasant";
			}

			levelText.text = "You saw " + numberOfClassmates +
			" classmates and infected " + numberOfInfections + " of them, making you a "
			+ noun + " of Colds!";


			if (playerColdPoints <= 0) {
				levelText.text += " After " + level + " classrooms, you ran out sickness.";
			} else {
				levelText.text += " After " + level + " classrooms, you ran out of Hall Passes.";
			}

			levelImage.SetActive(true);

			enabled = false;
		}


		IEnumerator MoveClassmates () {
			if (!doingSetup) {
				classmatesMoving = true;

				yield return new WaitForSeconds (turnDelay);

				if (classmates.Count == 0) {
					yield return new WaitForSeconds (turnDelay);
				}

				for (int i = 0; i < classmates.Count; i++) {
					classmates [i].MoveClassmate ();

					yield return new WaitForSeconds (classmates [i].moveTime);
				}

				playersTurn = true;

				classmatesMoving = false;
			}
		}
	}
}

