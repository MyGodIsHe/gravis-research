﻿/*
 * Based on http://wiki.unity3d.com/index.php?title=MouseOrbitImproved
 */

using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Settings;
using UI;
using UI.Selection;
using UnityEngine;
using Zenject;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour
{

    public Vector3 target;
    public float distance = 5.0f;
    public float xSpeed = 500.0f;
    public float ySpeed = 1500.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .05f;
    public float distanceMax = 1000f;

    float x = 0.0f;
    float y = 0.0f;

    public ClickNode clickNode;
    private bool _isSelecting = false;

    private TweenerCore<Vector3, Vector3, VectorOptions> _sequence;

    private NavigationSettings _navigationSettings;

    [Inject]
    private void Construct(NavigationSettings navigationSettings)
    {
        _navigationSettings = navigationSettings;
    }
    
    private void OnEnable() {
        clickNode = ClickNode.instance;
    }

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    async void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * distance * 0.02f;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Zooming(distance);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target;

        transform.rotation = rotation;
        transform.position = position;

        // create random node
        if (Input.GetKeyDown(KeyCode.X))
        {
            var parent = ClickNode.instance.node.GetComponent<NodeView>().nodeLink;
            if (parent != null && !_isSelecting)
            {
                _isSelecting = true;
                var point = Camera.main.WorldToScreenPoint(parent.gameObject.transform.position);
                
                var wheel = NodeTypeWheelSelector.Instance;
                var input = NodeInputField.Instance;
                
                wheel.SetPosition(point);
                input.SetPosition(point);
                
                var type = await wheel.Select();
                var strategy = SelectionHelper.GetStrategy(type);
                var text = await strategy.GetText();
                
                CreateNode(type, text, parent);
                _isSelecting = false;
            }
        }

        if (Input.GetKeyDown(_navigationSettings.MoveKey))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            var cast = Physics.Raycast(ray, out var hit);
            if (cast)
            {
                var node = hit.collider.GetComponent<NodeView>();
                if (node)
                {
                    if (_sequence.IsActive())
                    {
                        _sequence?.Complete();
                    }

                    _sequence = DOTween
                        .To(() => target, value => target = value, node.transform.position,  _navigationSettings.MoveDuration)
                        .SetEase(_navigationSettings.MoveEase);
                }
            }
;        }
    }

    public static async void CreateNode(NodeType type, string text, Node target)
    {
        var gm = GraphManager.Get();
        var pIndex = Random.Range(0, gm.GetParts().Count);
        
        var node = new Node
        {
            type = type,
            text = text
        };
        
        node.position = target.position + new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
            
        await gm.LinkNode(node, target, gm.GetParts()[pIndex]);
    }
    
    public static async Task FocusOn(Node node)
    {
        
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public float Zooming(float dist)
    {
        var modify = Input.GetAxis("Mouse ScrollWheel")*5;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            modify = 1*2;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            modify = -1*2;
        }

        return Mathf.Clamp(dist - modify, distanceMin, distanceMax);;
    }
    
}
