using WineManager.EntityModels;
using WineManager.DataContext;

namespace WineManager.UnitTests
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectTest()
        {
            //Arrange
            using WineManagerContext db = new();

            //Act and Assert
            Assert.True(db.Database.CanConnect(), "Should be able to connect to database");
        }

        [Fact]
        public void CheckSampleDataCreation()
        {
            //Arrange
            using WineManagerContext db = new();
            int expectedNumberOfWines = 10;

            //Act
            int actualNumberOfWines = db.Wines.Count();

            //Assert
            Assert.Equal(expectedNumberOfWines, actualNumberOfWines);
        }

        [Fact]
        public void FirstProducerNameTest()
        {
            //Arrange
            using WineManagerContext db = new();
            string expectedFirstProducerName = "Château Margaux";

            //Act
            string actualFirstProducerName = db.Producers.First().ProducerName ?? string.Empty;

            //Assert
            Assert.Equal(expectedFirstProducerName, actualFirstProducerName);
        }
    }
}