using UnityEngine;

public interface IGrid {
    void OnHover(Vector3 _pos,BridgeComponent _bridgeItem);
    void OnDragRelease(Vector3 _pos, BridgeComponent _bridgeItem);

    BridgeComponent OnEmptySelect(Vector3 _pos);
}
