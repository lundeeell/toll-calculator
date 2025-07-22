using TollCalculator.Models;

namespace TollCalculator.Tests;

[TestFixture]
public class TollCalculatorTests
{
    [TestCase(18, 30, 0)]
    [TestCase(19, 0, 0)]
    [TestCase(23, 59, 0)]
    [TestCase(0, 0, 0)]
    [TestCase(5, 59, 0)]
    public void GetTollFee_NonTollHours_ReturnsZero(int hour, int minute, int expected)
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var date = new DateTime(2025, 6, 16, hour, minute, 0);

        // Act
        var result = TollCalculator.GetTollFee(car, [date]);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetTollFee_SinglePass_ReturnsTollFee()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var dates = new DateTime[] { new(2025, 6, 16, 7, 30, 0) }; // 18 SEK

        // Act
        var result = TollCalculator.GetTollFee(car, dates);

        // Assert
        Assert.That(result, Is.EqualTo(18));
    }

    [Test]
    public void GetTollFee_MultiplePasses_ReturnsSumOfFees()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var dates = new DateTime[]
        {
            new(2025, 6, 16, 6, 30, 0), // 13 SEK
            new(2025, 6, 16, 7, 30, 0), // 18 SEK
            new(2025, 6, 16, 15, 30, 0), // 18 SEK
            new(2025, 6, 16, 20, 30, 0), // 18 SEK
        };

        // Act
        var result = TollCalculator.GetTollFee(car, dates);

        // Assert
        Assert.That(result, Is.EqualTo(49));
    }

    [Test]
    public void GetTollFee_ExceedsMaximum_Returns60SEK()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var dates = new DateTime[]
        {
            new(2025, 6, 16, 6, 30, 0), // 13 SEK
            new(2025, 6, 16, 7, 30, 0), // 18 SEK
            new(2025, 6, 16, 15, 30, 0), // 18 SEK
            new(2025, 6, 16, 17, 30, 0), // 13 SEK
        };

        // Act
        var result = TollCalculator.GetTollFee(car, dates);

        // Assert
        Assert.That(result, Is.EqualTo(60));
    }

    [TestCase(VehicleType.Motorbike)]
    [TestCase(VehicleType.Tractor)]
    [TestCase(VehicleType.Diplomat)]
    [TestCase(VehicleType.Emergency)]
    [TestCase(VehicleType.Military)]
    [TestCase(VehicleType.Tractor)]
    [TestCase(VehicleType.Foreign)]
    public void GetTollFee_TollFreeVehicle_ReturnsZero(VehicleType vehicleType)
    {
        // Arrange
        var vehicle = new Vehicle(vehicleType);
        DateTime[] date = [new(2025, 6, 16, 7, 30, 0)]; // 18 SEK

        // Act
        var result = TollCalculator.GetTollFee(vehicle, date);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_EmptyDateArray_ReturnsZero()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var dates = Array.Empty<DateTime>();

        // Act & Assert
        var result = TollCalculator.GetTollFee(car, dates);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_WithinSameHour_ReturnsHighestFee()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var dates = new DateTime[]
        {
            new(2025, 6, 16, 6, 15, 0), // 8 SEK
            new(2025, 6, 16, 6, 45, 0), // 13 SEK
            new(2025, 6, 16, 7, 0, 0), // 18 SEK (Highest)
            new(2025, 6, 16, 7, 18, 0), // 18 SEK (different hour)
        };

        // Act
        var result = TollCalculator.GetTollFee(car, dates);

        // Assert
        Assert.That(result, Is.EqualTo(36));
    }

    [TestCase(2025, 1, 1)] // Nyårsdagen
    [TestCase(2025, 1, 6)] // Trettondagen
    [TestCase(2025, 4, 18)] // Långfredagen
    [TestCase(2025, 4, 21)] // Annandagen
    [TestCase(2025, 5, 1)] // Första maj
    [TestCase(2025, 5, 29)] // kristi himmelfärd
    [TestCase(2025, 6, 6)] // Nationaldagen
    [TestCase(2025, 6, 20)] // Midsommarafton
    [TestCase(2025, 12, 24)] // Julafton
    [TestCase(2025, 12, 25)] // Juldagen
    [TestCase(2025, 12, 26)] // Annandagen
    [TestCase(2025, 12, 31)] // Nyårsafton
    [TestCase(2025, 05, 10)] // Saturday
    [TestCase(2025, 05, 11)] // Sunday
    public void GetTollFee_TollFreeDays_ReturnsZero(int year, int month, int day)
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);
        var holiday = new DateTime(year, month, day, 7, 30, 0);

        // Act
        var result = TollCalculator.GetTollFee(car, [holiday]);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetTollFee_July_AllDaysTollFree()
    {
        // Arrange
        var car = new Vehicle(VehicleType.Car);

        // Act
        var totalTollFee = Enumerable
            .Range(1, 31)
            .Select(day => new DateTime(2025, 7, day, 7, 30, 0))
            .Sum(d => TollCalculator.GetTollFee(car, [d]));

        // Assert
        Assert.That(totalTollFee, Is.EqualTo(0));
    }
}
