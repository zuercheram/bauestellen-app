using Baustellen.App.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baustellen.App.Domain.Extensions
{
    internal static class ModelBuilderExtensions
    {
        internal static void IsTrackingEntity<T>(this EntityTypeBuilder<T> typeBuilder) where T : TrackingEntityBase
        {
            typeBuilder.Property(p => p.ModifiedAt).IsConcurrencyToken();
        }
    }
}
