using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Animations;

using QuickVR;

public class TestParentConstraint : MonoBehaviour
{

    public Transform _parent = null;
    public Transform _child = null;

    protected ParentConstraint _pConstraint = null;

    protected virtual void Awake()
    {
        _pConstraint = _child.GetOrCreateComponent<ParentConstraint>();

        ConstraintSource cSource = new ConstraintSource();
        cSource.sourceTransform = _parent;
        cSource.weight = 1;
        
        _pConstraint.AddSource(cSource);

        _pConstraint.constraintActive = true;
    }

    //protected float dRight;
    //protected float dUp;
    //protected float dForward;

    //protected virtual void Awake()
    //{
    //    Vector3 offset = _child.position - _parent.position;

    //    dRight = Vector3.Dot(offset, _parent.right);
    //    dUp = Vector3.Dot(offset, _parent.up);
    //    dForward = Vector3.Dot(offset, _parent.forward);
    //}

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Vector3 fwd = Vector3.ProjectOnPlane(_parent.forward, Vector3.up).normalized;
    //    Vector3 right = Vector3.ProjectOnPlane(_parent.right, Vector3.up).normalized;

    //    //_child.position = _parent.position + _parent.right * dRight + _parent.up * dUp + _parent.forward * dForward;
    //    _child.position = _parent.position + right * dRight + Vector3.up * dUp + fwd * dForward;
    //}

    //protected virtual void OnDrawGizmos()
    //{
            
    //    if (_parent && _child)
    //    {
    //        Gizmos.DrawLine(_parent.position, _child.position);
    //    }

    //}

}
