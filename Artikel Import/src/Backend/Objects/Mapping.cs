using Artikel_Import.src.Backend.Objects.PairTypes;
using System.Collections.Generic;
using System.Linq;

namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// Collection of <see cref="Pair"/> s used for showing relation of a certain CSV file and the
    /// TempDB. Also contains <see cref="Discount"/> and <see cref="CustomDictionary"/>.
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// A collection of <see cref="Pair"/> s every mapping needs to have.
        /// </summary>
        public readonly Pair[] essentialPairs = new Pair[] {
            new PairFixedValue("", false, "ArtikelGruppe", "empty"),
            new PairCsvColumn("", false, "ArtikelNr", "empty"),
            new PairCsvColumn("",false, "BestellNr", "empty"),
            new PairCsvColumn("", false, "Code1", "empty"),
            new PairChangingFixedValue("",false, "GueltigBis", "empty"),
            new PairChangingFixedValue("",false, "GueltigVon", "empty"),
            new PairFixedValue("",false, "DatenLieferant", "empty"),
            new PairCsvColumn("",false, "EAN", "empty"),
            new PairFixedValue("",false, "PreisGruppeEk", "861"),
            new PairFixedValue("",false, "PreisGruppeVk", "761"),
            new PairFixedValue("",false, "EkPro", "1"),
            new PairFixedValue("",false, "VkPro", "1"),
            new PairCsvColumn("",true, "EK", "empty"),
            new PairCsvColumn("",false, "VK1_EDE", "empty"),
            new PairCsvColumn("",true, "VK2", "empty"),
            new PairCsvColumn("", false, "VK3", "empty"),
            new PairCsvColumn("", false, "LEK", "empty"),
            new PairCsvColumn("",false, "PG_VkPreis", "empty"),
            new PairCsvColumn("",false, "PG_EkPreis", "empty"),
            new PairCsvColumn("",false, "EinheitEinkauf", "empty"),
            new PairCsvColumn("",false, "EinheitLager", "empty"),
            new PairCsvColumn("",false, "EinheitVerkauf", "empty"),
            //new PairCsvColumn("",false, "EinheitVerpackung", "empty"),
            new PairCsvColumn("",false, "Bezeichnung", "empty"),
            new PairFixedValue("",false, "LieferantenNr", "empty")
        };

        /// <summary>
        /// Dictionary of all <see cref="CustomDictionary"/> linked to this mapping. The name is the
        /// name of the <see cref="CustomDictionary"/>.
        /// </summary>
        public Dictionary<string, CustomDictionary> dictionaries = new Dictionary<string, CustomDictionary>();

        private readonly string name;
        private Discount[] discounts;
        private Pair[] pairs;

        /// <summary>
        /// Empty collection of <see cref="Pair"/> s and <see cref="Discount"/> s
        /// </summary>
        /// <param name="name">how the mapping gets referenced</param>
        public Mapping(string name)
        {
            this.name = name;
            pairs = LoadPairs();
            discounts = LoadDiscounts();

            if(pairs.Length == 0)
            {
                //the mapping gets created new
                foreach(Pair pair in essentialPairs)
                {
                    pair.SetMappingName(name);
                    Add(pair);
                }
            }
            foreach(string dict in CustomDictionary.GetList(name))
            {
                dictionaries.Add(dict, new CustomDictionary(name, dict));
            }
        }

        /// <summary>
        /// A collection of <paramref name="pairs"/> and empty <see cref="Discount"/> s
        /// </summary>
        /// <param name="name">how the mapping gets referenced</param>
        /// <param name="pairs">are set to the mapping</param>
        public Mapping(string name, Pair[] pairs)
        {
            this.name = name;
            this.pairs = pairs;
            foreach(string dict in CustomDictionary.GetList(name))
            {
                dictionaries.Add(dict, new CustomDictionary(name, dict));
            }
        }

        /// <summary>
        /// Loads all mappings from the database <see cref="Constants.TableImportMappings"/>
        /// </summary>
        /// <returns>mapping array containing all mappings from the database</returns>
        public static Mapping[] GetMappings()
        {
            List<Mapping> m = new List<Mapping>();
            using(SQL sql = new SQL())
            {
                string[] names = sql.ExecuteQuery($"select NAME from {Constants.TableImportMappings} group by NAME order by NAME");
                for(int i = 0;i < names.Count();i++)
                {
                    Mapping mapping = new Mapping(names[i]);
                    m.Add(mapping);
                }
                return m.ToArray();
            }
        }

        /// <summary>
        /// Adds a <paramref name="discount"/> to the mapping and inserts it in the database <see cref="Constants.TableImportDiscounts"/>
        /// </summary>
        /// <param name="discount">to add</param>
        public void Add(Discount discount)
        {
            List<Discount> temp = discounts.ToList();
            temp.Add(discount);
            temp.Sort(delegate (Discount a, Discount b)
            {
                return a.GetName().CompareTo(b.GetName());
            });
            discounts = temp.ToArray();
            discount.Insert();
        }

        /// <summary>
        /// Adds a <paramref name="pair"/> to the mapping and inserts it in the database <see cref="Constants.TableImportMappings"/>
        /// </summary>
        /// <param name="pair">to get added</param>
        /// <returns>true when was successful</returns>
        public bool Add(Pair pair)
        {
            List<Pair> temp = pairs.ToList();
            temp.Add(pair);
            temp.Sort(delegate (Pair a, Pair b)
            {
                return a.GetTargetField().CompareTo(b.GetTargetField());
            });
            pairs = temp.ToArray();
            SqlReport report = pair.Insert();
            return report.WasSuccessful();
        }

        /// <summary>
        /// Deletes the mapping from the database <see cref="Constants.TableImportMappings"/> and
        /// all the <see cref="Pair"/> s, <see cref="Discount"/> s and <see
        /// cref="CustomDictionary"/> s associated with it.
        /// </summary>
        public void Delete()
        {
            foreach(Discount discount in discounts)
            {
                Delete(discount);
            }
            foreach(string dictionaryName in dictionaries.Keys)
            {
                dictionaries[dictionaryName].Remove();
            }
            string[] cmds = new string[] { $"delete from {Constants.TableImportMappings} where NAME='{name}'" };
            using(SQL sql = new SQL())
                sql.ExecuteCommands(cmds);
        }

        /// <summary>
        /// Deletes a <paramref name="discount"/> from the mapping and database <see cref="Constants.TableImportDiscounts"/>.
        /// </summary>
        /// <param name="discount"><see cref="Discount"/> to delete</param>
        public void Delete(Discount discount)
        {
            List<Discount> temp = discounts.ToList();
            temp.Remove(discount);
            discounts = temp.ToArray();
            discount.Remove();
        }

        /// <summary>
        /// Removes the <paramref name="pair"/> from the mapping and deletes it from the database
        /// <see cref="Constants.TableImportMappings"/>.
        /// </summary>
        /// <param name="pair"><see cref="Pair"/> to delete</param>
        public void Delete(Pair pair)
        {
            List<Pair> temp = pairs.ToList();
            pairs = temp.Where(p => p.GetTargetField() != pair.GetTargetField()).ToArray();
            pair.Remove();
        }

        /// <summary>
        /// <see cref="Discount"/> s of this mapping
        /// </summary>
        /// <returns>discount array</returns>
        public Discount[] GetDiscounts()
        {
            return discounts;
        }

        /// <summary>
        /// Name of this Mapping.
        /// </summary>
        /// <returns>name</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// <see cref="Pair"/> s of this mapping.
        /// </summary>
        /// <returns>pair array</returns>
        public Pair[] GetPairs()
        {
            return pairs;
        }

        /// <summary>
        /// Get <see cref="Discount"/> s associated with this mapping form the database <see cref="Constants.TableImportDiscounts"/>.
        /// </summary>
        /// <returns>discount array containing all discounts associated with this mapping</returns>
        public Discount[] LoadDiscounts()
        {
            List<Discount> discountsList = new List<Discount>();
            using(SQL sql = new SQL())
            {
                string[][] discountResponse = sql.ExecuteMultiLineQuery($"select KEY, DISCOUNT from {Constants.TableImportDiscounts} where MAPPING='{name}' order by KEY", 2);
                for(int i = 0;i < discountResponse.Count();i++)
                {
                    discountsList.Add(new Discount(
                        name,
                        discountResponse[i][0],
                        discountResponse[i][1]));
                }
                discountsList.Sort(delegate (Discount a, Discount b)
                {
                    return a.GetName().CompareTo(b.GetName());
                });
                discounts = discountsList.ToArray();
                return discounts;
            }
        }

        /// <summary>
        /// Get <see cref="Pair"/> s associated with this mapping from the database <see cref="Constants.TableImportMappings"/>.
        /// </summary>
        /// <returns>pair array of pairs from the database</returns>
        public Pair[] LoadPairs()
        {
            List<Pair> pairsList = new List<Pair>();
            using(SQL sql = new SQL())
            {
                string[][] pairResponse = sql.ExecuteMultiLineQuery($"select NAME, PAIR_TYPE, PAIR_TARGET_FIELD, PAIR_SOURCE_FIELD, OVERWRITE, FACTOR from {Constants.TableImportMappings} where NAME='{name}' order by PAIR_TARGET_FIELD", 6);
                for(int i = 0;i < pairResponse.Count();i++)
                {
                    string mappingName = pairResponse[i][0];
                    string pairType = pairResponse[i][1];
                    string targetField = pairResponse[i][2];
                    string source = pairResponse[i][3];
                    string overwrite = pairResponse[i][4];
                    string additionalSource = pairResponse[i][5];
                    Pair pair = Pair.GetUnknownPair(mappingName, pairType, targetField, source, overwrite, additionalSource);
                    pairsList.Add(pair);
                }
                pairsList.Sort(delegate (Pair a, Pair b)
                {
                    return a.GetTargetField().CompareTo(b.GetTargetField());
                });
                pairs = pairsList.ToArray();
                return pairs;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this mapping.
        /// </summary>
        /// <returns>string describing the mapping</returns>
        public override string ToString()
        {
            return $"Mapping {name} [Pairs: {pairs.Length}; Discounts: {discounts.Length}; Dictionaries: {dictionaries.Count}]";
        }
    }
}