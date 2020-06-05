using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ComponentType {
    None,
    BaseBridge,
    RightPillar,
    LeftPillar,
    VerticalPillar,
}

public static class Extensions {
    
    public static GridSlot NearestSlotFrom(this IEnumerable<GridSlot> m_slotList, Vector3 _pos){
        if(m_slotList.Count() == 0) return null;
        return m_slotList.Aggregate((gs1,gs2) => Vector3.Distance(gs1.transform.position, _pos) < Vector3.Distance(gs2.transform.position,_pos) ? gs1 : gs2);
    }

} 