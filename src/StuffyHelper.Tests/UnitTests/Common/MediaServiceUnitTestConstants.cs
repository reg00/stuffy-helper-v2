using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class MediaServiceUnitTestConstants
    {
        public static MediaEntry GetCorrectMediaEntry()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                FileName = "test.jpg",
                FileType = Minio.Features.Common.FileType.Jpg,
                IsPrimal = true,
                MediaType = MediaType.Image,
                Event = EventServiceUnitTestConstants.GetCorrectEventEntry()
            };
        }

        public static AddMediaEntry GetEmptyLinkMediaEntry()
        {
            return new()
            {
                MediaType = MediaType.Link
            };
        }

        public static AddMediaEntry GetEmptyFileMediaEntry()
        {
            return new()
            {
                MediaType = MediaType.Image
            };
        }

        public static AddMediaEntry GetCorrectAddMediaEntry()
        {
            return new()
            {
                MediaType = MediaType.Link,
                Link = "test"
            };
        }

        public static IList<MediaEntry> GetCorrectMediaEntries()
        {
            return new List<MediaEntry>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    EventId = Guid.NewGuid(),
                    FileName = "test.jpg",
                    FileType = Minio.Features.Common.FileType.Jpg,
                    IsPrimal = true,
                    MediaType = MediaType.Image
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    EventId = Guid.NewGuid(),
                    FileName = "test2.jpg",
                    FileType = Minio.Features.Common.FileType.Jpg,
                    IsPrimal = true,
                    MediaType = MediaType.Image
                }
            };
        }
    }
}
