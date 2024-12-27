// Decompiled with JetBrains decompiler
// Type: KryziK.Utilities.FuckUnityObject`1
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

namespace KryziK.Utilities
{
  internal class FuckUnityObject<T> where T : UnityEngine.Object
  {
    public T Value
    {
      get
      {
        return !((UnityEngine.Object) this.InternalInstance == (UnityEngine.Object) null) ? this.InternalInstance : (this.InternalInstance = this.Func());
      }
    }

    private System.Func<T> Func { get; }

    private T InternalInstance { get; set; }

    public FuckUnityObject(System.Func<T> func) => this.Func = func;
  }
}
