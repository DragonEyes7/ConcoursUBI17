using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrintClues : MonoBehaviour
{
    Text _PrintMessage;
    string[] _Messages;
    Vector3 _StartingPos;
    int _CurrentMessage;
    float _Up;

    public void Setup()
    {
        _PrintMessage = transform.GetChild(0).GetComponent<Text>();
        _Up = _PrintMessage.fontSize + 5;
        _StartingPos = transform.position;
    }

    public void Print(string[] messages)
    {
        _Messages = messages;        
        StartPrinting();
    }

    void StartPrinting()
    {

        if(_Messages.Length == 0)
        {
            _PrintMessage.text = "No clue Found!";
        }
        else
        {
            _PrintMessage.text = "";
        }

        _CurrentMessage = 0;
        transform.position = _StartingPos;
        StartCoroutine(AnimationStart(0.5f, 55));
    }

    void MoveUpward(float y)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
    }

    IEnumerator AnimationStart(float delay, float up)
    {
        yield return new WaitForSecondsRealtime(delay);
        MoveUpward(up);
        if (up != 90)
        {
            StartCoroutine(AnimationStart(0.25f, 90));
        }
        else
        {
            StartCoroutine(AnimationPrinting(0.15f, _Up));
        }
    }

    IEnumerator AnimationPrinting(float delay, float up)
    {
        if (_Messages.Length > 0)
        {
            if (_Messages[_CurrentMessage].Length > 43)
            {
                up += up;
            }

            _PrintMessage.text += "-" + _Messages[_CurrentMessage] + "\n";
            ++_CurrentMessage;
        }

        yield return new WaitForSecondsRealtime(delay);
        MoveUpward(up);
        if (_CurrentMessage < _Messages.Length && _Messages.Length > 0)
        {
            StartCoroutine(AnimationPrinting(0.15f, _Up));
        }
    }
}
