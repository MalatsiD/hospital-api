using Bogus;
using Hospital_API.Entities;

namespace Hospital_API.Data
{
    public class DataFakers
    {
        private Faker<Country>? _countryFaker = null;
        private Faker<Province>? _provinceFaker = null;

        public Faker<Country> GetCountryGenerator()
        {
            if (_countryFaker is null)
            {
                int id = 0;
                DateTime currentDate = DateTime.Now;

                _countryFaker = new Faker<Country>()
                    .RuleFor(c => c.Id, f => ++id)
                    .RuleFor(c => c.Name, f => f.Address.Country())
                    .RuleFor(c => c.Code, f => f.Address.CountryCode())
                    .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
                    .RuleFor(c => c.DateModified, f => currentDate)
                    .RuleFor(c => c.DateCreated, f => currentDate)
                    .RuleFor(c => c.Active, f => true);
            }

            return _countryFaker;
        }

        public Faker<Province> GetProvinceGenerator()
        {
            if (_provinceFaker is null)
            {
                int id = 0;
                DateTime currentDate = DateTime.Now;

                _provinceFaker = new Faker<Province>()
                    .RuleFor(c => c.Id, f => ++id)
                    .RuleFor(c => c.Name, f => f.Address.State())
                    .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
                    .RuleFor(c => c.DateModified, f => currentDate)
                    .RuleFor(c => c.DateCreated, f => currentDate)
                    .RuleFor(c => c.Active, f => true);
            }

            return _provinceFaker;
        }
    }
}
