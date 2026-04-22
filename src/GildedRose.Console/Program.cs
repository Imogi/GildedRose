using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<Item> Items;
        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                              new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                              new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                              new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = "Backstage passes to a TAFKAL80ETC concert",
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            var itemUpdateFactory = new ItemUpdaterFactory();
            foreach(var item in Items)
            {
                var itemUpdater = itemUpdateFactory.GetItemUpdater(item);
                itemUpdater.UpdateInformation(item);
            }
        }

    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

    public interface IItemUpdater
    {
        void UpdateInformation(Item item);
    }

    public class ItemUpdaterFactory
    {
        public IItemUpdater GetItemUpdater(Item item)
        {  
            if (item.Name == "Aged Brie")
                return new AgedBrieUpdater();

            if (item.Name == "Sulfuras, Hand of Ragnaros")
                return new SulfurasUpdater();
            
            if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                return new BackstagePassUpdater();

            if (item.Name == "Conjured Mana Cake")
                return new ConjuredItemUpdater();

            return new BasicItemUpdater();
        }
    }

    public class BasicItemUpdater : IItemUpdater
    {
        public void UpdateInformation(Item item)
        {
            item.SellIn--;

            if (item.Quality <= 0)
            {
                item.Quality = 0;
                return;
            }

            item.Quality--;

            if (item.SellIn < 0 && item.Quality > 0)
                item.Quality--;
        }
    }

    public class AgedBrieUpdater : IItemUpdater
    {
        public void UpdateInformation(Item item)
        {
            item.SellIn--;

            if (item.Quality < 50)
                item.Quality++;
            
            if (item.SellIn < 0 && item.Quality < 50)
                item.Quality++;
            
            if (item.Quality > 50)
                item.Quality = 50;
    
        }
    }
    
    public class SulfurasUpdater : IItemUpdater
    {
        public void UpdateInformation(Item item)
        {
            return;
        }
    }

    public class BackstagePassUpdater : IItemUpdater
    {
        public void UpdateInformation(Item item)
        {
            item.SellIn--;

            if (item.SellIn < 0)
            {
                item.Quality = 0;
                return;
            }

            if (item.Quality >= 50)
                return;

            if (item.SellIn < 5)
                item.Quality += 3;

            else if (item.SellIn < 10)
                item.Quality += 2;
            else
                item.Quality += 1;

            if (item.Quality > 50)
                item.Quality = 50;
        }
    }

    public class ConjuredItemUpdater : IItemUpdater
    {
        public void UpdateInformation(Item item)
        {
            item.SellIn--;

            int degrade = item.SellIn < 0 ? 4 : 2;

            item.Quality -= degrade;

            if (item.Quality < 0)
                item.Quality = 0;
            
        }
    }
}
