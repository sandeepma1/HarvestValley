using UnityEngine.U2D;
using UnityEngine;

public class AtlasBank : Singleton<AtlasBank>
{
    [SerializeField]
    private SpriteAtlas guiAtlas;
    [SerializeField]
    private SpriteAtlas farmingAtlas;
    [SerializeField]
    private SpriteAtlas buildingAtlas;
    [SerializeField]
    private Sprite missingSprite;

    private void Start()
    {
        if (missingSprite == null)
        {
            Debug.LogError("Placeholder sprite in Atlas Banks is missing, assign in editor");
        }
    }

    public Sprite GetSprite(string name, AtlasType type)
    {
        Sprite sprite = null;

        switch (type)
        {
            case AtlasType.GUI:
                sprite = guiAtlas.GetSprite(name);
                break;
            case AtlasType.Farming:
                sprite = farmingAtlas.GetSprite(name);
                break;
            case AtlasType.Buildings:
                sprite = buildingAtlas.GetSprite(name);
                break;
            default:
                break;
        }

        if (sprite == null)
        {
            Debug.Log("Sprite named " + name + " not found in Bank Type " + type);
            sprite = missingSprite;
        }
        return sprite;
    }
}

public enum AtlasType
{
    GUI,
    Farming,
    Buildings
}