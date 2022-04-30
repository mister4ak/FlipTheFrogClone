﻿using System.Collections.Generic;
using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Obstacles;

namespace CodeBase.Data
{
    public class LevelData
    {
        public Coin[] coins;
        public List<ObstacleMover> movingObstacles;
        public FinishLine finishLine;
    }
}