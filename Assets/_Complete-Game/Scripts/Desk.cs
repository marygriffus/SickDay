using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Desk : Wall {

		public Sprite dmgSprite;
		
		private SpriteRenderer spriteRenderer;
		
		
		void Awake () {
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		new public void RunIntoWall () {

			spriteRenderer.sprite = dmgSprite;

			gameObject.SetActive (false);
		}
	}
}
