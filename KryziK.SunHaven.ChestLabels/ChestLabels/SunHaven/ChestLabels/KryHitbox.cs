// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.KryHitbox
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using UnityEngine;

namespace KryziK.SunHaven.ChestLabels
{
  internal class KryHitbox : MonoBehaviour
  {
    public bool MouseOver { get; private set; }

    private BoxCollider2D MouseCollider { get; set; }

    private void Start()
    {
      this.MouseCollider = this.gameObject.AddComponent<BoxCollider2D>();
      this.MouseCollider.isTrigger = true;
      this.MouseCollider.size = new Vector2(1.8f, 2f);
      this.MouseCollider.offset = new Vector2(0.6667f, 0.6667f);
    }

    private void OnMouseEnter() => this.MouseOver = true;

    private void OnMouseExit() => this.MouseOver = false;
  }
}
