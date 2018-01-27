using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Wall : MonoBehaviour
	{
		public AudioClip runSound1;
		public AudioClip runSound2;
		public Sprite dmgSprite;
		private SpriteRenderer spriteRenderer;
		public int hp;


		void Awake () {
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}


		public void RunIntoWall (int damage) {
			SoundManager.instance.RandomizeSfx (runSound1, runSound2);

			hp -= damage;

			if (hp <= 0) {
				spriteRenderer.sprite = dmgSprite;
				gameObject.SetActive (false);
			}
		}
	}
}
