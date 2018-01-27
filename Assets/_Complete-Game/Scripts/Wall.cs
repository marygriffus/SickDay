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
		}


		public void RunIntoWall () {
			SoundManager.instance.RandomizeSfx (runSound1, runSound2);

			hp -= 1;

			if (hp <= 0) {
				spriteRenderer.sprite = dmgSprite;
				gameObject.SetActive (false);
			}
		}
	}
}
