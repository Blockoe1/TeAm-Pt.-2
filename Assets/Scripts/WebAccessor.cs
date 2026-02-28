#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0007 // Use var

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebAccessor : MonoBehaviour
{
    // Public Variables
    public GameObject loadingScreen;
    public TextMeshProUGUI infoText;
    public RawImage camDisplay;
    public Texture2D texture2D;

    public WebCamDevice[] foundCameras;

    [HideInInspector]
    public int _currentCamIndex = -1;

    // Private Variables
    WebCamTexture _texture;
    Rect _rectTransformRect;
    float _scaleFactorX;
    float _scaleFactorY;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(GetCameraAccess());
        //clearButton.SetActive(false);
    }

    IEnumerator GetCameraAccess()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            foundCameras = WebCamTexture.devices;
            string text = "Cameras Found: ";
            for (int i = 0; i < foundCameras.Length; i++)
            {
                print(foundCameras[i].name);
                text += foundCameras[i].name + ", ";
            }
            //infoText.text = text;
            StartCamera();
        }
        else
        {
            print("No webcams found");
            infoText.text = "No webcams found";
            camDisplay.enabled = false;
            Destroy(this);
        }
    }

    void StartCamera()
    {
        if (foundCameras.Length > 0)
        {
            WebCamDevice cam;
            if (_currentCamIndex < 0)
            {
                cam = foundCameras[0];
                //_currentCamIndex = foundCameras.Length - 1;
            }
            else
            {
                _texture.Stop();
                cam = foundCameras[_currentCamIndex];
                camDisplay.enabled = false;
            }

            _texture = new WebCamTexture(cam.name, Screen.width, Screen.height);
            camDisplay.texture = _texture;

            _texture.Play();
            print("Camera Started");
        }
    }

    public void SwitchCamera()
    {
        if (foundCameras.Length > 1)
        {
            Clear();

            _currentCamIndex++;

            if (_currentCamIndex >= foundCameras.Length)
            {
                _currentCamIndex = 0;
            }

            StartCamera();
        }
        else
        {
            infoText.text = "Only 1 Camera Found";
        }
    }

    public void StartTakePhoto()
    {
        //StartCoroutine(TakePhoto());
    }

    /*public IEnumerator TakePhoto()
	{
		infoText.text = string.Empty;

		takePhotoButton.SetActive(false);
		loadingScreen.SetActive(true);

		var camTexture = camDisplay.texture;
		var rdTexture = new RenderTexture(camTexture.width, camTexture.height, 0);
		Graphics.Blit(camTexture, rdTexture);
		texture2D = ToTexture2D(rdTexture);

		camDisplay.texture = texture2D;


		loadingScreen.SetActive(false);


		clearButton.SetActive(true);
	}
	*/

    public void Clear()
    {
        for (int i = 1; i < camDisplay.transform.childCount; i++)
        {
            Destroy(camDisplay.transform.GetChild(i).gameObject);
        }

        camDisplay.texture = _texture;

        _texture.Play();
    }

    public Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }
}