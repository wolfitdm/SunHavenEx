// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.KryChestLabel
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using KryziK.SunHaven.ChestLabels.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wish;
using PSS;

namespace KryziK.SunHaven.ChestLabels
{
  internal class KryChestLabel : MonoBehaviour
  {
    private static (Color32 color, Color32 outlineColor)[] ChestColors { get; } = ((IEnumerable<(int, int)>) new (int, int)[11]
    {
      (6045747, 3021313),
      (8723740, 3476491),
      (14375446, 4529159),
      (13220101, 4735746),
      (5403146, 2107141),
      (224944, 1908533),
      (6098836, 2759479),
      (13334429, 5838661),
      (16776438, 1840926),
      (7237744, 1840926),
      (2761770, 854797)
    }).Select<(int, int), (Color32, Color32)>((Func<(int, int), (Color32, Color32)>) (hex => (hex.Item1.ToColor(), hex.Item2.ToColor()))).ToArray<(Color32, Color32)>();

    private Canvas Canvas { get; set; }

    private Image Image { get; set; }

    private TextMeshProUGUI Label { get; set; }

    private KryHitbox KryHitbox { get; set; }

    public bool HasImage { get; set; }

    public bool PlayerOver { get; private set; }

    public KryChestLabel Init()
    {
      BoxCollider2D component = this.GetComponent<BoxCollider2D>();
      TMP_FontAsset font = SingletonBehaviour<DayCycle>.Instance.GetYearUI()?.font;
      this.Canvas = new GameObject("KryWorldCanvas").AddComponent<Canvas>();
      this.Canvas.renderMode = RenderMode.WorldSpace;
      this.Canvas.transform.SetParent(this.transform, false);
      this.Canvas.transform.localPosition = Vector3.zero;
      this.Canvas.transform.eulerAngles = new Vector3(315f, 0.0f, 0.0f);
      this.Canvas.GetComponent<RectTransform>().sizeDelta = component.size;
      this.Label = new GameObject(nameof (KryChestLabel)).AddComponent<TextMeshProUGUI>();
      this.Label.transform.SetParent(this.Canvas.transform, false);
      this.Label.GetComponent<RectTransform>().sizeDelta = component.size * 1.75f;
      this.Label.transform.localPosition = component.bounds.center - this.Canvas.transform.position + new Vector3(0.0f, 0.9f, 0.0f);
      this.Label.alignment = TextAlignmentOptions.Center;
      this.Label.enableAutoSizing = true;
      this.Label.enableWordWrapping = false;
      if ((UnityEngine.Object) font != (UnityEngine.Object) null)
        this.Label.font = font;
      else
        ChestLabelsPlugin.Instance.Logger.LogError((object) "Could not find font to use for Chest Labels.");
      this.Label.fontSizeMin = 0.3f;
      this.Label.fontSizeMax = 0.5f;
      this.Label.isOverlay = true;
      this.Label.outlineWidth = 0.15f;
      this.Image = new GameObject("KryChestImage").AddComponent<Image>();
      this.Image.transform.SetParent(this.Canvas.transform, false);
      this.Image.GetComponent<RectTransform>().sizeDelta = Vector2.one * 0.75f;
      this.Image.transform.localPosition = new Vector3(component.bounds.center.x - this.Canvas.transform.position.x, 0.5f, -0.1f);
      this.Image.preserveAspect = true;
      this.KryHitbox = new GameObject("KryHitbox").AddComponent<KryHitbox>();
      this.KryHitbox.transform.SetParent(this.transform, false);
      this.KryHitbox.transform.localPosition = new Vector3(0.0f, -0.2f, -0.3f);
      return this;
    }

    public void DoUpdate()
    {
      ChestData data = this.transform.GetComponentInParent<Chest>().GetData();
      this.SetTextAndIcon(data.name, data.color);
    }

    public string GetText() => this.Label.text;

    public static void ChestLabelsItemDataFunc(KryChestLabel instance, ItemData i)
    {
            instance.Image.sprite = i.icon;
            instance.HasImage = true;
    }

    public static void ChestLabelItemDataFailedFunc()
    {
            return;
    }
    public void SetTextAndIcon(string text, int color)
    {
      string[] strArray = (text ?? (text = "")).Split(new char[1]
      {
        ' '
      }, 2);
      TextMeshProUGUI label1 = this.Label;
      TextMeshProUGUI label2 = this.Label;
      (Color32 color, Color32 outlineColor) chestColor = KryChestLabel.ChestColors[color];
      Color color1 = (Color) chestColor.color;
      label1.color = color1;
      Color32 outlineColor = chestColor.outlineColor;
      label2.outlineColor = outlineColor;
      int result;
      Action<ItemData> itemDataFunc = (i) => ChestLabelsItemDataFunc(this, i);
      Action itemFailed = () => ChestLabelItemDataFailedFunc();
            if (strArray.Length == 1 || !int.TryParse(strArray[0], out result) || !Database.ValidID(result))
      {
        this.Label.text = text;
        this.HasImage = false;
      }
      else
      {
        Database.GetData<ItemData>(result,itemDataFunc, itemFailed);
        this.Label.text = strArray[strArray.Length - 1];
      }
    }

    private void LateUpdate()
    {
      this.Label.enabled = this.ShouldBeVisible(ChestLabelsPlugin.LabelVisibility.Value);
      this.Image.enabled = this.HasImage && this.ShouldBeVisible(ChestLabelsPlugin.IconVisibility.Value);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
      if (!((UnityEngine.Object) collider.gameObject == (UnityEngine.Object) Player.Instance.gameObject))
        return;
      this.PlayerOver = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
      if (!((UnityEngine.Object) collider.gameObject == (UnityEngine.Object) Player.Instance.gameObject))
        return;
      this.PlayerOver = false;
    }

    private bool ShouldBeVisible(ComponentVisibility visibility)
    {
      bool flag;
      int num;
      switch (visibility)
      {
        case ComponentVisibility.Hidden:
          flag = false;
          goto label_6;
        case ComponentVisibility.OnHover:
          num = this.KryHitbox.MouseOver ? 1 : (this.PlayerOver ? 1 : 0);
          break;
        case ComponentVisibility.Visible:
          flag = true;
          goto label_6;
        default:
          num = 0;
          break;
      }
      flag = num != 0;
label_6:
      return flag;
    }
  }
}
