using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using FinanceUi.Infrastructure.Mapping;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceUi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IBoardRepository, BoardRepository>()
            .AddScoped<IIncomeRepository, IncomeRepository>()
            .AddScoped<IGoalRepository, GoalRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>()
			.AddSingleton<IObjectMapper>(GetMapper());
    }

    public static ObjectMapper GetMapper()
    {
		var mapper = new ObjectMapper();

		//Boards
		mapper.Register<Board, BoardDto>(b => new BoardDto
		{
			Id = b.Id,
			Title = b.Title,
			PaidTotalGoal = b.Goals?.Sum(g => g.PaidAmount) ?? 0,
			TotalGoal = b.Goals?.Sum(g => g.Amount) ?? 0
		});
		mapper.Register<BoardDto, Board>(dto => new Board
		{
			Id = dto.Id,
			Title = dto.Title
		});

		mapper.Register<Board, BriefBoardDto>(b => new BriefBoardDto()
		{
			Id = b.Id,
			Title = b.Title
		});
		mapper.Register<BriefBoardDto, Board>(dto => new Board
		{
			Title = dto.Title
		});

		//Goals
		mapper.Register<Goal, GoalDto>(g => new GoalDto
		{
			Id = g.Id,
			Title = g.Title,
			Amount = g.Amount,
			PaidAmount = g.PaidAmount,
			BoardId = g.BoardId,
			BoardName = g.Board?.Title ?? string.Empty,
			Deadline = g.Deadline,
			PaymentsCount = g.Payments?.Count ?? 0,
		});
		mapper.Register<GoalDto, Goal>(dto => new Goal
		{
			Id = dto.Id,
			Title = dto.Title,
			Amount = dto.Amount,
			PaidAmount = dto.PaidAmount,
			BoardId = dto.BoardId,
			Deadline = dto.Deadline,
		});

		mapper.Register<Goal, BriefGoalDto>(g => new BriefGoalDto
		{
			Id = g.Id,
			Title = g.Title,
			Amount = g.Amount,
			PaidAmount = g.PaidAmount,
			BoardId = g.BoardId,
			Deadline = g.Deadline
		});
		mapper.Register<BriefGoalDto, Goal>(dto => new Goal
		{
			Id = dto.Id,
			Title = dto.Title,
			Amount = dto.Amount,
			PaidAmount = dto.PaidAmount,
			BoardId = dto.BoardId,
			Deadline = dto.Deadline,
		});

		//Incomes
		mapper.Register<Income, IncomeDto>(i => new IncomeDto
		{
			Id = i.Id,
			Name = i.Name,
			Amount = i.Amount,
			BoardId = i.BoardId,
			BoardTitle = i.Board?.Title ?? string.Empty,
			Date = i.Date,
			Periodicity = i.Periodicity
		});
		mapper.Register<IncomeDto, Income>(dto => new Income
		{
			Id = dto.Id,
			Name = dto.Name,
			Amount = dto.Amount,
			BoardId = dto.BoardId,
			Date = dto.Date,
			Periodicity = dto.Periodicity
		});

		mapper.Register<Income, BriefIncomeDto>(i => new BriefIncomeDto
		{
			Id = i.Id,
			Name = i.Name,
			Amount = i.Amount,
			BoardId = i.BoardId,
			Date = i.Date,
			Periodicity = i.Periodicity
		});
		mapper.Register<BriefIncomeDto, Income>(dto => new Income
		{
			Id = dto.Id,
			Name = dto.Name,
			Amount = dto.Amount,
			BoardId = dto.BoardId,
			Date = dto.Date,
			Periodicity = dto.Periodicity
		});

		//Payments
		mapper.Register<Payment, PaymentDto>(p => new PaymentDto
		{
			Id = p.Id,
			Name = p.Name,
			Amount = p.Amount,
			BoardId = p.BoardId,
			BoardTitle = p.Board?.Title ?? string.Empty,
			GoalId = p.GoalId,
			GoalTitle = p.Goal?.Title,
			Periodicity = p.Periodicity
		});
		mapper.Register<PaymentDto, Payment>(dto => new Payment
		{
			Id = dto.Id,
			Name = dto.Name,
			Amount = dto.Amount,
			BoardId = dto.BoardId,
			GoalId = dto.GoalId,
			Periodicity = dto.Periodicity
		});

		mapper.Register<Payment, BriefPaymentDto>(p => new BriefPaymentDto
		{
			Id = p.Id,
			Name = p.Name,
			Amount = p.Amount,
			BoardId = p.BoardId,
			GoalId = p.GoalId,
			Periodicity = p.Periodicity
		});
		mapper.Register<BriefPaymentDto, Payment>(dto => new Payment
		{
			Id = dto.Id,
			Name = dto.Name,
			Amount = dto.Amount,
			BoardId = dto.BoardId,
			GoalId = dto.GoalId,
			Periodicity = dto.Periodicity
		});

		return mapper;
	}
}