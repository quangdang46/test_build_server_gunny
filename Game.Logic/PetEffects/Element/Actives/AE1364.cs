﻿using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1364 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public AE1364(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1364, elementID)
        {
            m_count = count;
            m_coldDown = count;
            m_probability = probability == -1 ? 10000 : probability;
            m_type = type;
            m_delay = delay;
            m_currentId = skillId;
        }

        public override bool Start(Living living)
        {
            AE1364 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1364) as AE1364;
            if (effect != null)
            {
                effect.m_probability = m_probability > effect.m_probability ? m_probability : effect.m_probability;
                return true;
            }
            else
            {
                return base.Start(living);
            }
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += Player_PlayerBuffSkillPet;      
        }

        private void Player_PlayerBuffSkillPet(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            {                
                List<Living> allies = player.Game.Map.FindAllNearestSameTeam(player.X, player.Y, 250, player);
                foreach (Living ally in allies)
                {
                    m_added = ally.MaxBlood * 8 / 100;
                    ally.SyncAtTime = true;
                    ally.AddBlood(m_added);
                    ally.SyncAtTime = false;
                }
                m_added = player.MaxBlood * 8 / 100;
                player.SyncAtTime = true;
                player.AddBlood(m_added);
                player.SyncAtTime = false;
            }
        }        

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
        }         
    }
}
