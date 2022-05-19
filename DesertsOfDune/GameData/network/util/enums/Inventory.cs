using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.enums
{
    /// <summary>
    /// This class resambles the Character inventory.
    /// </summary>
    public class Inventory
    {
        private int maxSize;
        private int amount;

        /// <summary>
        /// Constructor of the class Inventory
        /// </summary>
        /// <param name="maxSize">the maximum size of the inventory</param>
        /// <param name="amount">the amount within the inventory</param>
        public Inventory(int maxSize, int amount)
        {
            this.maxSize = maxSize;
            this.amount = amount; 
        }

        public int Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// This method tells weather the inventory is full or not.
        /// </summary>
        /// <returns>true, if inventory is full</returns>
        public bool IsFull()
        {
            if (amount == maxSize)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method removes spice if possible
        /// </summary>
        /// <param name="amount">the amount of spice to remove</param>
        /// <returns>true, if removal of spice was possible</returns>
        public bool RemoveSpice(int amount)
        {
            if (this.amount >= amount)
            {
                this.amount -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method adds spice to the current spiceamount if possible
        /// </summary>
        /// <param name="amount">the amount to be added</param>
        /// <returns>true, if action was possible</returns>
        public bool AddSpice(int amount)
        {
            if (this.amount + amount <= maxSize)
            {
                this.amount += amount;
            } else if (IsFull())
            {
                return false;
            } 
            return false;
        }
    }
}
