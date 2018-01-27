using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Wall : MonoBehaviour
	{
		public AudioClip runSound1;
		public AudioClip runSound2;


		void Awake () {
		}


		public void RunIntoWall () {
			SoundManager.instance.RandomizeSfx (runSound1, runSound2);
		}
	}
}
