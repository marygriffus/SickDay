using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
	using System.Collections.Generic;
	using UnityEngine.UI;
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;
		public float turnDelay = 0.01f;
		public static int numberOfLevels = 10;
		public int playerColdPoints = 50;
		public int hallPasses = 5;
		public int numberOfClassmates = 0;
		public int numberOfInfections = 0;
		public static GameManager instance = null;
		[HideInInspector] public bool playersTurn = true;
		public string messageForLevel = "";
		
		private Text levelText;


		private GameObject levelImage;
		private BoardManager boardScript;
		private int level = 1;
		private List<Classmate> classmates;
		private bool classmatesMoving;
		private bool doingSetup = true;
		

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
			instance.level++;
			instance.InitGame ();

		}


		void InitGame() {
			doingSetup = true;

			levelImage = GameObject.Find("LevelImage");
			levelText = GameObject.Find("LevelText").GetComponent<Text>();

			if (level == 1) {
				levelText.text = "Classroom " + level;
			} else if (level == numberOfLevels) {
				double percentageInfected = (float)numberOfClassmates / (float)numberOfInfections;
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
					"classmates and infected " + numberOfInfections + " of them, making you a "
					+ noun + " of Colds!";
				levelStartDelay = 30;
			} else {
				levelText.text = messageForLevel + " Classroom " + level;
			}

			classmates.Clear();

			levelImage.SetActive(true);

			Invoke("HideLevelImage", levelStartDelay);

			boardScript.SetupScene(level);
		}
		

		void HideLevelImage() {

			levelImage.SetActive(false);
			doingSetup = false;
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
			if (playerColdPoints <= 0) {
				levelText.text = "After " + level + " classrooms, you ran out sickness. You are no God of Colds!";
			} else {
				levelText.text = "After " + level + " classrooms, you ran out of Hall Passes. You are no God of Colds!";
			}

			levelImage.SetActive(true);

			enabled = false;
		}


		IEnumerator MoveClassmates () {

			classmatesMoving = true;

			yield return new WaitForSeconds(turnDelay);

			if (classmates.Count == 0) {
				yield return new WaitForSeconds(turnDelay);
			}

			for (int i = 0; i < classmates.Count; i++) {
				classmates[i].MoveClassmate ();

				yield return new WaitForSeconds(classmates[i].moveTime);
			}

			playersTurn = true;

			classmatesMoving = false;
		}
	}
}

