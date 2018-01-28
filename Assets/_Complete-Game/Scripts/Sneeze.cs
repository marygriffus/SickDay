using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Sneeze : MonoBehaviour
	{
		public AudioClip sneezeSound;


		protected void Start ()
		{
			Invoke ("Disappear", 2);
		}

		protected void Disappear() {
			gameObject.SetActive (false);
		}

	}
}
