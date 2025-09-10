using AutoMapper;
using ContactManagerBLL;
using ContactManagerBLL.Models;
using ContactManagerBLL.Services;
using ContactManagerDAL.Entities;
using ContactManagerDAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ContactManagerTests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(_ => { }, typeof(MappingProfile).Assembly);

        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        _service = new ContactService(_repositoryMock.Object, mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsContacts()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Contact>
            {
                new() { Id = 1, Name = "Alex" },
                new() { Id = 2, Name = "Maria" }
            });

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Name == "Alex");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_IfNotFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_CallsRepository()
    {
        // Arrange
        var model = new ContactModel { Id = 1, Name = "Test" };

        // Act
        await _service.AddAsync(model);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);
    }
}