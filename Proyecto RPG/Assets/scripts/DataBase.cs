using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseDatos", menuName = "Inventario/Lista", order = 1)]
public class DataBase : ScriptableObject
{
    [System.Serializable]
    public struct ObjetoInventario
    {
        public string nombre;
        public int ID;
        public Sprite icono;
        public Tipo tipo;
        public bool acumulable;
        public string descripcion;
        public string Void;
    }

    public enum Tipo
    {
        consumible,
        equipable
    }

    public ObjetoInventario[] baseDatos;
}
