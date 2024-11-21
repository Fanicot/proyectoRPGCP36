using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventarioJugador : MonoBehaviour
{

    [System.Serializable]
    public struct ObjetoInvId
    {
        public int id;
        public int cantidad;
        public ObjetoInvId(int id, int cantidad)
        {
            this.id = id; ;
            this.cantidad = cantidad;
        }
    }

    [SerializeField]
    DataBase data;
    [Header("Variables del Drag and Drop")]
    public GraphicRaycaster raycaster;
    private PointerEventData pointedData;
    private List<RaycastResult> raycastResults;
    public static RectTransform canvas;
    [SerializeField]
    private GameObject ObjetoSeleccionado;
    [SerializeField]
    private Transform ExParent;


    [Header("Prefs y items")]
    public static GameObject Descripcion;
    public Transform Contenido;
    public item item;
    public List<ObjetoInvId> invemtarioo = new List<ObjetoInvId>();

    private void Start()
    {
        pointedData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        Descripcion = GameObject.Find("descripcion");

        canvas = transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Arrastrar();
    }

    public void Arrastrar()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointedData.position = Input.mousePosition;
            raycaster.Raycast(pointedData, raycastResults);
            if (raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject.GetComponent<item>())
                {
                    ObjetoSeleccionado = raycastResults[0].gameObject;
                    ExParent = ObjetoSeleccionado.transform.parent.transform;
                    
                    ObjetoSeleccionado.transform.SetParent(canvas, false);
                }
            }
        }

        if (ObjetoSeleccionado != null)
        {
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out localPosition);
            ObjetoSeleccionado.GetComponent<RectTransform>().SetLocalPositionAndRotation(localPosition, ObjetoSeleccionado.GetComponent<RectTransform>().rotation);
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            pointedData.position = Input.mousePosition;
            raycastResults.Clear();
            raycaster.Raycast(pointedData, raycastResults);
            if (raycastResults.Count > 0)
            {
                foreach(var resultado in raycastResults) 
                {
                    if (resultado.gameObject.tag == "slot")
                    {
                        if (resultado.gameObject.GetComponentInChildren<item>() == null) 
                        {
                            ObjetoSeleccionado.transform.SetParent(resultado.gameObject.transform);
                            ObjetoSeleccionado.transform.localPosition = Vector2.zero;
                            ExParent = ObjetoSeleccionado.transform.parent.transform;
                        }

                        else if (resultado.gameObject.GetComponentInChildren<item>().ID == ObjetoSeleccionado.GetComponent<item>().ID)
                            {
                                resultado.gameObject.GetComponentInChildren<item>().cantidad += ObjetoSeleccionado.GetComponent<item>().cantidad;
                                Destroy(ObjetoSeleccionado.gameObject);
                            }

                        else
                        {
                            ObjetoSeleccionado.transform.SetParent(ExParent);
                            ObjetoSeleccionado.transform.localPosition = Vector2.zero;
                        }
                    }

                    else
                    {
                        ObjetoSeleccionado.transform.SetParent(ExParent);
                        ObjetoSeleccionado.transform.localPosition = Vector2.zero;
                    }
                }
            }
            ObjetoSeleccionado = null;
        }

        raycastResults.Clear();
    }

    List<item> pool = new List<item>();

    public void AgregarItem(int id, int cantidad)
    {
        for(int i = 0; i < invemtarioo.Count; i++) 
        {
            if (invemtarioo[i].id == id && data.baseDatos[id].acumulable)
            {
                invemtarioo[i] = new ObjetoInvId(invemtarioo[i].id, invemtarioo[i].cantidad + cantidad);
                InventoryUpdate();
                return;
            }
        }

        if (!data.baseDatos[id].acumulable)
        {
            invemtarioo.Add(new ObjetoInvId(id, 1));
        }

        else
        {
            invemtarioo.Add(new ObjetoInvId(id, cantidad));
        }

        InventoryUpdate();
    }
    public void EliminarItem(int id, int cantidad)
    {
        for (int i = 0;i < invemtarioo.Count; i++)
        {
            if (invemtarioo[i].id == id)
            {
                invemtarioo[i] = new ObjetoInvId(invemtarioo[i].id, invemtarioo[i].cantidad - cantidad);
                if (invemtarioo[i].cantidad < 0)
                {
                    invemtarioo.Remove(invemtarioo[i]);
                    InventoryUpdate();
                    break;
                }
            }

            InventoryUpdate();
        }
    }
    public void InventoryUpdate()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if(i < invemtarioo.Count)
            {
                ObjetoInvId o = invemtarioo[i];
                pool[i].ID = o.id;
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].GetComponent<RectTransform>().localPosition = Vector2.zero;
                pool[i].cantidad = o.cantidad;


                pool[i].gameObject.SetActive(true);
            }

            else
            {
                pool[i].gameObject.SetActive(false);

                //pool[i].gameObject.transform.parent.GetComponent<Image>().fillCenter = false;
            }
        }

        if (invemtarioo.Count > pool.Count) 
        {
            for(int i = pool.Count; i < invemtarioo.Count; i++) 
            {
                item it = Instantiate(item, Contenido.GetChild(i));
                pool.Add(it);

                if(Contenido.GetChild(i).childCount >= 2)
                {
                    for(int s = 0; s < pool.Count; s++)
                    {
                        if(Contenido.GetChild(s).childCount == 0)
                        {
                            it.transform.SetParent(Contenido.GetChild(s));
                            break;
                        }
                    }
                }
                it.transform.position = Vector3.zero;
                it.transform.localScale = Vector3.one;

                ObjetoInvId o = invemtarioo[i];
                pool[i].ID = o.id;
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].GetComponent<RectTransform>().localPosition = Vector2.zero;
                pool[i].cantidad = o.cantidad;


                pool[i].gameObject.SetActive(true);
            }
        }
    }
}
