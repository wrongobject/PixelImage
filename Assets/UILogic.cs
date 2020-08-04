using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class UILogic : MonoBehaviour
{
    static readonly Color TransparentColor = new Color(0,0,0,0);
    private float scale = 1;
    private int Scale { get { return (int)scale; } }
    private float qualityScale = 1;
   // private int QualityScale { get { return (int)QualityScale; } }

    public Button fileButton;
    public Button modifyButton;
    public Image image;
    public InputField widthInput;
    public InputField heightInput;
    public Slider slider;
    private int width = 50;
    private int height = 50;
    private Texture2D origeTexture;
    public Texture2D targetTexture;
    private Coroutine coroutine;
    
    private void OnEnable()
    {
        fileButton.onClick.AddListener(OnClickFileBtn);
        modifyButton.onClick.AddListener(OnClickModifyBtn);
        InputLogic.mouseScrollEvent += OnScaleChange;
        InputLogic.mouseDragEvent += OnImageDrag;
        slider.onValueChanged.AddListener(OnScaleSliderChange);
    }

    void OnClickFileBtn()
    {
        OpenFileDlg param = new OpenFileDlg();
        param.structSize = System.Runtime.InteropServices.Marshal.SizeOf(param);
        param.filter = "png | *.png;*.jpg;";
        param.file = new string(new char[256]);
        param.maxFile = param.file.Length;
        param.fileTitle = new string(new char[64]);
        param.maxFileTitle = param.fileTitle.Length;
        param.initialDir = Application.dataPath;  // default path
        param.title = "标题";
        param.defExt = "png";
        param.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (OpenFileDialog.GetOpenFileName(param))
        {
            Debug.Log(param.file);

            if (coroutine == null)
            {
                coroutine = StartCoroutine(ReadTexture(param.file));
            }
            
        }
    }

    IEnumerator ReadTexture(string file)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(file);
        DownloadHandlerTexture handlerTexture = new DownloadHandlerTexture();
        unityWebRequest.downloadHandler = handlerTexture;
        yield return unityWebRequest.SendWebRequest();
        origeTexture = handlerTexture.texture;
        //加载出来需要设置下当前大小
        if (origeTexture)
        {
            width = origeTexture.width;
            height = origeTexture.height;
        }
        slider.value = scale;
        SetSprite();
        coroutine = null;
    }

    void SetSprite()
    {
        if (!origeTexture) return;
        int realScale = Scale <= 1 ? 1 : Scale;
        if (!targetTexture)
        {           
            targetTexture = new Texture2D(width * realScale, height * realScale, TextureFormat.ARGB32, false, false);
        }
        else
        {
            targetTexture.Resize(width * realScale, height * realScale);
        }
        image.rectTransform.sizeDelta = new Vector2(width * realScale, height * realScale);
        if (Scale >= 0)
            UpTextureData();
        else
            DownTextureData();
        image.sprite = Sprite.Create(targetTexture, new Rect(0,0, targetTexture.width, targetTexture.height),Vector2.zero);
        if (!image.enabled)
            image.enabled = true;
    }

    void OnClickModifyBtn()
    {
        int w = 0, h = 0;
        if (widthInput)
        {
            int.TryParse(widthInput.text, out w);
        }
        if (heightInput)
        {
            int.TryParse(heightInput.text, out h);
        }

        width = w ;
        height = h ;
        SetSprite();
    }

    void OnScaleChange(float delta)
    {
        float newscale = scale + delta;
        if (newscale < -10)
            newscale = -10;
        if (newscale > 10)
            newscale = 10;

        if (Scale == (int)newscale) return;
        slider.value = newscale;
    }

    void OnScaleSliderChange(float value)
    {
        scale = value;
        SetSprite();
    }

    void OnQualitySliderChange(float value)
    {
        qualityScale = value;
    }

    void OnImageDrag(Vector3 delta)
    {
        image.transform.localPosition += delta;
    }
    void UpTextureData()
    {
        int realScale = Scale <= 1 ? 1 : Scale;
        for (int i = 0; i < targetTexture.width; i++)
        {
            for (int j = 0; j < targetTexture.height; j++)
            {
                targetTexture.SetPixel(i, j, origeTexture.GetPixel(i / realScale, j / realScale));
            }
        }
        targetTexture.Apply();
    }
    Color[] colors = new Color[] { Color.white,Color.yellow,Color.red,Color.gray,Color.cyan,Color.blue};
    void DownTextureData()
    {
        int step = -Scale;
        float per = 1.0f / step / step;
        Color color = Color.black;
        int w = targetTexture.width / step;
        int h = targetTexture.height / step;
        
        int startw = 0,starth = 0;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                startw = i * step;
                starth = j * step;
                color = Color.black;
                for (int m = startw; m < startw + step; m++)
                {
                    for (int n = starth; n < starth + step; n++)
                    {
                        color += origeTexture.GetPixel(m, n) * per;
                    }
                }
                //color = colors[Random.Range(0, colors.Length)];
                for (int m = startw; m < startw + step; m++)
                {
                    for (int n = starth; n < starth + step; n++)
                    {

                        targetTexture.SetPixel(m, n, color);
                    }
                }
                
            }
        }
       
        targetTexture.Apply();
    }
}
