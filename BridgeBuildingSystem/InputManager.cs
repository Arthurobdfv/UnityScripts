using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    private static BridgeComponent m_selectedBC;
    public static BridgeComponent SelectedBridgeComponent{
        get { return m_selectedBC; }
        private set{
            m_selectedBC = value;
        }
    }


    Vector3 m_lastMousePos = Vector3.negativeInfinity;

    public GraphicRaycaster m_graphicRaycaster;
    EventSystem m_mainEvtSys;

    void Awake(){   
        m_mainEvtSys = FindObjectOfType<EventSystem>();
    }

    void Update(){
        InputChecker();
    }

    void InputChecker(){
        if(Input.GetMouseButtonDown(0) && m_selectedBC == null){
            RaycastHit hit;
            var res = new List<RaycastResult>();
            if(GraphicRaycasterOnMousePosition(out res)){
                var bridgeC = res.Select((r) => r.gameObject).Where((r) => r.GetComponent<BridgeComponentButton>() != null).FirstOrDefault()?.GetComponent<BridgeComponentButton>();
                if(bridgeC != null) SetBridgeComponent(bridgeC.OnClick());
            }
            else if(RaycastOnMousePosition(out hit)){
                var grid = hit.collider?.GetComponent<IGrid>();
                if(grid != null){
                    m_selectedBC = grid.OnEmptySelect(hit.point);
                }
            }

        }
        if(Input.GetMouseButton(0) && m_selectedBC != null){
            if(Input.mousePosition == m_lastMousePos) return;
            m_lastMousePos = Input.mousePosition;
            RaycastHit hit;
            if(RaycastOnMousePosition(out hit)){
                var grid = hit.collider?.GetComponent<IGrid>();
                if(grid != null){
                    grid.OnHover(hit.point,m_selectedBC);
                }
                else{
                    m_selectedBC.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f-Camera.main.transform.position.z+3f));
                }
            }
            else{
                m_selectedBC.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f-Camera.main.transform.position.z+3f));
            }
        }
        if(Input.GetMouseButtonUp(0) && m_selectedBC != null){
            RaycastHit hit;
            if(RaycastOnMousePosition(out hit)){
                var grid = hit.collider?.GetComponent<IGrid>();
                if(grid != null){
                    grid.OnDragRelease(hit.point,m_selectedBC);
                }
                else Destroy(SelectedBridgeComponent.gameObject);
            }else{
                Destroy(SelectedBridgeComponent.gameObject);
            }
            SelectedBridgeComponent = null;
        }
    }



    bool RaycastOnMousePosition(out RaycastHit hit){
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray,out hit);
    }

    bool GraphicRaycasterOnMousePosition(out List<RaycastResult> results){
                var pevt = new PointerEventData(m_mainEvtSys);
                pevt.position = Input.mousePosition;
                var x = new List<RaycastResult>();
                m_graphicRaycaster.Raycast(pevt, x);
                results = x;
                return results.Count > 0;
    }

    public void SetBridgeComponent(BridgeComponent _compo){
        if(SelectedBridgeComponent != null) Destroy(SelectedBridgeComponent.gameObject);
        SelectedBridgeComponent = Instantiate(_compo.gameObject).GetComponent<BridgeComponent>();
    }

}