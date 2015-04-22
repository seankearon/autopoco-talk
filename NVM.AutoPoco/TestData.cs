using System.Linq;
using AutoPoco;
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
                    .Setup(c => c.FirstName).Use<FirstNameSource>()
                    .Setup(c => c.LastName).Use<LastNameSource>()
                    ;

            });

            Session = factory.CreateSession();
        }

        public static IGenerationSession Session { get; set; }

        public static Customer[] Items
        {
            get
            {
                return Session.List<Customer>(2).Get().ToArray();
                /*
                                return new[]
                                {
                                    new Customer
                                    {
                                        Id = 1,
                                        FirstName = "Sean",
                                        LastName = "Kearon"
                                    },
                                    new Customer
                                    {
                                        Id = 2,
                                        FirstName = "Gillian",
                                        LastName = "Macdonald"
                                    }
                                };
                */
            }
        }
    }
}