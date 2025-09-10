using System.Text;
using AutoMapper;
using ContactManagerBLL;
using ContactManagerBLL.Services;
using ContactManagerDAL.Entities;
using ContactManagerDAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ContactManagerTests;

public class ContactServiceCsvTests
{
    private readonly Mock<IContactRepository> _repoMock;
    private readonly ContactService _service;

    public ContactServiceCsvTests()
    {
        _repoMock = new Mock<IContactRepository>();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(_ => {}, typeof(MappingProfile).Assembly);

        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        _service = new ContactService(_repoMock.Object, mapper);
    }

    [Fact]
    public async Task ImportCsvAsync_ShouldImportValidCsv()
    {
        // Arrange
        var csv = @"Name,DateOfBirth,Married,Phone,Salary
Alex Johnson,1990-05-12,true,+1234567890,50000
Maria Smith,1985-11-23,false,+9876543210,70000";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        _repoMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Contact>>()))
            .Returns(Task.CompletedTask);

        // Act
        var (imported, errors) = await _service.ImportCsvAsync(stream);

        // Assert
        Assert.Equal(2, imported);
        Assert.Empty(errors);
        _repoMock.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<Contact>>()), Times.Once);
    }
}