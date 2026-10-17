using System;
using System.Threading.Tasks;
using Inno.Extensibility.Catalogs;
using Inno.Runtime;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class StaticGenericFactoryTests
{
    [Fact]
    public void FactoriesDoNotIntroduceClosedConstructionsIntoDiscovery()
    {
        var source = new StaticTypeCatalogSource([register =>
        {
            register.Register(Metadata(typeof(Box<>)), null);
            register.RegisterFactory(typeof(Box<int>), static () => new Box<int>());
            register.RegisterFactory(typeof(Box<int>), static () => throw new InvalidOperationException("Duplicate factory must not execute."));
        }]);

        Assert.Equal(typeof(Box<int>), source.ConstructGenericType(typeof(Box<>), [typeof(int)]));
        Assert.IsType<Box<int>>(source.CreateInstance(typeof(Box<int>)));
        Assert.DoesNotContain(typeof(Box<int>), source.GetTypes(typeof(Box<>).Assembly));
        Assert.Throws<NotSupportedException>(() => source.ConstructGenericType(typeof(Box<>), [typeof(string)]));
    }

    [Fact]
    public void AConstructionRequiresItsOwningDeclaration()
        => Assert.Throws<ArgumentException>(() => new StaticTypeCatalogSource([
            register => register.RegisterFactory(typeof(Box<int>), static () => new Box<int>())]));

    [Fact]
    public void RetainedRegistrarCannotMutateThePublishedCatalog()
    {
        ITypeCatalogRegistrar? retained = null;
        var source = new StaticTypeCatalogSource([register =>
        {
            retained = register;
            register.Register(Metadata(typeof(Box<>)), null);
        }]);

        Assert.Throws<InvalidOperationException>(() => retained!.RegisterFactory(typeof(Box<int>), static () => new Box<int>()));
        Assert.False(source.CanCreateInstance(typeof(Box<int>)));
    }

    [Fact]
    public void FailedContributorAlsoRetiresItsRegistrar()
    {
        ITypeCatalogRegistrar? retained = null;
        Assert.Throws<InvalidOperationException>(() => new StaticTypeCatalogSource([register =>
        {
            retained = register;
            throw new InvalidOperationException("The contribution failed.");
        }]));
        Assert.Throws<InvalidOperationException>(() => retained!.Register(Metadata(typeof(Box<>)), null));
    }

    [Fact]
    public void ContributorCannotTransferRegistrationToAnotherThread()
        => new StaticTypeCatalogSource([register => Task.Run(() =>
            Assert.Throws<InvalidOperationException>(() => register.Register(Metadata(typeof(Box<>)), null)))
            .GetAwaiter().GetResult()]);

    [Fact]
    public void GenericArgumentValidationIsIndependentOfAvailableFactories()
    {
        var source = new StaticTypeCatalogSource([register => register.Register(Metadata(typeof(Box<>)), null)]);
        Assert.Throws<ArgumentException>(() => source.ConstructGenericType(typeof(Box<>), []));
        Assert.Throws<ArgumentException>(() => source.ConstructGenericType(typeof(Box<>), [typeof(Box<>)]));
        Assert.Throws<ArgumentException>(() => source.ConstructGenericType(typeof(string), []));
    }

    [Fact]
    public void ConstraintRejectionsAreCopiedAndRemainDistinctFromMissingFactories()
    {
        Type[] arguments = [typeof(string)];
        var source = new StaticTypeCatalogSource([register =>
        {
            register.Register(Metadata(typeof(Box<>)), null);
            register.RejectGenericConstruction(typeof(Box<>), arguments);
            register.RejectGenericConstruction(typeof(Box<>), arguments);
        }]);
        arguments[0] = typeof(int);
        Assert.Null(source.ConstructGenericType(typeof(Box<>), [typeof(string)]));
        Assert.Throws<NotSupportedException>(() => source.ConstructGenericType(typeof(Box<>), [typeof(int)]));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ConstraintFactsRequireADeclarationAndCannotContradictLinkedFactories(bool declaration)
        => Assert.Throws<ArgumentException>(() => new StaticTypeCatalogSource([register =>
        {
            if (declaration)
            {
                register.Register(Metadata(typeof(Box<>)), null);
                register.RegisterFactory(typeof(Box<int>), static () => new Box<int>());
            }
            register.RejectGenericConstruction(typeof(Box<>), [typeof(int)]);
        }]));

    [Fact]
    public void IncompatibleFactoryResultFailsExplicitly()
    {
        var source = new StaticTypeCatalogSource([register =>
        {
            register.Register(Metadata(typeof(Box<>)), null);
            register.RegisterFactory(typeof(Box<int>), static () => "invalid");
        }]);
        Assert.Throws<InvalidOperationException>(() => source.CreateInstance(typeof(Box<int>)));
    }

    private static TypeCatalogMetadata Metadata(Type type) => new(type, [], [], [], [], []);

    public sealed class Box<T>;
}
