﻿using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfUnitTypeStore : IUnitTypeStore
    {
        private readonly StuffyHelperContext _context;

        public EfUnitTypeStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<UnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            try
            {
                var entry = await _context.UnitTypes
                    .FirstOrDefaultAsync(e => e.Id == unitTypeId, cancellationToken);

                if (entry is null)
                    throw new ResourceNotFoundException($"UnitType with Id '{unitTypeId}' Not Found.");

                return entry;
            }
            catch (ResourceNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }

        }

        public async Task<Response<UnitTypeEntry>> GetUnitTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.UnitTypes
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (purchaseId == null || e.Purchases.Any(x => x.Id == purchaseId)))
                    .OrderByDescending(e => e.Name)
                    .ToListAsync(cancellationToken);

                return new Response<UnitTypeEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<UnitTypeEntry> AddUnitTypeAsync(UnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));

            try
            {
                var entry = await _context.UnitTypes.AddAsync(unitType, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task DeleteUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            try
            {
                var unitType = await _context.UnitTypes
                    .FirstOrDefaultAsync(
                    s => s.Id == unitTypeId, cancellationToken);

                if (unitType is null)
                {
                    throw new ResourceNotFoundException($"UnitType with Id '{unitTypeId}' not found.");
                }

                unitType.IsActive = false;

                _context.UnitTypes.Update(unitType);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<UnitTypeEntry> UpdateUnitTypeAsync(UnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));

            try
            {
                var entry = _context.UnitTypes.Update(unitType);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }
    }
}