using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CharacterDatabaseAPI.Controllers;
using CharacterDatabaseAPI.Services;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.Tests;

public class CharacterControllerTests
{
    private const string TestUniverse = "Test Universe";
    private const string TestName = "Test Name";

    [Fact]
    public async void Get_ReturnsAllCharacters_InAUniverse()
    {
        var controller = SetupGetAll(out var characters, out var characterService);

        var result = await controller.Get(TestUniverse);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(characters, okObjectResult.Value);
    }

    [Fact]
    public async void Get_ReturnsNothing_IfUniverseDoesntExist()
    {
        var controller = SetupGetAll(out var characters, out var characterService);

        var result = await controller.Get("Non-existent Universe");

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Empty(okObjectResult.Value as IEnumerable<Character>);
    }

    [Fact]
    public async void Get_ReturnsCharacter_InAUniverseWithName()
    {
        var controller = SetupGetOne(out var character, out var characterService);

        var result = await controller.Get(TestUniverse, TestName);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(character, okObjectResult.Value);
    }

    [Fact]
    public async void Get_ReturnsNull_IfNoCharacterInAUniverseWithName()
    {
        var controller = SetupGetOne(out var character, out var characterService);

        var result = await controller.Get(TestUniverse, "Non-existent Name");

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okObjectResult.Value);
    }

    [Fact]
    public async void Get_ReturnsCorrectCharacter_IfTwoCharactersInDifferentUniverses_WithSameName()
    {
        const string otherUniverse = "Other Universe";
        var otherCharacter = new Character()
        {
            Universe = otherUniverse,
            Name = TestName,
        };
        var controller = SetupGetOne(out var character, out var characterService);
        characterService.Setup(service => service.GetAsync(otherUniverse, TestName))
            .ReturnsAsync(otherCharacter);

        var result = await controller.Get(otherUniverse, TestName);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(otherCharacter, okObjectResult.Value);
    }

    [Fact]
    public async void Search_ReturnsAllCharactersInUniverse_WhenNoCategoryTypeAndValueGiven()
    {
        var controller = SetupGetAll(out var characters, out var characterService);
        characterService.Setup(service => service.SearchAsync(TestUniverse, null, null))
            .ReturnsAsync(characters);

        var result = await controller.Search(TestUniverse);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(characters, okObjectResult.Value);
    }

    [Fact]
    public async void Search_ReturnsCorrectCharacter_InUniverseWithCategoryTypeAndValue()
    {
        const string categoryType = "Test Type";
        const string categoryValue = "Test Value";
        var controller = SetupGetOne(out var character, out var characterService);
        character.CategorySets = new Dictionary<string, string>() { { categoryType, categoryValue } };
        var characters = new List<Character> { character };
        characterService.Setup(service => service.SearchAsync(TestUniverse, categoryType, categoryValue))
            .ReturnsAsync(characters);

        var result = await controller.Search(TestUniverse, categoryType, categoryValue);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(characters, okObjectResult.Value);
    }

    [Fact]
    public async void Search_ReturnsNothing_IfNoCharacter_InUniverseWithCategoryTypeAndValue()
    {
        var controller = SetupGetOne(out var character, out var characterService);

        var result = await controller.Search(TestUniverse, "Test Type", "Non-existent Value");

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Empty(okObjectResult.Value as IEnumerable<Character>);
    }

    [Fact]
    public async void Search_ReturnsError_IfCategoryTypeGivenWithoutValue()
    {
        const string categoryType = "Test Type";
        var controller = SetupGetOne(out var character, out var characterService);
        characterService.Setup(service => service.SearchAsync(TestUniverse, categoryType, null))
            .Throws(new ArgumentNullException());

        var result = await controller.Search(TestUniverse, categoryType);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void Search_ReturnsError_IfCategoryValueGivenWithoutType()
    {
        const string categoryValue = "Test Value";
        var controller = SetupGetOne(out var character, out var characterService);
        characterService.Setup(service => service.SearchAsync(TestUniverse, null, categoryValue))
            .Throws(new ArgumentNullException());

        var result = await controller.Search(TestUniverse, null, categoryValue);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    private CharacterController SetupGetAll(out List<Character> characters, out Mock<ICharacterService> characterService)
    {
        characters = GetCharacters();
        characterService = GetCharacterServiceMock(characters);
        return new CharacterController(characterService.Object);
    }

    private CharacterController SetupGetOne(out Character character, out Mock<ICharacterService> characterService)
    {
        character = new Character()
        {
            Universe = TestUniverse,
            Name = TestName,
        };
        characterService = GetCharacterServiceMock(new List<Character>() { character });
        characterService.Setup(service => service.GetAsync(TestUniverse, TestName))
            .ReturnsAsync(character);
        return new CharacterController(characterService.Object);
    }

    private Mock<ICharacterService> GetCharacterServiceMock(List<Character>? characters = null)
    {
        var characterService = new Mock<ICharacterService>();
        if (characters is not null)
            characterService.Setup(service => service.GetAsync(TestUniverse))
                .ReturnsAsync(characters);

        return characterService;
    }
    private List<Character> GetCharacters()
    {
        var characters = new List<Character>();
        characters.Add(new Character()
        {
            Universe = TestUniverse,
            Name = "Test One",
        });
        characters.Add(new Character()
        {
            Universe = TestUniverse,
            Name = "Test Two",
        });
        return characters;
    }
}