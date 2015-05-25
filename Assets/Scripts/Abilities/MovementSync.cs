using UnityEngine;
using System.Collections;

public class MovementSync : Photon.MonoBehaviour {

    private Vector3 _latestCorrectPos;
    private Vector3 _onUpdatePos;
    private Quaternion _latestCorrectRot;
    private Quaternion _onUpdateRot;
    private float _lerpFraction;


    void Start() {
        _latestCorrectPos = transform.position;
        _onUpdatePos = transform.position;
        _latestCorrectRot = transform.rotation;
    }

    void FixedUpdate() {
        if (photonView.isMine) {
            //do nothing
        }
        else {
            SyncedMovement();
        }
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            _onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            _latestCorrectRot = rot;
            _onUpdateRot = transform.rotation;
            _lerpFraction = 0;                           // reset the fraction we alreay moved. see Update(

        }
    }

    private void SyncedMovement() {
        // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead.

        _lerpFraction = _lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _lerpFraction);    // set our pos between A and B
        transform.rotation = Quaternion.Slerp(_onUpdateRot, _latestCorrectRot, _lerpFraction);

    }
}
