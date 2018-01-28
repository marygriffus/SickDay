using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Sneeze : MonoBehaviour
	{

		public AudioClip sneezeSound;


		protected void Start ()
		{

		}


		public void Hit (int xDir, int yDir)
		{

		}


		protected void AttemptHit (int xDir, int yDir) {

			SoundManager.instance.PlaySingle (sneezeSound);
			Hit (xDir, yDir);
		}
	}
}
