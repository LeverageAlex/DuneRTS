using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This is the base class for all Characterstypes to inherit from
    /// </summary>
    public abstract class Character
    {
        private string name;
        private float healthPoints;
        private int healingHP;
        private int movementPoints;
        private int actionPoints;
        private float damage;
        private bool isLoud;
        private MapField mapField;
        // TODO: add Class Inventory and reference it as a field.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public bool DecreaseHP(float hp)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HealIfHasntMoved()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        public bool SpentMP(int mp)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public bool SpentAp(int ap)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool resetMPandAp()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// this method is retuns true, if the character is dead.
        /// </summary>
        /// <returns>true, if healthpoints are equal to zero</returns>
        public bool IsDead()
        {
            if(healthPoints == 0) return true;
            return false;
        }

        /// <summary>
        /// Getter for the fild isLoud
        /// </summary>
        /// <returns>true, if the character is loud</returns>
        public bool IsLoud()
        {
            return isLoud;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StandingNextToCityField()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startField"></param>
        /// <param name="mapField"></param>
        /// <returns></returns>
        public bool Movement(MapField startField,MapField mapField)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Atack(Character target)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CollectSpice()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GiftSpice()
        {
            // TODO: implement logic
            return false;
        }
    }
}
