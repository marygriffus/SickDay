using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count {
			public int minimum;
			public int maximum;

			public Count (int min, int max) {
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns;
		public int rows;

		public Count deskCount;
		public Count foodCount = new Count (1, 5);
		public Count classmateCount;

		public GameObject exit;
		public GameObject teacherTile;
		public GameObject[] floorTiles;
		public GameObject[] wallTiles;
		public GameObject[] deskTiles;
		public GameObject[] foodTiles;
		public GameObject[] classmateTiles;
		public GameObject[] outerWallTiles;
		
		private Transform boardHolder;
		private List <Vector3> gridPositions = new List <Vector3> ();

		void InitializeSize (int difficulty) {
			columns = 9;
			rows = 8;
			foodCount = new Count (3 - difficulty, 5 - difficulty);
			deskCount = new Count (3 + difficulty, 4 + difficulty);
			classmateCount = new Count(2 + difficulty, 3 + difficulty);
		}
		
		void InitializeList() {
			gridPositions.Clear();
			for (int x = 1; x < columns - 1; x++) {
				for (int y = 1; y < columns - 1; y++) {
					gridPositions.Add(new Vector3(x, y, 0f));
				}
			}
		}
		
		void BoardSetup() {
			boardHolder = new GameObject("Board").transform;
			for (int x = -1; x < columns + 1; x++) {
				for (int y = -1; y < columns + 1; y++) {
					GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
					if (x == columns - 1 && y == rows) {
						toInstantiate = exit;	
					} else if (x == -1 || x == columns || y == -1 || y == rows) {
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					}
					GameObject instance = Instantiate(toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

					instance.transform.SetParent(boardHolder);
				}
			}
		}
		
		Vector3 RandomPosition() {

			int randomIndex = Random.Range (0, gridPositions.Count);
			Vector3 randomPosition = gridPositions[randomIndex];
			gridPositions.RemoveAt (randomIndex);

			return randomPosition;
		}
		
		void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {

			int objectCount = Random.Range (minimum, maximum+1);

			for (int i = 0; i < objectCount; i++) {
				Vector3 randomPosition = RandomPosition();
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}

		public void SetupScene(int level) {

			int difficulty = (int)Mathf.Log (level, 2f);
			if (difficulty == 0)
				difficulty = 1;

			InitializeSize (difficulty);

			BoardSetup();
			InitializeList();

			LayoutObjectAtRandom (deskTiles, deskCount.minimum, deskCount.maximum);
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

			LayoutObjectAtRandom (classmateTiles, classmateCount.minimum, classmateCount.maximum);
			Vector3 teacherPosition = RandomPosition ();
			Instantiate (teacherTile, teacherPosition, Quaternion.identity);
		}
	}
}
