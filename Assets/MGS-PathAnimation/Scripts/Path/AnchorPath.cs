﻿/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AnchorPath.cs
 *  Description  :  Define curve path base on anchors.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  2/28/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using Developer.AnimationCurveExtension;
using UnityEngine;

namespace Developer.PathAnimation
{
    [AddComponentMenu("Developer/PathAnimation/AnchorPath")]
    public class AnchorPath : CurvePath
    {
        #region Field and Property
        /// <summary>
        /// Path curve is close?
        /// </summary>
        public bool close = false;

        /// <summary>
        /// Anchors of path curve.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected List<Vector3> anchors = new List<Vector3>() { Vector3.one,
            new Vector3(1, 1, 2), new Vector3(3, 1, 2), new Vector3(3, 1, 3)};

        /// <summary>
        /// Count of path curve anchors.
        /// </summary>
        public int AnchorsCount { get { return anchors.Count; } }

        /// <summary>
        /// WrapMode of path curve.
        /// </summary>
        public WrapMode Wrapmode
        {
            set { curve.PreWrapMode = curve.PostWrapMode = value; }
            get { return curve.PreWrapMode; }
        }

        /// <summary>
        /// VectorAnimationCurve of path.
        /// </summary>
        protected VectorAnimationCurve curve = new VectorAnimationCurve();
        #endregion

        #region Protected Method
        protected bool CheckAnchorIndex(int index)
        {
            if (index >= 0 && index < anchors.Count)
                return true;
            else
                return false;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild path.
        /// </summary>
        public override void Rebuild()
        {
            curve = VectorAnimationCurve.FromAnchors(anchors.ToArray(), close);
        }

        /// <summary>
        /// Get point on path curve at time.
        /// </summary>
        /// <param name="time">Time of curve.</param>
        /// <returns>The point on path curve at time.</returns>
        public override Vector3 GetPoint(float time)
        {
            return transform.TransformPoint(curve.Evaluate(time));
        }

        /// <summary>
        /// Get max time of path curve.
        /// </summary>
        /// <returns>Max time of path curve.</returns>
        public override float GetMaxTime()
        {
            if (curve.Length > 0)
                return curve[curve.Length - 1].time;
            else
                return 0;
        }

        #region Anchor Operate
        public void AddAnchor(Vector3 item)
        {
            anchors.Add(transform.InverseTransformPoint(item));
        }

        public void InsertAnchor(int index, Vector3 item)
        {
            if (CheckAnchorIndex(index))
                anchors.Insert(index, transform.InverseTransformPoint(item));
        }

        public void SetAnchorAt(int index, Vector3 position)
        {
            if (CheckAnchorIndex(index))
                anchors[index] = transform.InverseTransformPoint(position);
        }

        public Vector3 GetAnchorAt(int index)
        {
            if (CheckAnchorIndex(index))
                return transform.TransformPoint(anchors[index]);
            else
                return Vector3.zero;
        }

        public void RemoveAnchorAt(int index)
        {
            if (CheckAnchorIndex(index))
                anchors.RemoveAt(index);
        }

        public void ClearAnchors()
        {
            anchors.Clear();
        }
        #endregion
        #endregion
    }
}