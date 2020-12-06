namespace ArdalisRating
{
    class Program
    {
        /*    Identifiy Reponsibilities/Separation of Concerns
         *      Presistence
         *      Logging
         *      Validation
         *      Business Logic
         * 
         *    Tight Coupling
         *      Binds two (or more) details together in a way that's difficult to change.
         *      
         *    Loose Coupling
         *      Offers a modular way to choose which details are involved in particular operation.
         * 
         */
        static void Main(string[] args)
        {
            ILogger logger;
            IFilePolicySource dataAccess;

            logger = new ConsoleLogger();
            dataAccess = new FilePolicySource();

            logger.Information("Ardalis Insurance Rating System Starting...");

            var engine = new RatingEngine(logger, dataAccess);
            engine.Rate();

            if (engine.Rating > 0)
            {
                logger.Information($"Rating: {engine.Rating}");
            }
            else
            {
                logger.Information("No rating produced.");
            }

        }
    }
}
