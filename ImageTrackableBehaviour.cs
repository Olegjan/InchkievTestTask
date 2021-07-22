/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.IO;
using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine.Rendering;

namespace maxstAR
{
    public class ImageTrackableBehaviour : AbstractImageTrackableBehaviour
    {
		bool _onTrackFail;
		bool _onTrackSuccess;
		public static event System.Action<bool> OnTrackSuccessFailEvent;
		public static event System.Action<bool> OnTrackFailEvent;
		int logicForOneCallOnTrackInSuccess;
		int logicForOneCallOnTrackInFail;
		private void Start()
        {
			
		}

		public override void OnTrackSuccess(string id, string name, Matrix4x4 poseMatrix)
        {
			logicForOneCallOnTrackInSuccess++;
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable renderers
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
				
			}

			// Enable colliders
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			transform.position = MatrixUtils.PositionFromMatrix(poseMatrix);
			transform.rotation = MatrixUtils.QuaternionFromMatrix(poseMatrix);

			_onTrackSuccess = true;

			if (_onTrackSuccess && _onTrackFail)
            {
				OnTrackSuccessFailEvent?.Invoke(_onTrackSuccess);
				_onTrackFail = !_onTrackFail;
			}
		}
		public override void OnTrackFail()
		{
			if(logicForOneCallOnTrackInFail == logicForOneCallOnTrackInSuccess)
            {
				Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
				Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

				// Disable renderer
				foreach (Renderer component in rendererComponents)
				{
					component.enabled = false;
				}

				// Disable collider
				foreach (Collider component in colliderComponents)
				{
					component.enabled = false;
				}
				_onTrackFail = true;
				if (_onTrackFail && _onTrackSuccess)
				{
					_onTrackSuccess = !_onTrackSuccess;
					OnTrackSuccessFailEvent?.Invoke(_onTrackSuccess);
				}
			}
			logicForOneCallOnTrackInFail = logicForOneCallOnTrackInSuccess;
		}
	}
}