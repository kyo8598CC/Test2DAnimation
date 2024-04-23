using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum RoleAnimaType
{
    None,
    走路,
    跑步,
    骑乘交通工具,
    骑乘宠物
}

public enum SlotType
{
    身体,
    帽子,
    头发,
    衣服,
    坐骑
}

public enum Direction
{
    上,
    下,
    左,
    右
}

public class SpriteAnima : MonoBehaviour
{
    [Space(30)] public SpriteRenderer body;
    public SpriteRenderer head;
    public SpriteRenderer hair;
    public SpriteRenderer clothes;
    public SpriteRenderer rider;

    [HideInInspector] public bool showRider;

    [Title("身体id")] [PropertyOrder(-6)] [GUIColor(0.3f, 0.8f, 0.8f, 1)]
    public string currentBodyId;

    [PropertyOrder(-6)]
    [Button("切换身体", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChangeBodyId()
    {
        Debug.Log("ChangeBodyId");
        LoadSprites(SlotType.身体);
    }

    [Title("衣服id")] [PropertyOrder(-5)] [GUIColor(0.3f, 0.8f, 0.8f, 1)]
    public string currentClothesId;

    [PropertyOrder(-5)]
    [Button("切换衣服", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChangeClothesId()
    {
        LoadSprites(SlotType.衣服);
    }

    [Title("帽子id")] [PropertyOrder(-4)] [GUIColor(0.3f, 0.8f, 0.8f, 1)]
    public string currentHeadId;

    [PropertyOrder(-4)]
    [Button("切换帽子", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChangeHeadId()
    {
        LoadSprites(SlotType.帽子);
    }

    [Title("头发id")] [PropertyOrder(-3)] [GUIColor(0.3f, 0.8f, 0.8f, 1)]
    public string currentHairId;

    [PropertyOrder(-3)]
    [Button("切换头发", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChangeHairId()
    {
        LoadSprites(SlotType.头发);
    }

    [ShowIf("showRider")] [Title("坐骑id")] [PropertyOrder(-2)] [GUIColor(0.3f, 0.8f, 0.8f, 1)]
    public int currentRiderId;

    [ShowIf("showRider")]
    [PropertyOrder(-2)]
    [Button("切换坐骑", ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void ChangeRiderId()
    {
        LoadSprites(SlotType.坐骑);
    }

    [Title("当前动画状态")] [EnumToggleButtons()]
    public RoleAnimaType roleAnimaType;

    [Title("当前方向")] [EnumToggleButtons()] public Direction direction;

    private RoleAnimaType lastRoleAnimaType = RoleAnimaType.None;

    [PreviewField(ObjectFieldAlignment.Left)]
    public List<Sprite> bodyList = new List<Sprite>();

    [PreviewField(ObjectFieldAlignment.Left)]
    public List<Sprite> hairList = new List<Sprite>();

    [PreviewField(ObjectFieldAlignment.Left)]
    public List<Sprite> headList = new List<Sprite>();

    [PreviewField(ObjectFieldAlignment.Left)]
    public List<Sprite> clothesList = new List<Sprite>();

    [PreviewField(ObjectFieldAlignment.Left)]
    public List<Sprite> riderList = new List<Sprite>();


    [Title("帧间隔")] [ShowInInspector] private float interval = 0.2f;
    private float lastTime;
    private int currentFrame;
    private bool isPositive = true;


    void Start()
    {
        lastRoleAnimaType = RoleAnimaType.None;
        lastTime = 0;
        interval = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeRoleAnimaType();
        if (lastTime + interval < Time.time)
        {
            lastTime = Time.time;
            if (isPositive)
            {
                currentFrame++;
                if (currentFrame >= 3)
                {
                    currentFrame = 1;
                    isPositive = false;
                }
            }
            else
            {
                currentFrame--;
                if (currentFrame < 0)
                {
                    currentFrame = 1;
                    isPositive = true;
                }
            }

            ShowAvatar(SlotType.身体);
            ShowAvatar(SlotType.头发);
            ShowAvatar(SlotType.帽子);
            ShowAvatar(SlotType.衣服);
            if (roleAnimaType is RoleAnimaType.骑乘宠物 or RoleAnimaType.骑乘交通工具)
            {
                ShowAvatar(SlotType.坐骑);
            }
        }
    }

    public void ShowAvatar(SlotType slotType)
    {
        int frame = currentFrame;
        if (direction == Direction.右)
        {
            frame += 3;
        }
        else if (direction == Direction.下)
        {
            frame += 6;
        }
        else if (direction == Direction.左)
        {
            frame += 9;
        }

        switch (slotType)
        {
            case SlotType.身体:
                if (bodyList.Count > 0)
                {
                    body.sprite = bodyList[frame];
                }

                break;
            case SlotType.头发:
                if (hairList.Count > 0)
                {
                    hair.sprite = hairList[frame];
                }

                break;
            case SlotType.帽子:
                if (headList.Count > 0)
                {
                    head.sprite = headList[frame];
                }

                break;

            case SlotType.衣服:
                if (clothesList.Count > 0)
                {
                    clothes.sprite = clothesList[frame];
                }

                break;
            case SlotType.坐骑:
                if (riderList.Count > 0)
                {
                    rider.sprite = riderList[frame];
                }

                break;
        }
    }

    public void ChangeRoleAnimaType()
    {
        if (lastRoleAnimaType != roleAnimaType)
        {
            lastRoleAnimaType = roleAnimaType;

            LoadSprites(SlotType.身体);
            LoadSprites(SlotType.头发);
            LoadSprites(SlotType.帽子);
            LoadSprites(SlotType.衣服);
            LoadSprites(SlotType.坐骑);
        }
    }

    public void LoadSprites(SlotType slotType)
    {
        string res = "";
        if (roleAnimaType == RoleAnimaType.走路)
        {
            res = "Assets/Art/Walk/";
        }
        else if (roleAnimaType == RoleAnimaType.跑步)
        {
            res = "Assets/Art/Run/";
        }
        else if (roleAnimaType == RoleAnimaType.骑乘交通工具)
        {
            res = "Assets/Art/RiderVehicle/";
        }
        else if (roleAnimaType == RoleAnimaType.骑乘宠物)
        {
            res = "Assets/Art/RiderPokemon/";
        }

        string resPath = "";
        List<Sprite> currentList = new List<Sprite>();
        switch (slotType)
        {
            case SlotType.身体:
                resPath = "body/" + currentBodyId + ".png";
                currentList = bodyList;
                break;
            case SlotType.头发:
                resPath = "hair/" + currentHairId + ".png";
                currentList = hairList;
                break;
            case SlotType.帽子:
                resPath = "head/" + currentHeadId + ".png";
                currentList = headList;
                break;
            case SlotType.衣服:
                resPath = "clothes/" + currentClothesId + ".png";
                currentList = clothesList;
                break;
            case SlotType.坐骑:
                resPath = "rider/" + currentRiderId + ".png";
                currentList = riderList;
                break;
        }

        var path = Path.Combine(res, resPath);
        Debug.Log(path);
        var sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        currentList.Clear();
        for (var i = 0; i < sprites.Length; i++)
        {
            if (i == 0) continue;
            currentList.Add(sprites[i] as Sprite);
        }
    }
}