using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Utils
{
    public class MapDistanceHelper
    {
        public static readonly double EarthRadiusKm = 6371.0;
        public static List<EntityDistance> CalculateDistance(EntityDistance originEntity, List<EntityDistance> entities)
        {
            double lat1 = DegreesToRadians(originEntity.Lat);
            double lon1 = DegreesToRadians(originEntity.Lon);

            List<EntityDistance> result = new List<EntityDistance>();

            foreach (EntityDistance entity in entities)
            {
                double lat2 = DegreesToRadians(entity.Lat);
                double lon2 = DegreesToRadians(entity.Lon);

                double dLat = lat2 - lat1;
                double dLon = lon2 - lon1;

                // Haversine formula
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(lat1) * Math.Cos(lat2) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                // Distance in kilometers
                double distance = EarthRadiusKm * c;

                // Store distance in entity
                entity.Distance = distance;
                result.Add(entity);
            }

            List<EntityDistance> finalOrderdResult = result.OrderBy(e => e.Distance).ToList();
            return finalOrderdResult;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
