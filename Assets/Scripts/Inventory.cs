using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<GameObject> ObjectsList = new List<GameObject>();
    public GameObject[] prefabs;
    float timer = 4f;
    public LayerMask mask;
    private Vector3 mOffset;
    private bool MouseDragging = false;
    private float mZCoord;
    RaycastHit objMoved = new RaycastHit();
    GameObject objOver = null;
    public manaManager mMana;

    void OnMouseDown(RaycastHit hit)
    {
        mZCoord = Camera.main.WorldToScreenPoint(hit.transform.position).z;

        mOffset = hit.transform.position - GetMouseAsWorldPoint();
        MouseDragging = true;

    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    void OnMouseDrag()
    {
        objMoved.transform.position = GetMouseAsWorldPoint() + mOffset;
        MouseDragging = true;
    }


    void OnMouseUp()
    {
        MouseDragging = false;
        if (objMoved.transform.position.x > -45f && objMoved.transform.position.x < 4.5f && objMoved.transform.position.z > -4.5f && objMoved.transform.position.z < 4.5f) SuppObj(objMoved.transform.gameObject);
        InventoryUpdate();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            if (ObjectsList.Count < 15) AddObj(prefabs[Random.Range(0, 2)]);
            timer = 0;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask) && !MouseDragging)
        {
            objOver = hit.transform.gameObject;
            objMoved = hit;
            if (Input.GetMouseButtonDown(0) && !MouseDragging) OnMouseDown(hit);
        }
        if (MouseDragging)
        {
            if (Input.GetMouseButton(0)) OnMouseDrag();
            if (Input.GetMouseButtonUp(0)) OnMouseUp();
        }
    }

    void AddObj(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        ObjectsList.Add(obj);
        InventoryUpdate();
    }

    void SuppObj(GameObject obj)
    {
        if (obj.tag == "Malus") mMana.LoseMana(0.1f);
        else mMana.GainMana(0.1f);
        ObjectsList.Remove(obj);
        Destroy(obj);
        InventoryUpdate();
    }

    void InventoryUpdate()
    {
        Vector2 posStart = new Vector2(-4, -6);
        foreach (GameObject obj in ObjectsList)
        {
            if (MouseDragging)
            {
                if (obj != objMoved.transform.gameObject)
                {

                    obj.transform.position = new Vector3(posStart.x, 0, posStart.y);
                    posStart.x += 2;
                    if (posStart.x > 4)
                    {
                        posStart.x = -4;
                        posStart.y -= 2;
                    }
                }
            }
            else
            {
                if (obj.transform.position.y == 1) obj.transform.position = new Vector3(posStart.x, 1, posStart.y);
                else obj.transform.position = new Vector3(posStart.x, 0, posStart.y);
                posStart.x += 2;
                if (posStart.x > 4)
                {
                    posStart.x = -4;
                    posStart.y -= 2;
                }
                
            }
            
        }
    }
}
