using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class SkinData
    {
        public List<int> purchasedSkins;
        public int currentSkinID;
        public string skinSpritePath;
    }
}