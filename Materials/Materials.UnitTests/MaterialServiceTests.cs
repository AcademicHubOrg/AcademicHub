namespace Materials.UnitTests;
using System;
using Materials.Data;
using Materials.Core;
using CustomExceptions;
using Moq;

public class MaterialServiceTests
{
    [Fact]
    public async Task TestAddValidMaterial()
    {
        // Arrange
        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<MaterialData>());
        mockRepository.Setup(r => r.AddAsync(It.IsAny<MaterialData>())).Returns(Task.CompletedTask);

        var materialService = new MaterialService(mockRepository.Object);
        var newMaterial = new MaterialDataDtoAdd
        {
            Name = "New Material",
            DataText = "Some data text",
            CourseId = "1"
        };

        // Act
        await materialService.AddAsync(newMaterial);

        // Assert
        mockRepository.Verify(r => r.AddAsync(It.Is<MaterialData>(m =>
            m.MaterialName == newMaterial.Name &&
            m.DataText == newMaterial.DataText &&
            m.CourseId == Convert.ToInt32(newMaterial.CourseId))), Times.Once());
    }

    [Fact]
    public async Task TestAddMaterialWithExistingNameAndCourseId()
    {
        // Arrange
        var existingMaterial = new MaterialData
        {
            MaterialName = "Existing Material",
            DataText = "Existing data text",
            CourseId = 1
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<MaterialData> { existingMaterial });

        var materialService = new MaterialService(mockRepository.Object);
        var newMaterial = new MaterialDataDtoAdd
        {
            Name = "Existing Material",
            DataText = "Some new data text",
            CourseId = "1"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConflictException>(() => materialService.AddAsync(newMaterial));
        Assert.Equal($"The course with id: '1' contains a material with the name 'Existing Material'", exception.Message);
    }

    [Fact]
    public async Task TestAddValidEssentialMaterial()
    {
        // Arrange
        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(new List<EssentialMaterial>());
        mockRepository.Setup(r => r.AddEssentialAsync(It.IsAny<EssentialMaterial>())).Returns(Task.CompletedTask);

        var materialService = new MaterialService(mockRepository.Object);
        var newEssentialMaterial = new MaterialDataDtoAdd
        {
            Name = "New Essential Material",
            DataText = "Essential data text",
            CourseId = "101"
        };

        // Act
        await materialService.AddEssentialAsync(newEssentialMaterial);

        // Assert
        mockRepository.Verify(r => r.AddEssentialAsync(It.Is<EssentialMaterial>(m =>
            m.MaterialName == newEssentialMaterial.Name &&
            m.DataText == newEssentialMaterial.DataText &&
            m.TemplateId == Convert.ToInt32(newEssentialMaterial.CourseId))), Times.Once());
    }

    [Fact]
    public async Task TestAddEssentialMaterialWithExistingNameAndTemplateId()
    {
        // Arrange
        var existingEssentialMaterial = new EssentialMaterial
        {
            MaterialName = "Existing Essential Material",
            DataText = "Existing data text",
            TemplateId = 102
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(new List<EssentialMaterial> { existingEssentialMaterial });

        var materialService = new MaterialService(mockRepository.Object);
        var newEssentialMaterial = new MaterialDataDtoAdd
        {
            Name = "Existing Essential Material",
            DataText = "Some new data text",
            CourseId = "102"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConflictException>(() => materialService.AddEssentialAsync(newEssentialMaterial));
        Assert.Equal($"The template with id: '102' contains a material with the name 'Existing Essential Material'", exception.Message);
    }

    [Fact]
    public async Task TestListMaterialsEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<MaterialData>());

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task TestListMaterialsNonEmpty()
    {
        // Arrange
        var mockMaterials = new List<MaterialData>
        {
            new MaterialData { MaterialName = "Material 1", DataText = "Text 1", CourseId = 1 },
            new MaterialData { MaterialName = "Material 2", DataText = "Text 2", CourseId = 2 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(mockMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Ensure two materials are returned
        Assert.Contains(result, m => m.Name == "Material 1" && m.DataText == "Text 1" && m.CourseId == "1");
        Assert.Contains(result, m => m.Name == "Material 2" && m.DataText == "Text 2" && m.CourseId == "2");
    }

    [Fact]
    public async Task TestListEssentialsEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(new List<EssentialMaterial>());

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListEssentialsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task TestListEssentialsNonEmpty()
    {
        // Arrange
        var mockEssentials = new List<EssentialMaterial>
        {
            new EssentialMaterial { MaterialName = "Essential Material 1", DataText = "Essential Text 1", TemplateId = 101 },
            new EssentialMaterial { MaterialName = "Essential Material 2", DataText = "Essential Text 2", TemplateId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(mockEssentials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListEssentialsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Ensure two essential materials are returned
        Assert.Contains(result, m => m.Name == "Essential Material 1" && m.DataText == "Essential Text 1" && m.CourseId == "101");
        Assert.Contains(result, m => m.Name == "Essential Material 2" && m.DataText == "Essential Text 2" && m.CourseId == "102");
    }

    [Fact]
    public async Task TestListByValidCourseId()
    {
        // Arrange
        int testCourseId = 1;
        var mockMaterials = new List<MaterialData>
        {
            new MaterialData { MaterialName = "Material 1", DataText = "Text 1", CourseId = testCourseId },
            new MaterialData { MaterialName = "Material 2", DataText = "Text 2", CourseId = testCourseId },
            new MaterialData { MaterialName = "Material 3", DataText = "Text 3", CourseId = 2 } // Different CourseId
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(mockMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListByCourseIdAsync(testCourseId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Only materials with testCourseId should be returned
        Assert.All(result, m => Assert.Contains("Material", m.Name)); // Check if the name contains "Material"
        Assert.All(result, m => Assert.Contains("Text", m.DataText)); // Check if the DataText contains "Text"
    }

    [Fact]
    public async Task TestListByInvalidCourseId()
    {
        // Arrange
        int invalidCourseId = 999; // An ID that does not exist in the repository
        var mockMaterials = new List<MaterialData>
        {
            new MaterialData { MaterialName = "Material 1", DataText = "Text 1", CourseId = 1 },
            new MaterialData { MaterialName = "Material 2", DataText = "Text 2", CourseId = 2 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(mockMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => materialService.ListByCourseIdAsync(invalidCourseId));
        Assert.Contains($"Material with course ID: '{invalidCourseId}'", exception.Message);
    }

    [Fact]
    public async Task TestListByValidTemplateId()
    {
        // Arrange
        int validTemplateId = 101;
        var mockEssentialMaterials = new List<EssentialMaterial>
        {
            new EssentialMaterial { MaterialName = "Essential Material 1", DataText = "Essential Text 1", TemplateId = validTemplateId },
            new EssentialMaterial { MaterialName = "Essential Material 2", DataText = "Essential Text 2", TemplateId = validTemplateId },
            new EssentialMaterial { MaterialName = "Essential Material 3", DataText = "Essential Text 3", TemplateId = 102 } // Different TemplateId
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(mockEssentialMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListByTemplateIdAsync(validTemplateId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Only essential materials with validTemplateId should be returned
        Assert.All(result, m => Assert.Contains("Essential Material", m.Name)); // Check if the name contains "Essential Material"
        Assert.All(result, m => Assert.Contains("Essential Text", m.DataText)); // Check if the DataText contains "Essential Text"
    }

    [Fact]
    public async Task TestListByInvalidTemplateId()
    {
        // Arrange
        int invalidTemplateId = 999; // An ID that does not exist in the repository
        var mockEssentialMaterials = new List<EssentialMaterial>
        {
            new EssentialMaterial { MaterialName = "Essential Material 1", DataText = "Essential Text 1", TemplateId = 101 },
            new EssentialMaterial { MaterialName = "Essential Material 2", DataText = "Essential Text 2", TemplateId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(mockEssentialMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => materialService.ListByTemplateIdAsync(invalidTemplateId));
        Assert.Contains($"Material with template ID: '{invalidTemplateId}'", exception.Message);
    }

    [Fact]
    public async Task TestListByValidMaterialId()
    {
        // Arrange
        int validMaterialId = 1;
        var mockMaterials = new List<MaterialData>
        {
            new MaterialData { Id = validMaterialId, MaterialName = "Material 1", DataText = "Text 1", CourseId = 101 },
            new MaterialData { Id = 2, MaterialName = "Material 2", DataText = "Text 2", CourseId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(mockMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListByIdAsync(validMaterialId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // Ensure only one material is returned
        var material = result.First();
        Assert.Equal("Material 1", material.Name);
        Assert.Equal("Text 1", material.DataText);
    }

    [Fact]
    public async Task TestListByInvalidMaterialId()
    {
        // Arrange
        int invalidMaterialId = 999; // An ID that does not exist in the repository
        var mockMaterials = new List<MaterialData>
        {
            new MaterialData { Id = 1, MaterialName = "Material 1", DataText = "Text 1", CourseId = 101 },
            new MaterialData { Id = 2, MaterialName = "Material 2", DataText = "Text 2", CourseId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListAsync()).ReturnsAsync(mockMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => materialService.ListByIdAsync(invalidMaterialId));
        Assert.Contains($"Material with ID: '{invalidMaterialId}'", exception.Message);
    }

    [Fact]
    public async Task TestListEssentialsByValidMaterialId()
    {
        // Arrange
        int validMaterialId = 1;
        var mockEssentialMaterials = new List<EssentialMaterial>
        {
            new EssentialMaterial { Id = validMaterialId, MaterialName = "Essential Material 1", DataText = "Essential Text 1", TemplateId = 101 },
            new EssentialMaterial { Id = 2, MaterialName = "Essential Material 2", DataText = "Essential Text 2", TemplateId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(mockEssentialMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act
        var result = await materialService.ListEssentialsByIdAsync(validMaterialId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // Ensure only one essential material is returned
        var essentialMaterial = result.First();
        Assert.Equal("Essential Material 1", essentialMaterial.Name);
        Assert.Equal("Essential Text 1", essentialMaterial.DataText);
    }

    [Fact]
    public async Task TestListEssentialsByInvalidMaterialId()
    {
        // Arrange
        int invalidMaterialId = 999; // An ID that does not exist in the repository
        var mockEssentialMaterials = new List<EssentialMaterial>
        {
            new EssentialMaterial { Id = 1, MaterialName = "Essential Material 1", DataText = "Essential Text 1", TemplateId = 101 },
            new EssentialMaterial { Id = 2, MaterialName = "Essential Material 2", DataText = "Essential Text 2", TemplateId = 102 }
        };

        var mockRepository = new Mock<IMaterialsRepository>();
        mockRepository.Setup(r => r.ListEssentialsAsync()).ReturnsAsync(mockEssentialMaterials);

        var materialService = new MaterialService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => materialService.ListEssentialsByIdAsync(invalidMaterialId));
        Assert.Contains($"Material with ID: '{invalidMaterialId}'", exception.Message);
    }

    [Fact]
    public async Task TestAddAndGetMaterialDataDtoShowById()
    {
        var newMaterial = new MaterialDataDtoShow
        {
            Name = "Test Material",
            DataText = "Test Data",
            CourseId = "1",
            Id = "123"
        };

        Assert.Equal(newMaterial.Id, "123");
    }
























}