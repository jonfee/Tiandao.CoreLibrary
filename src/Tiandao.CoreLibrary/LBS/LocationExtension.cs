using System;
using System.Collections.Generic;

namespace Tiandao.LBS
{
    public static class LocationExtension
    {
	    public static double GetDistance(this Location source, Location target)
	    {
			if(source == null)
				throw new ArgumentNullException("source");

			if(target == null)
				throw new ArgumentNullException("target");

		    return LocationUtility.GetDistance(source, target);
	    }

	    public static double GetDistance(this Location source, double latitude, double longitude)
	    {
			if(source == null)
				throw new ArgumentNullException("source");

		    return LocationUtility.GetDistance(source.Latitude, source.Longitude, latitude, longitude);
	    }

	    public static Position GetPosition(this Location source, double distance)
	    {
			if(source == null)
				throw new ArgumentNullException("source");

			return LocationUtility.GetPosition(source, distance);
	    }

		public static bool IsInRange(this Location source, Location target, double distance)
		{
			return LocationUtility.IsInRange(source, target, distance);
		}

	    public static bool IsInRange(this Location source, double latitude, double longitude, double distance)
	    {
			return LocationUtility.IsInRange(source, new Location(latitude, longitude), distance);
		}
    }
}