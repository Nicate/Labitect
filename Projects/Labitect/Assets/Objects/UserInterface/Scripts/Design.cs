using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Design : MonoBehaviour {
	public GameObject[] designs;

	public GameObject icon;

	public GameObject inventoriesParent;
	public GameObject itemsParent;

	public float space;
	public float step;

	public Rect levelSize;

	private bool dragging;
	private GameObject draggedInventory;
	private GameObject draggedItem;

	/// <summary>
	/// Huge hack.
	/// </summary>
	private Vector3 nullVector;


	public void Start() {
		nullVector = new Vector3(levelSize.xMin * 100.0f, 0.0f, levelSize.yMin * 100.0f);

		float x = 0.0f;

		foreach(GameObject design in designs) {
			// Don't insert space at the beginning or end.
			if(x > 0.0f) {
				x += space;
			}

			Inventory inventory = design.GetComponent<Inventory>();
			Item item = design.GetComponent<Item>();

			if(inventory != null) {
				createIcon(x, inventory.image, (eventData) => {
					startDrag(inventory);
				});

				x += inventory.image.width;
			}
			else if(item != null) {
				createIcon(x, item.image, (eventData) => {
					startDrag(item);
				});

				x += item.image.width;
			}
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
	}
	
	private void createIcon(float x, Texture image, UnityAction<BaseEventData> startDragListener) {
		GameObject iconInstance = Instantiate(icon, transform) as GameObject;

		RawImage iconInstanceRawImage = iconInstance.GetComponent<RawImage>();
		iconInstanceRawImage.texture = image;

		RectTransform iconInstanceRectTransform = iconInstance.GetComponent<RectTransform>();
		iconInstanceRectTransform.localPosition = new Vector3(x, 0.0f, 0.0f);

		EventTrigger iconEventTrigger = iconInstance.AddComponent<EventTrigger>();

		EventTrigger.Entry startDragEntry = new EventTrigger.Entry();
		startDragEntry.eventID = EventTriggerType.PointerDown;
		startDragEntry.callback.AddListener(startDragListener);
		iconEventTrigger.triggers.Add(startDragEntry);

		EventTrigger.Entry dragEntry = new EventTrigger.Entry();
		dragEntry.eventID = EventTriggerType.Drag;
		dragEntry.callback.AddListener((eventData) => {
			drag();
		});
		iconEventTrigger.triggers.Add(dragEntry);

		EventTrigger.Entry stopDragEntry = new EventTrigger.Entry();
		stopDragEntry.eventID = EventTriggerType.PointerUp;
		stopDragEntry.callback.AddListener((eventData) => {
			stopDrag();
		});
		iconEventTrigger.triggers.Add(stopDragEntry);
	}


	public void left() {
		RectTransform rectTransform = GetComponent<RectTransform>();

		float x = rectTransform.localPosition.x + step;

		if(x <= 0.0f) {
			rectTransform.localPosition = new Vector3(x, 0.0f, 0.0f);
		}
	}

	public void right() {
		RectTransform rectTransform = GetComponent<RectTransform>();

		float x = rectTransform.localPosition.x - step;

		if(x + rectTransform.rect.width >= transform.parent.GetComponent<RectTransform>().rect.width) {
			rectTransform.localPosition = new Vector3(x, 0.0f, 0.0f);
		}
	}


	public void startDrag(Inventory inventory) {
		dragging = true;

		Vector3 position = getInteractionPositionInLevelOfNearestEmptySpace(null);
		Quaternion rotation = Quaternion.identity;

		draggedInventory = Instantiate(inventory.gameObject, position, rotation, inventoriesParent.transform) as GameObject;
		draggedItem = null;
	}

	public void startDrag(Item item) {
		dragging = true;
		
		Vector3 position = getInteractionPositionInLevelOnScreenOfNearestInventory();

		draggedInventory = null;
		draggedItem = Instantiate(item.gameObject, itemsParent.transform) as GameObject;

		RectTransform iconInstanceRectTransform = draggedItem.GetComponent<RectTransform>();
		iconInstanceRectTransform.position = position;
	}

	public void drag() {
		if(dragging) {
			if(draggedInventory != null) {
				draggedInventory.transform.position = getInteractionPositionInLevelOfNearestEmptySpace(draggedInventory.GetComponent<Inventory>());
			}
			else if(draggedItem != null) {
				RectTransform iconInstanceRectTransform = draggedItem.GetComponent<RectTransform>();
				iconInstanceRectTransform.position = getInteractionPositionInLevelOnScreenOfNearestInventory();
			}
		}
	}

	public void stopDrag() {
		dragging = false;

		if(draggedInventory != null) {
			if(isNullVector(draggedInventory.transform.position)) {
				Destroy(draggedInventory);
			}
		}
		else if(draggedItem != null) {
			Inventory inventory = getNearestInventory(getInteractionPosition());

			if(inventory == null || !inventory.hasSpace()) {
				Destroy(draggedItem);
			}
			else {
				inventory.inventory.Add(draggedItem.GetComponent<Item>());

				draggedItem.transform.SetParent(null);
			}
		}

		draggedInventory = null;
		draggedItem = null;
	}


	private Vector3 getInteractionPosition() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Plane plane = new Plane(Vector3.up, 0.0f);

		float distance;

		if(plane.Raycast(ray, out distance)) {
			return ray.origin + distance * ray.direction;
		}
		else {
			return Vector3.zero;
		}
	}

	private Vector3 getInteractionPositionInLevel() {
		Vector3 position = getInteractionPosition();

		position.x = Mathf.Round(position.x);
		position.z = Mathf.Round(position.z);

		if(position.x < levelSize.xMin) {
			position.x = levelSize.xMin;
		}
		else if(position.x > levelSize.xMax) {
			position.x = levelSize.xMax;
		}

		if(position.z < levelSize.yMin) {
			position.z = levelSize.yMin;
		}
		else if(position.z > levelSize.yMax) {
			position.z = levelSize.yMax;
		}

		return position;
	}


	private Vector3 getInteractionPositionOnScreen() {
		return Input.mousePosition;
	}

	private Vector3 getInteractionPositionInLevelOnScreen() {
		return Camera.main.WorldToScreenPoint(getInteractionPositionInLevel());
	}
	

	private Vector3 getInteractionPositionInLevelOfNearestEmptySpace(Inventory exclude) {
		Vector3 position = getInteractionPosition();

		float minimumDistance = float.MaxValue;
		Vector3 nearestEmptySpace = nullVector;

		for(int z = Mathf.RoundToInt(levelSize.yMin); z <= Mathf.RoundToInt(levelSize.yMax); z++) {
			for(int x = Mathf.RoundToInt(levelSize.xMin); x <= Mathf.RoundToInt(levelSize.xMax); x++) {
				Vector3 emptySpace = new Vector3(x, 0.0f, z);

				Inventory inventory = getNearestInventory(emptySpace, exclude);
				
				if(inventory == null || Vector3.Distance(inventory.transform.position, emptySpace) > 0.5f) {
					float distance = Vector3.Distance(position, emptySpace);

					if(distance < minimumDistance) {
						minimumDistance = distance;
						nearestEmptySpace = emptySpace;
					}
				}
			}
		}

		return nearestEmptySpace;
	}

	
	private Vector3 getInteractionPositionInLevelOnScreenOfNearestInventory() {
		Inventory nearestInventory = getNearestInventory(getInteractionPosition());

		if(nearestInventory != null) {
			return Camera.main.WorldToScreenPoint(nearestInventory.transform.position);
		}
		else {
			return Camera.main.WorldToScreenPoint(nullVector);
		}
	}


	private Inventory getNearestInventory(Vector3 position) {
		Inventory[] inventories = GameObject.FindObjectsOfType<Inventory>();

		float minimumDistance = float.MaxValue;
		Inventory nearestInventory = null;

		foreach(Inventory inventory in inventories) {
			float distance = Vector3.Distance(inventory.transform.position, position);

			if(distance < minimumDistance) {
				minimumDistance = distance;
				nearestInventory = inventory;
			}
		}

		return nearestInventory;
	}
	
	private Inventory getNearestInventory(Vector3 position, Inventory exclude) {
		Inventory[] inventories = GameObject.FindObjectsOfType<Inventory>();

		float minimumDistance = float.MaxValue;
		Inventory nearestInventory = null;

		foreach(Inventory inventory in inventories) {
			if(inventory != exclude) {
				float distance = Vector3.Distance(inventory.transform.position, position);

				if(distance < minimumDistance) {
					minimumDistance = distance;
					nearestInventory = inventory;
				}
			}
		}

		return nearestInventory;
	}


	private bool isNullVector(Vector3 vector) {
		return Vector3.Distance(nullVector, vector) < 1.0f;
	}
}
