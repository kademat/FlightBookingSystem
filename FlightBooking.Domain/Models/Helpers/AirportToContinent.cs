using FlightBooking.Domain.Enums;

namespace FlightBooking.Domain.Models.Helpers
{
    /// <summary>
    /// A static class for mapping airport codes (IATA) to their respective continents.
    /// This mapping is intended as a static example and does not represent a complete or dynamic solution.
    /// 
    /// In a production environment, it is recommended to use a reliable external API or database to retrieve 
    /// airport-to-continent mappings. This approach ensures up-to-date data and automatic handling of new airports 
    /// or changes in airport information.
    /// 
    /// Example use case:
    /// - Check if a flight destination is in Africa based on its IATA code.
    /// 
    /// Note:
    /// - This static mapping contains an incomplete list of African airport codes and examples from other continents 
    ///   (e.g., Europe, North America, Asia).
    /// - Adding new airport codes manually requires maintaining this list.
    /// </summary>
    internal static class AirportContinentMapper
    {
        /// <summary>
        /// A dictionary mapping airport IATA codes to continents.
        /// Keys represent three-letter IATA codes, and values represent continent enums.
        /// </summary>
        private static readonly Dictionary<string, Continent> MapAirportToContinent = new()
        {
            { "JNB", Continent.Africa }, // Johannesburg, South Africa
            { "CPT", Continent.Africa }, // Cape Town, South Africa
            { "LOS", Continent.Africa }, // Lagos, Nigeria
            { "NBO", Continent.Africa }, // Nairobi, Kenya
            { "CAI", Continent.Africa }, // Cairo, Egypt
            { "ADD", Continent.Africa }, // Addis Ababa, Ethiopia
            { "CMN", Continent.Africa }, // Casablanca, Morocco
            { "DUR", Continent.Africa }, // Durban, South Africa
            { "KRT", Continent.Africa }, // Khartoum, Sudan
            { "DAR", Continent.Africa }, // Dar es Salaam, Tanzania
            { "ABJ", Continent.Africa }, // Abidjan, Ivory Coast
            { "HRE", Continent.Africa }, // Harare, Zimbabwe
            { "ALG", Continent.Africa }, // Algiers, Algeria
            { "SEZ", Continent.Africa }, // Seychelles International Airport, Seychelles
            { "TUN", Continent.Africa }, // Tunis, Tunisia
            { "EBB", Continent.Africa }, // Entebbe, Uganda
            { "FIH", Continent.Africa }, // Kinshasa, Democratic Republic of the Congo
            { "BGF", Continent.Africa }, // Bangui, Central African Republic
            { "ROB", Continent.Africa }, // Monrovia, Liberia
            { "LFW", Continent.Africa }, // Lomé, Togo
            { "KGL", Continent.Africa }, // Kigali, Rwanda
            { "MPM", Continent.Africa }, // Maputo, Mozambique
            { "BJL", Continent.Africa }, // Banjul, Gambia
            { "OUA", Continent.Africa }, // Ouagadougou, Burkina Faso
            { "BLZ", Continent.Africa }, // Blantyre, Malawi
            { "FNA", Continent.Africa }, // Freetown, Sierra Leone
            { "MBA", Continent.Africa }, // Mombasa, Kenya
            { "ABV", Continent.Africa }, // Abuja, Nigeria
            { "ASW", Continent.Africa }, // Aswan, Egypt
            { "CKY", Continent.Africa }, // Conakry, Guinea
            { "DKR", Continent.Africa }, // Dakar, Senegal
            { "SSG", Continent.Africa }, // Malabo, Equatorial Guinea
            { "KIS", Continent.Africa }, // Kisumu, Kenya
            { "LUN", Continent.Africa }, // Lusaka, Zambia
            { "HGA", Continent.Africa }, // Hargeisa, Somalia
            { "MGQ", Continent.Africa }, // Mogadishu, Somalia
            { "WDH", Continent.Africa }, // Windhoek, Namibia
            { "LAD", Continent.Africa }, // Luanda, Angola
            { "MRU", Continent.Africa }, // Port Louis, Mauritius
            { "RUN", Continent.Africa }, // Réunion
            { "LHR", Continent.Europe }, // London Heathrow
            { "JFK", Continent.NorthAmerica }, // New York JFK
            { "HND", Continent.Asia }, // Tokyo Haneda
        };

        /// <summary>
        /// Gets the continent for a given airport IATA code.
        /// </summary>
        /// <param name="airportCode">The IATA code of the airport.</param>
        /// <returns>The continent enum, or null if the code is not mapped.</returns>
        public static Continent? GetContinent(string airportCode)
        {
            if (MapAirportToContinent.TryGetValue(airportCode.ToUpper(), out var continent))
            {
                return continent;
            }
            return null;
        }
    }
}