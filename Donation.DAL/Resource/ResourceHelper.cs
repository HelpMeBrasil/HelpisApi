using System.Globalization;
using System.Resources;
using System.Threading;

namespace Donation.Infrastructure.Models
{
    public static class ResourceHelper
    {
        public static ResourceSet GetResources(string culture)
        {
            CultureInfo info;

            try
            {
                info = new CultureInfo(culture);
            }
            catch (CultureNotFoundException)
            {
                info = Thread.CurrentThread.CurrentUICulture;
            }

            ResourceManager resources = new ResourceManager("Donation.DAL.Resource.Resource", typeof(ResourceHelper).Assembly);

            return resources.GetResourceSet(CultureInfo.CreateSpecificCulture(info.Name), true, false);
        }
    }
}