﻿using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1181 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public AE1181(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1181, elementID)
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
            AE1181 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1181) as AE1181;
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
            player.PlayerBuffSkillPet += new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
        }

        void player_AfterBuffSkillPetByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            {
                List<Player> allies = player.Game.GetAllTeamPlayers(player);
                int currentCount = 2;
                foreach (Player ally in allies)
                {
                    currentCount = 2;
                    CE1181 effect1 = ally.PetEffectList.GetOfType(ePetEffectType.CE1181) as CE1181;
                    CE1182 effect2 = ally.PetEffectList.GetOfType(ePetEffectType.CE1182) as CE1182;
                    CE1183 effect3 = ally.PetEffectList.GetOfType(ePetEffectType.CE1183) as CE1183;
                    if (effect1 != null)
                    {
                        currentCount = effect1.Count;
                        effect1.Stop();
                    }
                    if (effect2 != null)
                    {
                        currentCount = effect2.Count;
                        effect2.Stop();
                    }
                    if (effect3 != null)
                    {
                        currentCount = effect3.Count;
                        effect3.Stop();
                    }

                    ally.Game.SendPetBuff(player, ElementInfo, true, 1);
                    ally.AddPetEffect(new CE1181(currentCount, m_probability, m_type, m_currentId, m_delay, ElementInfo.ID.ToString()), 0);

                }
                //Console.WriteLine("Buff Name: {2}, ID: {0}, player.CurrentDamagePlus: {1}", ElementInfo.ID, player.CurrentDamagePlus, ElementInfo.Name);
            }
        }      
    }
}
