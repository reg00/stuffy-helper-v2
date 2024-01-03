using MediatR;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.Api.Features.Models.Request.UnitType
{
    public class GetUnitTypeListRequest : IRequest<Result<PagedData<UnitTypeShortEntry>>>
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public string? Name { get; set; }
        public Guid? PurchaseId { get; set; }
        public bool? IsActive { get; set; }

        public GetUnitTypeListRequest(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            Offset = offset;
            Limit = limit;
            Name = name;
            PurchaseId = purchaseId;
            IsActive = isActive;
        }
    }
}
