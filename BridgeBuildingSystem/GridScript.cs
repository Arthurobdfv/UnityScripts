using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridScript : MonoBehaviour, IGrid
{
    public List<GridSlot> m_gridSlots = new List<GridSlot>();

    void Start()
    {
        m_gridSlots = GetComponentsInChildren<GridSlot>().ToList();
    }

    #region IGrid Implementation
    public void OnHover(Vector3 _pos, BridgeComponent _bridgeItem)
    {
        var hoveringItem = GetPersistentNearestSlot(_pos, _bridgeItem.m_thisType);
        _bridgeItem.transform.position = hoveringItem.transform.position;
    }
    public void OnDragRelease(Vector3 _pos, BridgeComponent _bridgeItem)
    {
        var droppedSlot = GetPersistentNearestSlot(_pos, _bridgeItem.m_thisType);
        droppedSlot.SetBridgeComponent(_bridgeItem);
    }
    #endregion
 
    #region Single Slot Managment
    GridSlot GetPersistentNearestSlot(Vector3 _pos, ComponentType _type){
        return m_gridSlots.Where((slot) => slot.m_buildableType.Contains(_type)).NearestSlotFrom(_pos);
    }
    GridSlot GetNonEmptyNearerSlot(Vector3 _pos){
        var oneOfEach = new List<GridSlot>(){
            GetPersistentNearestSlot(_pos,ComponentType.BaseBridge),
            GetPersistentNearestSlot(_pos,ComponentType.LeftPillar),
            GetPersistentNearestSlot(_pos,ComponentType.VerticalPillar)
        };
        return oneOfEach.Where((slot) => slot.Component != null)?.NearestSlotFrom(_pos);
    }
    public BridgeComponent OnEmptySelect(Vector3 _pos)
    {   
        var slot = GetNonEmptyNearerSlot(_pos);
        var item = slot?.Component;
        if(item != null) slot.EmptySlot();
        return item;
    }
    #endregion
    
    #region Multiple Slots Managment
    public ComponentType[] CurrentStructure (){
        return m_gridSlots.Select((gs) => gs.Component == null ? ComponentType.None : gs.Component.m_thisType).ToArray();
    }
    #endregion

    #region Not Relevant Method
    public void ApplyPhysics(){
        var bridgePieces = m_gridSlots.Where((slot) => slot.Component != null)?.Select((slot) => slot.Component);
        if(bridgePieces == null || bridgePieces.Count() == 0) return;
        else{
            foreach(var bp in bridgePieces){
                bp.gameObject.AddComponent<Rigidbody>();
            }
        }
    }
    #endregion
}
