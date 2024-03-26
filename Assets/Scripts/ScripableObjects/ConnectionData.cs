using UnityEngine;

namespace ScripableObjects
{
    [CreateAssetMenu(fileName = "ConnectionData", menuName = "Data connection")]
    public class ConnectionData : ScriptableObject
    {
        public string referenceProduction;
        public string referenceLocal;
    }
}
