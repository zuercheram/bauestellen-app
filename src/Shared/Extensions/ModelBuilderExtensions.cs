using Baustellen.App.Shared.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baustellen.App.Shared.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void IsTrackingEntity<T>(this EntityTypeBuilder<T> typeBuilder) where T : TrackingEntityBase
        {
            typeBuilder.Property(p => p.ModifiedAt).IsConcurrencyToken();
        }
    }
}
