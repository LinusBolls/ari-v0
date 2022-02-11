using System;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// <see cref="Discount"/> s are being used to apply a discount on a price in a row depending on
    /// which discountKey is in a different column in that row. This way different discount can be
    /// applied on the same upload. All discount also get saved into <see
    /// cref="Constants.TableEinkauf"/>, in order to make them visible in eNVenta and not just fill
    /// in the already calculated price. The discount can be edited in the <see cref="Frontend.MainForm.tabPageDiscounts"/>.
    /// </summary>
    public class Discount : MappingObject
    {
        /// <summary>
        /// percentage! of the discount 20 = 20% discount = price*0.80
        /// </summary>
        private readonly double discount;

        /// <summary>
        /// name of this discount.
        /// </summary>
        private readonly string key;

        /// <summary>
        /// name of the <see cref="Mapping"/>, the discount belongs to.
        /// </summary>
        private readonly string mapping;

        /// <summary>
        /// Create a new Discount.
        /// </summary>
        /// <param name="mapping">name of the <see cref="Mapping"/>, the discount belongs to</param>
        /// <param name="key">name of the discount. Can't be longer than 8 chars.</param>
        /// <param name="discount">string percentage of the discount 20 = 20%</param>
        public Discount(string mapping, string key, string discount)
        {
            this.mapping = mapping;
            if(key.Length > 8)
                key = key.Substring(0, 8);//The name can't be larger than 8 chars
            this.key = key;
            this.discount = double.Parse(discount);
        }

        /// <summary>
        /// Create a new Discount.
        /// </summary>
        /// <param name="mapping">name of the <see cref="Mapping"/>, the discount belongs to</param>
        /// <param name="key">name of the discount. Can't be longer than 8 chars</param>
        /// <param name="discount">int percentage of the discount 20 = 20%</param>
        public Discount(string mapping, string key, int discount)
        {
            this.mapping = mapping;
            if(key.Length > 8)
                key = key.Substring(0, 8);//The name can't be larger than 8 chars
            this.key = key;
            this.discount = discount;
        }

        /// <summary>
        /// Find a discount by its <see cref="Mapping"/> and name.
        /// </summary>
        /// <param name="mapping">name of the <see cref="Mapping"/></param>
        /// <param name="discountKey"></param>
        /// <param name="discounts">list of <see cref="Discount"/> s</param>
        /// <returns>the found discount</returns>
        public static Discount GetDiscountByKey(string mapping, string discountKey, Discount[] discounts)
        {
            if(discountKey.Length > 8)
                discountKey = discountKey.Substring(0, 8);//The name can't be larger than 8 chars
            return Array.Find(discounts, i => mapping.Equals(i.GetMapping()) && discountKey.Equals(i.GetName()));
        }

        /// <summary>
        /// Discount Amount as int.
        /// </summary>
        /// <returns>amount</returns>
        public double GetDiscountAmount()
        {
            return discount;
        }

        /// <summary>
        /// Name of the <see cref="Mapping"/> the discount belongs to.
        /// </summary>
        /// <returns>mapping name</returns>
        public string GetMapping()
        {
            return mapping;
        }

        /// <summary>
        /// Key of the discount / name.
        /// </summary>
        /// <returns>name shortened to 8 chars</returns>
        public override string GetName()
        {
            return key; //shortened to 8 chars
        }

        /// <summary>
        /// Insert the Discount into the database <see cref="Constants.TableImportDiscounts"/>.
        /// </summary>
        /// <returns>of success</returns>
        public override SqlReport Insert()
        {
            string[] cmds = new string[]
            {
                $"insert into {Constants.TableImportDiscounts} values('{mapping}', '{key}', TO_NUMBER('{discount}'))"
            };
            using(SQL sql = new SQL())
                return sql.ExecuteCommands(cmds);
        }

        /// <summary>
        /// Delete the Discount form the database <see cref="Constants.TableImportDiscounts"/>.
        /// </summary>
        /// <returns>of success</returns>
        public override SqlReport Remove()
        {
            using(SQL sql = new SQL())
                return sql.ExecuteCommand($"delete from {Constants.TableImportDiscounts} where MAPPING='{mapping}' and KEY='{key}'");
        }

        /// <summary>
        /// Returns a string that describes the Discount Object.
        /// </summary>
        /// <returns>Description of the Discount</returns>
        public override string ToString()
        {
            return $"Discount [Mapping: {mapping}; Name: {key}; Discount: {discount}]";
        }
    }
}