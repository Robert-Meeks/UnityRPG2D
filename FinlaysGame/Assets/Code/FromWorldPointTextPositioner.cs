using UnityEngine;

public class FromWorldPointTextPositioner : IFloatingTextPositioner
{
    private readonly Camera _camera;
    private readonly Vector3 _worldPosition;
    private float _timeToLive;
    private readonly float _speed;

    private float _yOffset;

    public FromWorldPointTextPositioner(Camera camera, Vector3 worldPosition, float timeToLive, float speed)
    {
        Debug.Log(string.Format("FromWorldPointTextPositioner.cs / Contructor / "));
        _camera = camera;
        _worldPosition = worldPosition;
        _timeToLive = timeToLive;
        _speed = speed;
    }



    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        Debug.Log("GET POSITION()");
        if ((_timeToLive -= Time.deltaTime) <= 0)
        {
            Debug.Log("FromWorldPointTextPosition.cs/ GetPosition() - IF returned false.");
            return false;
        }
        var screenPosition = _camera.WorldToScreenPoint(_worldPosition);
        position.x = screenPosition.x - (size.x / 2);
        position.y = Screen.height - screenPosition.y - _yOffset;

        Debug.Log(string.Format("FromWorldPointTextPosition.cs/ GetPosition() - _speed = {0}", _speed));
        _yOffset += Time.deltaTime * _speed;

        return true;
    }
}

