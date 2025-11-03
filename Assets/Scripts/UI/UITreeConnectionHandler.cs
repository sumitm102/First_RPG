using System.Collections.Generic;
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

        if (_rect == null)
            _rect = GetComponent<RectTransform>();
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
            var nodeDetail = _connectionDetails[i];
            var connection = _connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(_rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(nodeDetail.direction, nodeDetail.length, nodeDetail.rotation);

            if (nodeDetail.childNode == null)
                continue;

            nodeDetail.childNode.SetPosition(targetPosition);
            nodeDetail.childNode.SetConnectionImage(connectionImage);

            // This is so that the child nodes always stay below the parent node
            nodeDetail.childNode.transform.SetAsLastSibling();

        }
    }

    public void UpdateAllChildConnections() {
        UpdateConnections();

        foreach(var node in _connectionDetails) 
            if (node.childNode != null) 
                node.childNode.UpdateConnections();  
    }

    public UITreeNode[] GetChildNodes() {
        List<UITreeNode> childrenToReturn = new List<UITreeNode>();

        foreach(var node in _connectionDetails) {
            
            if(node.childNode != null && node.childNode.TryGetComponent<UITreeNode>(out var uiTreeNodeOfChildren))
                childrenToReturn.Add(uiTreeNodeOfChildren);
        }

        return childrenToReturn.ToArray();
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
