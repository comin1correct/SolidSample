using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace ArdalisRating
{
    /// <summary>
    /// The RatingEngine reads the policy application details from a file and produces a numeric 
    /// rating value based on the details.
    /// </summary>
    public class RatingEngine
    {

        private readonly ILogger _logger;
        private readonly IFilePolicySource _dataAccess;
        private PolicySerializer PolicySerializer = new PolicySerializer();

        public RatingEngine(ILogger logger, IFilePolicySource filePolicy)
        {
            _logger = logger;
            _dataAccess = filePolicy;
        }

        public decimal Rating { get; set; }
        public void Rate()
        {
            _logger.Information("Starting rate.");                                  //<--- Logging

            _logger.Information("Loading policy.");

            // load policy - open file policy.json
            string policyJson = _dataAccess.GetPolicyFromSource();                  //<--- Persistence/Data Access

            var policy = PolicySerializer.GetPolicyFromJsonString(policyJson);      //<--- Hard coded Format json... yaml, xml, binary format this will have to change

            switch (policy.Type)
            {
                case PolicyType.Auto:                                               //<--- Business Rules
                    _logger.Information("Rating AUTO policy...");
                    _logger.Information("Validating policy.");
                    if (String.IsNullOrEmpty(policy.Make))                          //<--- Validation
                    {
                        _logger.Information("Auto policy must specify Make");
                        return;
                    }
                    if (policy.Make == "BMW")
                    {
                        if (policy.Deductible < 500)
                        {
                            Rating = 1000m;
                        }
                        Rating = 900m;
                    }
                    break;

                case PolicyType.Land:
                    _logger.Information("Rating LAND policy...");
                    _logger.Information("Validating policy.");
                    if (policy.BondAmount == 0 || policy.Valuation == 0)
                    {
                        _logger.Information("Land policy must specify Bond Amount and Valuation.");
                        return;
                    }
                    if (policy.BondAmount < 0.8m * policy.Valuation)
                    {
                        _logger.Information("Insufficient bond amount.");
                        return;
                    }
                    Rating = policy.BondAmount * 0.05m;
                    break;

                case PolicyType.Life:
                    _logger.Information("Rating LIFE policy...");
                    _logger.Information("Validating policy.");
                    if (policy.DateOfBirth == DateTime.MinValue)
                    {
                        _logger.Information("Life policy must include Date of Birth.");
                        return;
                    }
                    if (policy.DateOfBirth < DateTime.Today.AddYears(-100))
                    {
                        _logger.Information("Centenarians are not eligible for coverage.");
                        return;
                    }
                    if (policy.Amount == 0)
                    {
                        _logger.Information("Life policy must include an Amount.");
                        return;
                    }
                    int age = DateTime.Today.Year - policy.DateOfBirth.Year;                    //<--- Age calculation... does not need to be next to high level concerns of how we want to rate these policies
                    if (policy.DateOfBirth.Month == DateTime.Today.Month &&
                        DateTime.Today.Day < policy.DateOfBirth.Day ||
                        DateTime.Today.Month < policy.DateOfBirth.Month)
                    {
                        age--;
                    }
                    decimal baseRate = policy.Amount * age / 200;
                    if (policy.IsSmoker)
                    {
                        Rating = baseRate * 2;
                        break;
                    }
                    Rating = baseRate;
                    break;

                default:
                    _logger.Information("Unknown policy type");
                    break;
            }

            _logger.Information("Rating completed.");
        }
    }
}
