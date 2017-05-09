// <copyright file="OrbwalkingMode.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Orbwalker.Modes
{
    using System.Windows.Input;

    using Ensage.SDK.Input;
    using Ensage.SDK.Service;

    public abstract class OrbwalkingMode : ControllableService, IOrbwalkingMode
    {
        protected OrbwalkingMode(IOrbwalker orbwalker)
        {
            this.Orbwalker = orbwalker;
        }

        public abstract bool CanExecute { get; }

        protected IServiceContext Context => this.Orbwalker.Context;

        protected IOrbwalker Orbwalker { get; }

        protected Hero Owner => this.Context.Owner;

        public abstract void Execute();
    }
}