﻿namespace AgarIo.Server.Logic.Spawners
{
    using System;

    using AgarIo.Server.Logic.Blobs;
    using AgarIo.Server.Logic.Physics;

    public class FoodSpawner : EntitiesSpawner
    {
        private readonly IStateTracker _stateTracker;

        public FoodSpawner(IGame game, IPhysics physics, IStateTracker stateTracker) : base(game, physics)
        {
            _stateTracker = stateTracker;
        }

        protected override void SpawnEntity()
        {
            var position = Game.GetRandomPosition();

            var foodBlob = new FoodBlob(Game, Physics, _stateTracker, position);

            Game.AddBlob(foodBlob);
        }

        protected override Type[] SpawnTypes => new[] { typeof(FoodBlob) };

        protected override uint SpawnTicksInterval => Game.Settings.FoodSpawnTicksInterval;

        protected override int SpawnAmount => Game.Settings.FoodSpawnAmount;

        protected override int MaxSpawnAmount => Game.Settings.MaxFoodCount;

        protected override int InitialSpawnAmount => Game.Settings.InitialFoodAmount;
    }
}