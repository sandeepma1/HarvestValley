using UnityEngine.U2D;
using UnityEngine;

public class AtlasBank : MonoBehaviour
{
    public static AtlasBank Instance = null;
    [SerializeField]
    private SpriteAtlas guiAtlas;
    [SerializeField]
    private SpriteAtlas farmingAtlas;
    [SerializeField]
    private SpriteAtlas buildingAtlas;
    private Sprite missingSprite;

    private void Awake()
    {
        Instance = this;
        missingSprite = GetSprite("Blockx100", AtlasType.Buildings);
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
    Buildings
}