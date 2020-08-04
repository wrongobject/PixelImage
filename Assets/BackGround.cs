using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class BackGround : MonoBehaviour
{
    static readonly Color Gray = new Color(0.7f,0.7f,0.7f);

    public int PixelX = 10;
    public int PixelY = 10;    
    public Texture2D texture;
    private Sprite sprite;
    private Image image;   
    // Start is called before the first frame update
    private void Awake()
    {
        image = GetComponent<Image>();       
    }   
    void Start()
    {
        if (!image) return;
        GenerateBG();
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        image.sprite = sprite;
    }   

    void OnValidate()
    {
        if (!image) return;
        GenerateBG();       
    }
    void GenerateBG()
    {
        if (!texture)
            texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false, false);
        else
            texture.Resize(Screen.width, Screen.height);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                texture.SetPixel(i, texture.height - j - 1, GetColor(i,j));
            }
        }
        texture.Apply();
        
    }

    Color GetColor(int i, int j)
    {
        if ((i / PixelX) % 2 == 0 )
        {
            if ((j / PixelY) % 2 == 0)
                return Color.white;
            return Gray;
        }
        else
        {
            if ((j / PixelY) % 2 == 0)
                return Gray;
            return Color.white;
        }
    }
}
