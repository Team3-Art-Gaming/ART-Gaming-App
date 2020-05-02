//The control strick GameMasters use to control the AR scene

using UnityEngine;

public class ARController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] icons;

    private int curIcon;
    private SpriteRenderer monsterSprite;

    void Awake()
    {
        foreach (GameObject icon in icons) icon.SetActive(false);
        curIcon = 0;
        icons[curIcon].SetActive(true);
        monsterSprite = icons[2].GetComponent<SpriteRenderer>();
    }

    public void SetIcon(int i)
    {
        icons[curIcon].SetActive(false);
        curIcon = i;
        icons[curIcon].SetActive(true);
    }

    public void SetMonster(Sprite sprite)
    {
        monsterSprite.sprite = sprite;
    }

    public void SetPos(Vector3 vec)
    {
        transform.localPosition = vec;
    }
}
