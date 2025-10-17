using System;
using UnityEngine;

[Serializable]
public class UITreeConnectionDetails {

    public UITreeConnectionHandler childNode;
    public NodeDirectionType direction;

    [Range(100f, 350f)]
    public float length;
}

public class UITreeConnectionHandler : MonoBehaviour
{
    [SerializeField] private UITreeConnectionDetails[] _connectionDetails;
    [SerializeField] private UITreeConnection[] _connections;

    private RectTransform _rect;

    private void OnValidate() {
        if(_rect == null)
            _rect = GetComponent<RectTransform>();

        if (_connectionDetails.Length != _connections.Length) {
            Debug.Log("Amount of details should be same as amount of connections. - " + gameObject.name);
            return;
        }

        UpdateConnections();


    }

    private void UpdateConnections() {
        for(int i = 0; i < _connectionDetails.Length; i++) {
            var detail = _connectionDetails[i];
            var connection = _connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(_rect);

            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode.SetPosition(targetPosition);

        }
    }

    public void SetPosition(Vector2 position) => _rect.anchoredPosition = position;
    

}
