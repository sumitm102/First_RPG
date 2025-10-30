using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UITreeConnectionDetails {

    public UITreeConnectionHandler childNode;
    public NodeDirectionType direction;

    [Range(100f, 350f)]
    public float length;

    [Range(-50f, 50f)]
    public float rotation;
}

public class UITreeConnectionHandler : MonoBehaviour
{
    [SerializeField] private UITreeConnectionDetails[] _connectionDetails;
    [SerializeField] private UITreeConnection[] _connections;

    private RectTransform _rect;

    private Image _connectionImage;
    private Color _originalColor;

    private void Awake() {
        if(_connectionImage != null)
            _originalColor = _connectionImage.color;
    }

    private void OnValidate() {
        if(_rect == null)
            _rect = GetComponent<RectTransform>();

        if (_connectionDetails.Length <= 0)
            return;

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
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);

            // This is so that the child nodes always stay below the parent node
            detail.childNode.transform.SetAsLastSibling();

        }
    }

    public void UpdateAllConnections() {
        UpdateConnections();

        foreach(var node in _connectionDetails) 
            if(node != null)
                node.childNode.UpdateConnections();
        
    }

    public void ConnectionImageUnlocked(bool unlocked) {
        if (_connectionImage == null)
            return;

        _connectionImage.color = unlocked ? Color.white : _originalColor;
    }
    public void SetConnectionImage(Image image) => _connectionImage = image;
    public void SetPosition(Vector2 position) {
        if(_rect == null) {
            Debug.Log("Rect Transfrom of " + this.gameObject.name + " is null");
            return;
        }

        _rect.anchoredPosition = position;
    }
    

}
