using Xunit;
using GildedRose.Console;
using System.Linq;

namespace GildedRose.Tests
{

    public class GildedRoseTests
    {
        // Helper function to generate a Program with given items
        private Program CreateProgramGivenItems(params Item[] items)
        {
            return new Program()
            {
                Items = items.ToList()
            };
        }

        #region Basic Items

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
    }
}   