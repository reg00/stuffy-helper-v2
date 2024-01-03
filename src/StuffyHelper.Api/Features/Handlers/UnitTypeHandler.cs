using MediatR;
using StuffyHelper.Api.Features.Models.Request.UnitType;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.Api.Features.Handlers
{
    public class UnitTypeHandler : IRequestHandler<GetUnitTypeListRequest, Result<PagedData<UnitTypeShortEntry>>>
    {
        private readonly IUnitTypeService _unitTypeService;

        public UnitTypeHandler(IUnitTypeService unitTypeService)
        {
            _unitTypeService = unitTypeService;
        }

        public async Task<Result<PagedData<UnitTypeShortEntry>>> Handle(GetUnitTypeListRequest request, CancellationToken cancellationToken)
        {
            var a = await _unitTypeService.GetUnitTypesAsync(
                request.Offset,
                request.Limit,
                request.Name,
                request.PurchaseId,
                request.IsActive,
                cancellationToken);

            return a;
        }
    }
}
