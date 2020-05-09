using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	private Vector2 velocidade;
	public float smoothTimeX;
	public float smoothTimeY;

	public Texture2D cursorTexture;

	public GameObject player;

	void Start() {
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player) {
			float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocidade.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocidade.y, smoothTimeY);

			/*if (posX < 0.08f)
				posX = 0.08f;
			else if (posX > 4.38f)
				posX = 4.38f;

			if (posY < 0)
				posY = 0;
			else if (posY > 0.3f)
				posY = 0.3f;*/

			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}
}
