using UnityEngine;
using UnityEngine.UI;

public class Design : MonoBehaviour {
	public GameObject[] designs;

	public GameObject icon;

	public float space;
	public float step;


	public void Start() {
		float x = 0.0f;
		
		foreach(GameObject design in designs) {
			// Don't insert space at the beginning or end.
			if(x > 0.0f) {
				 x += space;
			}

			Inventory inventory = design.GetComponent<Inventory>();
			Item item = design.GetComponent<Item>();
			
			if(inventory != null) {
				GameObject iconInstance = Instantiate(icon, transform) as GameObject;

				RawImage iconInstanceRawImage = iconInstance.GetComponent<RawImage>();
				iconInstanceRawImage.texture = inventory.image;

				RectTransform iconInstanceRectTransform = iconInstance.GetComponent<RectTransform>();
				iconInstanceRectTransform.localPosition = new Vector3(x, 0.0f, 0.0f);

				x += inventory.image.width;
			}
			else if(item != null) {
				GameObject iconInstance = Instantiate(icon, transform) as GameObject;

				RawImage iconInstanceRawImage = iconInstance.GetComponent<RawImage>();
				iconInstanceRawImage.texture = item.image;

				RectTransform iconInstanceRectTransform = iconInstance.GetComponent<RectTransform>();
				iconInstanceRectTransform.localPosition = new Vector3(x, 0.0f, 0.0f);

				x += item.image.width;
			}
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
	}


	public void left() {
		RectTransform rectTransform = GetComponent<RectTransform>();

		float x = rectTransform.position.x - step;

		if(x >= 0.0f) {
			rectTransform.position = new Vector3(x, 0.0f, 0.0f);
		}
	}

	public void right() {
		RectTransform rectTransform = GetComponent<RectTransform>();

		float x = rectTransform.position.x + step;

		if(x + rectTransform.sizeDelta.x <= GetComponentInParent<RectTransform>().sizeDelta.x) {
			rectTransform.position = new Vector3(x, 0.0f, 0.0f);
		}
	}
}
