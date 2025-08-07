using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Dtos.Planning;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Application.Services
{
    public class PlanningService : IPlanningService
    {
        private readonly IPlanningRepository _planningRepository;

        public PlanningService(IPlanningRepository planningRepository)
        {
            _planningRepository = planningRepository;
        }

        public async Task<Column> GetColumn(GetColumnDto dto)
        {
            return await _planningRepository.GetColumn(dto);
        }

        public async Task<ICollection<Column>> GetColumns(GetColumnsDto dto)
        {
            return await _planningRepository.GetColumns(dto);
        }
    }
}
