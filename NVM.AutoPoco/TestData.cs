using System;
using System.Linq;
using AutoPoco;
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
                    c.UseDefaultConventions();
                });

                x.Include<Customer>()
                    .Setup(c => c.Id).Use<IntegerIdSource>()
                    .Setup(c => c.Name).Use<FullNameSource>()
                    .Setup(c => c.Address).Use<AddressSource>()
                    .Setup(c => c.DateOfBirth).Use<DateOfBirthSource>()
                    ;

            });

            Session = factory.CreateSession();
        }

        public static IGenerationSession Session { get; set; }

        public static Customer[] Items
        {
            get
            {
                return Session.List<Customer>(200).Get().ToArray();
            }
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
}