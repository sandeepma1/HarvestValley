using UnityEngine.U2D;
using UnityEngine;
using HarvestValley.IO;

public class AtlasBank : Singleton<AtlasBank>
{
    [SerializeField]
    private SpriteAtlas guiAtlas;
    [SerializeField]
    private SpriteAtlas farmingAtlas;
    [SerializeField]
    private SpriteAtlas buildingAtlas;
    [SerializeField]
    private SpriteAtlas livestockAtlas;
    [SerializeField]
    private SpriteAtlas itemsAtlas;
    private Sprite missingSprite;

    private void Start()
    {
        missingSprite = GetSprite("Blockx100", AtlasType.Buildings);
    }

    public Sprite GetSprite(int itemId, AtlasType type)
    {
        return GetSprite(ItemDatabase.GetItemSlugById(itemId), type);
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
            case AtlasType.Livestock:
                sprite = livestockAtlas.GetSprite(name);
                break;
            case AtlasType.Items:
                sprite = itemsAtlas.GetSprite(name);
                break;
        }

        if (sprite == null)
        {
            if (GEM.ShowDebugInfo) Debug.Log("Sprite named " + name + " not found in Bank Type " + type);
            sprite = missingSprite;
        }
        return sprite;
    }
}

public enum AtlasType
{
    GUI,
    Farming,
    Buildings,
    Livestock,
    Items
}