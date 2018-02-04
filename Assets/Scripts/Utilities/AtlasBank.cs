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

    public Sprite GetSprite(string name, AtlasType type)
    {
        Sprite sprite;

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
                Debug.LogError("Sprite not found in Bank");
                sprite = new Sprite();
                break;
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