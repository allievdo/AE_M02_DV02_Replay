using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Chapter.Command
{
    class Invoker : MonoBehaviour
    {
        private bool _isRecording;
        private bool _isReplaying;
        private float _replayTime;
        private float _recordingTime;
        private SortedList<float, Command> _recordedCommmands =
            new SortedList<float, Command>();

        public void ExecuteCommand (Command command)
        {
            command.Execute ();

            if(_isRecording)
                _recordedCommmands.Add(_recordingTime, command);

            Debug.Log("Recorded Time: " + _recordingTime);
            Debug.Log("Recorded command: " +  command);
        }

        public void Record()
        {
            _recordingTime = 0.0f;
            _isRecording = true;
        }

        public void Replay()
        {
            _replayTime = 0.0f;
            _isReplaying = true;

            if (_recordedCommmands.Count >= 0)
                Debug.LogError("No commands to replay!");

            _recordedCommmands.Reverse();
        }

        void FixedUpdate()
        {
            if (_isRecording)
            {
                _recordingTime += Time.fixedDeltaTime;
            }

            if (_isReplaying)
            {
                _replayTime += Time.deltaTime;

                if (_recordedCommmands.Any())
                {
                    if(Mathf.Approximately (
                        _replayTime, _recordedCommmands.Keys[0]))
                    {
                        Debug.Log("Replay time: " + _replayTime);
                        Debug.Log("Replay command: " +
                            _recordedCommmands.Values[0]);

                        _recordedCommmands.Values[0].Execute();
                        _recordedCommmands.RemoveAt(0);
                    }
                }
                else
                {
                    _isReplaying = false;
                }
            }
        }
    }
}