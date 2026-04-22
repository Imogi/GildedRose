using Xunit;
using GildedRose.Console;
using System.Linq;

namespace GildedRose.Tests
{

    public class GildedRoseTests
    {
        // Helper function to generate a Program with given items
        private static Program CreateProgramGivenItems(params Item[] items)
        {
            return new Program()
            {
                Items = items.ToList()
            };
        }

        #region Basic Items Tests

        [Fact]
        public void BasicItem_DecreasesQualityAndSellInByOne()
        {
            var item = new Item { Name = "Basic Item", SellIn = 5, Quality = 10 };

            var program = CreateProgramGivenItems(item);
            program.UpdateQuality();

            Assert.Equal(9, program.Items[0].Quality);
            Assert.Equal(4, program.Items[0].SellIn);
        }

        [Fact]
        public void BasicItem_QualityDegradesTwiceAfterSellInDate()
        {
            var item = new Item { Name = "Basic Item", SellIn = 0, Quality = 10 };

            var program = CreateProgramGivenItems(item);
            program.UpdateQuality();

            Assert.Equal(-1, program.Items[0].SellIn);
            Assert.Equal(8, program.Items[0].Quality);
        }

        [Fact]
        public void BasicItem_QualityNeverNegative()
        {
            var item = new Item { Name = "Basic Item", SellIn = 10, Quality = 0 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(0, program.Items[0].Quality);

            program.UpdateQuality();
            Assert.Equal(0, program.Items[0].Quality);
        }


        // Current UpdateQuality() implementation allows negative SellIn values
        [Fact]
        public void BasicItem_SellInCanGoNegative()
        {
            var item = new Item { Name = "Basic Item", SellIn = 0, Quality = 10 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(-1, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(-2, program.Items[0].SellIn);
        }

        #endregion

        #region Aged Brie item Tests

        [Fact]
        public void AgedBrie_IncreasesInQualityByOneWhenSellInDecreases()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 10, Quality = 10 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(11, program.Items[0].Quality);
            Assert.Equal(9, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(12, program.Items[0].Quality);
            Assert.Equal(8, program.Items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_IncreasesTwiceAfterSellInDate()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(12, program.Items[0].Quality);
            Assert.Equal(-1, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(14, program.Items[0].Quality);
            Assert.Equal(-2, program.Items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_QualityNeverExceedsFifty()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 1, Quality = 50 };
            
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(50, program.Items[0].Quality);
            Assert.Equal(0, program.Items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_QualityNeverExceedsFiftyEvenAfterSellInDate()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 0, Quality = 47 };
            
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(49, program.Items[0].Quality);
            Assert.Equal(-1, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(50, program.Items[0].Quality);
            Assert.Equal(-2, program.Items[0].SellIn);
        }

        #endregion
    
        #region Sulfuras Legenday item Tests
        
        [Fact]
        public void Sulfuras_NeverChangesQualityOrSellIn()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 80 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(80, program.Items[0].Quality);
            Assert.Equal(10, program.Items[0].SellIn);
        }

        [Fact]
        public void Sulfuras_NeverChangesQualityEvenAfterSellIn()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(80, program.Items[0].Quality);
            Assert.Equal(-1, program.Items[0].SellIn);
        }

        #endregion
    
        #region Back stage passes item Tests
        
        [Fact]
        public void BackstagePass_IncreasesByOne_WhenSellInGreaterThan10()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(21, program.Items[0].Quality);
            Assert.Equal(14, program.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_IncreasesByTwo_WhenSellIn10orLess()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(22, program.Items[0].Quality);
            Assert.Equal(9, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(24, program.Items[0].Quality);
            Assert.Equal(8, program.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_IncreasesByThree_WhenSellIn5orLess()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(23, program.Items[0].Quality);
            Assert.Equal(4, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(26, program.Items[0].Quality);
            Assert.Equal(3, program.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_DropsToZeroAfterConcert()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 20 }; 
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(23, program.Items[0].Quality);
            Assert.Equal(0, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(0, program.Items[0].Quality);
            Assert.Equal(-1, program.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_QualityNeverExceedsFifty()
        {
            var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 };
            var program = CreateProgramGivenItems(item);

            program.UpdateQuality();
            Assert.Equal(50, program.Items[0].Quality);
            Assert.Equal(4, program.Items[0].SellIn);

            program.UpdateQuality();
            Assert.Equal(50, program.Items[0].Quality);
            Assert.Equal(3, program.Items[0].SellIn);
        }

        #endregion
    }
}   