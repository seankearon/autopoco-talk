namespace NVM.AutoPoco
{
    public static class TestData
    {
        public static Customer[] Items 
        {
            get
            {
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
            }
        }
    }
}