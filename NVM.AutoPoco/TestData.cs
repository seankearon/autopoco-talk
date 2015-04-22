using System;
using System.Linq;
using AutoPoco;
using AutoPoco.Configuration;
using AutoPoco.Contrib.DataSources;
using AutoPoco.DataSources;
using AutoPoco.Engine;

namespace NVM.AutoPoco
{
    public static class TestData
    {
        static TestData()
        {
            var factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c =>
                {
                    // Order of conventions important.
                    c.UseDefaultConventions();
                    c.Register(typeof(AddressConvention));
                });

                x.Include<Customer>()
                    .Setup(c => c.Id).Use<IntegerIdSource>()
                    .Setup(c => c.Name).Use<FullNameSource>()
                    .Setup(c => c.DateOfBirth).Use<DateOfBirthSource>()
                    .Invoke(c => c.SetPassword(
                      Use.Source<string, PasswordSource>()))
                    ;

                x.Include<Order>()
                    .Setup(c => c.Number).Use<OrderNumberSource>()
                    .Setup(c => c.Product).Use<ProductNameSource>()
                    .Setup(c => c.Total).Use<ProductPriceSource>()
                    ;

            });

            Session = factory.CreateSession();
        }

        public static IGenerationSession Session { get; set; }

        public static Customer[] Items
        {
            get
            {
                return Session.List<Customer>(200)
                    .Impose(x => x.Orders, Session.List<Order>(4).Get().ToArray())
                    .Get().ToArray();
            }
        }
    }

    public class OrderNumberSource : DatasourceBase<string>
    {
        readonly IntegerIdSource _idSource = new IntegerIdSource();
        public override string Next(IGenerationContext context)
        {
            return "Order " + _idSource.Next(context);
        }
    }

    public class PasswordSource : DatasourceBase<string>
    {
        private readonly string[] _values = {
            "Secret",
            "Pa55w0rd",
            "LetMeOut"
        };
        private readonly Random _random = new Random(1337);
        public override string Next(IGenerationContext context)
        {
            return _values[_random.Next(0, _values.Length)];
        }
    }

    public class ProductNameSource : DatasourceBase<string>
    {
        private readonly string[] _values = {
            "Beer",
            "Chips",
            "Gravy"
        };
        private readonly Random _random = new Random(1337);
        public override string Next(IGenerationContext context)
        {
            return _values[_random.Next(0, _values.Length)];
        }
    }

    public class ProductPriceSource : DatasourceBase<decimal>
    {
        private readonly decimal[] _values = {
            3m,
            2.5m,
            1.25m
        };
        private readonly Random _random = new Random(1337);
        public override decimal Next(IGenerationContext context)
        {
            return _values[_random.Next(0, _values.Length)];
        }
    }

    public class FullNameSource : DatasourceBase<string>
    {
        readonly FirstNameSource _firstNameSource = new FirstNameSource();
        readonly LastNameSource _lastNameSource = new LastNameSource();

        public override string Next(IGenerationContext context)
        {
            return _firstNameSource.Next(context) + " " + _lastNameSource.Next(context);
        }
    }

    public class AddressSource : IDatasource<string>
    {
        readonly UKStreetAddressSource _streets = new UKStreetAddressSource();
        readonly UKCitySource _cities = new UKCitySource();
        readonly UKCountySource _counties = new UKCountySource();
        readonly UKPostcodeSource _postcodes = new UKPostcodeSource();

        public object Next(IGenerationContext context)
        {
            return string.Join(Environment.NewLine,
                _streets.Next(context),
                _cities.Next(context),
                _counties.Next(context),
                _postcodes.Next(context));
        }
    }

    public class AddressConvention : ITypePropertyConvention
    {
        public void Apply(ITypePropertyConventionContext context)
        {
            context.SetSource<AddressSource>();
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Name(x => x.Contains("Address") && !x.Contains("Email"));
            requirements.Type(x => x == typeof(string));
        }
    }
}