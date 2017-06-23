// <copyright file="alchemist_unstable_concoction.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Abilities.npc_dota_hero_alchemist
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Ensage.SDK.Extensions;
    using Ensage.SDK.Helpers;

    public class alchemist_unstable_concoction : RangedAbility, IHasModifier, IHasTargetModifierTexture, IAreaOfEffectAbility
    {
        public alchemist_unstable_concoction(Ability ability)
            : base(ability)
        {
            this.ThrowAbility = this.AbilityFactory.Value.GetAbility<alchemist_unstable_concoction_throw>();
        }

        public float Duration
        {
            get
            {
                return this.Ability.GetAbilitySpecialData("brew_time");
            }
        }

        public string ModifierName { get; } = "modifier_alchemist_unstable_concoction";

        public float Radius
        {
            get
            {
                return this.Ability.GetAbilitySpecialData("midair_explosion_radius");
            }
        }

        public override float Speed
        {
            get
            {
                return this.Ability.GetAbilitySpecialData("movement_speed");
            }
        }

        public string[] TargetModifierTextureName { get; } = { "modifier_alchemist_unstable_concoction_throw" };

        public alchemist_unstable_concoction_throw ThrowAbility { get; }

        [Import(typeof(AbilityFactory))]
        private Lazy<AbilityFactory> AbilityFactory { get; set; }

        public float GetDamage(float concotionDuration, params Unit[] targets)
        {
            var minDamage = this.Ability.GetAbilitySpecialData("min_damage");
            var maxDamage = this.Ability.GetAbilitySpecialData("max_damage");

            var percentage = concotionDuration / this.Duration;
            var damage = minDamage + ((maxDamage - minDamage) * percentage);
            var amp = this.Owner.GetSpellAmplification();
            var reduction = 0.0f;
            if (targets.Any())
            {
                reduction = this.Ability.GetDamageReduction(targets.First(), this.Ability.DamageType);
            }

            return DamageHelpers.GetSpellDamage(damage, amp, reduction);
        }

        public override float GetDamage(params Unit[] targets)
        {
            return this.GetDamage(this.Duration, targets);
        }

        /// <summary>
        /// Releases the unstable concoction.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool UseAbility(Unit target)
        {
            return this.ThrowAbility.CanBeCasted && this.ThrowAbility.UseAbility(target);
        }
    }
}