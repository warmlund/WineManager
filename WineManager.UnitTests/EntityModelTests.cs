using WineManager.EntityModels;
using WineManager.DataContext;

namespace WineManager.UnitTests
{
    public class EntityModelTests
    {
        /// <summary>
        /// Tests that the database connection works
        /// </summary>
        [Fact]
        public void DatabaseConnectTest()
        {
            //Arrange
            using WineManagerContext db = new();

            //Act and Assert
            Assert.True(db.Database.CanConnect(), "Should be able to connect to database");
        }


        /// <summary>
        /// Checks that the number of wines in the database are correct
        /// </summary>
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

        /// <summary>
        /// Checks that the first name of the wine is correct
        /// </summary>
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